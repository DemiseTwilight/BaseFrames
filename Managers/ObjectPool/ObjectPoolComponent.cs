using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace ObjectPool {
    /// <summary>
    /// 对象池组件
    /// 挂载在根节点以使用
    /// 如要手动创建，请需要生成的对象后使用Initialized方法进行初始化
    /// </summary>
    public class ObjectPoolComponent : MonoBehaviour {
        private readonly List<GameObject> _objects=new List<GameObject>();
        private ObjectPool<GameObject> _pool;
        private readonly Dictionary<int,IReusable> _reusableDictionary=new Dictionary<int, IReusable>();
        private bool _isReusableObject = false;
        public bool collectionChecks = true;
        public int defaultCapacity = 10;
        public int maxPoolSize = 100;
        /// <summary>
        /// 预生成对象数
        /// </summary>
        public int preInstantiate = 10;
        /// <summary>
        /// 需要生成的预制体
        /// </summary>
        public GameObject prefab;
        /// <summary>
        /// 不活跃对象数
        /// </summary>
        public int CountInactive =>  _pool.CountInactive;
        /// <summary>
        /// 活跃对象数
        /// </summary>
        public int CountActive =>  _pool.CountActive;
        /// <summary>
        /// 对象总数
        /// </summary>
        public int CountAll =>  _pool.CountAll;

        private void Awake() {
            Initialized();
        }
        private GameObject OnCreateObject() {
            GameObject obj = Instantiate(prefab,transform);
            obj.name = prefab.name;
            _objects.Add(obj);
            if (_isReusableObject) {
                IReusable reusableObject=obj.GetComponent<IReusable>();
                reusableObject.Belong = this;
                if (!_reusableDictionary.TryAdd(obj.GetInstanceID(), reusableObject)) {
                    Debug.LogError("可重用对象脚本添加字典失败，请检查");
                }
            }
            return obj;
        }

        private void OnGet(GameObject getObj) {
            getObj.SetActive(true);
        }

        private void OnRelease(GameObject releaseObj) {
            releaseObj.SetActive(false);
        }

        private void OnDestroyObj(GameObject destroyObj) {
            _objects.Remove(destroyObj);
            if (_isReusableObject) {
                _reusableDictionary.Remove(destroyObj.GetInstanceID());
            }
            Destroy(destroyObj);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 加载可重用对象脚本
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private IReusable LoadReusableScript(GameObject obj) {
            if (_reusableDictionary.TryGetValue(obj.GetInstanceID(), out var reusableScript)) {
                return reusableScript;
            }
            else {
                Debug.LogError("可重用对象脚本不存在于字典中，请检查");
                return null;
            }
        }

        /// <summary>
        /// 预生成
        /// </summary>
        private void PreInstantiate() {
            for (int i = 0; i < preInstantiate; i++) {
                _pool.Get(out var obj);
            }
            ReleaseAll();
        }
        
        /// <summary>
        /// 初始化组件，对象池组件必须在调用初始化后才能使用，如果不存在生成目标，则初始化失败
        /// 初始化后，原先生成的对象将全部丢失
        /// </summary>
        public void Initialized() {
            if (_pool !=null) {
                _pool.Clear();
            }
            if (prefab) {
                _pool = new ObjectPool<GameObject>(OnCreateObject, OnGet, OnRelease, OnDestroyObj,
                    collectionChecks,defaultCapacity, maxPoolSize);
                _isReusableObject = prefab.GetComponent<IReusable>() != null;
                PreInstantiate();
            }
            else {
                Debug.LogWarning("对象池初始化失败，请设置需要生成的对象并重新调用Initialized进行初始化");
            }
        }

        /// <summary>
        /// 取出对象
        /// </summary>
        /// <returns>取出的对象</returns>
        public GameObject Get() {
            var getObj = _pool.Get();
            if (_isReusableObject) {
                LoadReusableScript(getObj)?.Get();
            }
            return getObj;
        }

        public GameObject Get(Transform parent) {
            var obj = Get();
            obj.transform.SetParent(parent);
            return obj;
        }

        public GameObject Get(Vector3 position,Quaternion rotation) {
            var obj = Get();
            obj.transform.SetPositionAndRotation(position,rotation);
            return obj;
        }

        public GameObject Get(Vector3 position,Quaternion rotation,Transform parent) {
            var obj = Get();
            obj.transform.SetParent(parent);
            obj.transform.SetPositionAndRotation(position,rotation);
            return obj;
        }
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 放回对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="triggerResponse"></param>
        public bool Release(GameObject obj, bool triggerResponse = true) {
            try {
                if (_isReusableObject && triggerResponse) {
                    LoadReusableScript(obj)?.Release();
                }
                _pool.Release(obj);
                return true;
            }
            catch (Exception e) {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
       /// 放回全部对象
       /// </summary>
       /// <param name="triggerResponse">需要响应对可重用对象的回收回调</param>
        public void ReleaseAll(bool triggerResponse=false) {
            foreach (var obj in _objects) {
                if (obj.activeSelf) {
                    Release(obj,triggerResponse);
                }
            }
        }
       /// <summary>
        /// 清除对象池
        /// </summary>
        public void Clear() {
           ReleaseAll();
            _pool.Clear();
        }
    }

}

