using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool {
    /// <summary>
    /// 对象池管理器，用于管理多项对象池
    /// </summary>
    public class ObjectPoolManager : SingletonBase<ObjectPoolManager> {
        private Dictionary<string, ObjectPoolComponent> _poolDatas = new Dictionary<string, ObjectPoolComponent>();

        public GameObject Get(string objName) {
            if (_poolDatas.ContainsKey(objName) == false) {
                CreateObjectPool(objName);
            }
            
            var pool = _poolDatas[objName];
            return pool.Get();
        }
        
        public GameObject Get(GameObject prefab) {
            if (_poolDatas.ContainsKey(prefab.name) == false) {
                CreateObjectPool(prefab);
            }
            
            var pool = _poolDatas[prefab.name];
            return pool.Get();
        }
        /// <summary>
        /// 创建对象池
        /// </summary>
        public ObjectPoolComponent CreateObjectPool(string objName,bool collectionChecks = true,int defaultCapacity = 10,int maxPoolSize = 100,int preInstantiate = 0) {
            var prefab = Resources.Load<GameObject>(objName);
            return CreateObjectPool(prefab, collectionChecks, defaultCapacity, maxPoolSize, preInstantiate);
        }
        /// <summary>
        /// 创建对象池
        /// </summary>
        public ObjectPoolComponent CreateObjectPool(GameObject prefab,bool collectionChecks = true,int defaultCapacity = 10,int maxPoolSize = 100,int preInstantiate = 0) {
            var boot = new GameObject(prefab.name);
            var component = boot.AddComponent<ObjectPoolComponent>();
            FillObjectPoolComponent(ref component, prefab, collectionChecks, defaultCapacity, maxPoolSize,
                preInstantiate);
            _poolDatas.Add(prefab.name, component);
            return component;
        }

        /// <summary>
        /// 回收对象
        /// 如果对象不在池中放回false
        /// </summary>
        /// <param name="obj">要回收的对象</param>
        public bool Release(GameObject obj) {
            if (_poolDatas.TryGetValue(obj.name,out var poolComponent)){
                return poolComponent.Release(obj);
            }
            return false;
        }
        /// <summary>
        /// 放回全部对象
        /// </summary>
        /// <param name="triggerResponse">需要响应对可重用对象的回收回调</param>
        public void ReleaseAll(bool triggerResponse=false) {
            foreach (var pool in _poolDatas.Values) {
                pool.ReleaseAll(triggerResponse);
            }
        }
        /// <summary>
        /// 清理，切换场景时请调用
        /// </summary>
        public void Clear() {
            foreach (var pool in _poolDatas.Values) {
                pool.Clear();
                Destroy(pool.gameObject);
            }
            _poolDatas.Clear();
        }

        /// <summary>
        /// 重设对象池数据
        /// </summary>
        /// <param name="component"></param>
        /// <param name="prefab"></param>
        /// <param name="collectionChecks"></param>
        /// <param name="defaultCapacity"></param>
        /// <param name="maxPoolSize"></param>
        /// <param name="preInstantiate"></param>
        private void FillObjectPoolComponent(ref ObjectPoolComponent component,GameObject prefab,bool collectionChecks=true,int defaultCapacity=10,int maxPoolSize = 100,int preInstantiate = 0) {
            component.prefab = prefab;
            component.collectionChecks = collectionChecks;
            component.defaultCapacity = defaultCapacity;
            component.maxPoolSize = maxPoolSize;
            component.preInstantiate = preInstantiate;
            component.Initialized();
        }
    }
}