using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace ObjectPool {
    public class ObjectPoolComponent : MonoBehaviour {
        private readonly List<GameObject> _objects=new List<GameObject>();
        private ObjectPool<GameObject> _pool;
        private readonly Dictionary<int,IReusable> _reusableDictionary=new Dictionary<int, IReusable>();
        private bool _isReusableObject = false;
        public bool collectionChecks = true;
        public int defaultCapacity = 10;
        public int maxPoolSize = 100;
        /// <summary>
        /// 需要生成的预制体
        /// </summary>
        public GameObject prefab;

        public int CountInactive =>  _pool.CountInactive;
        public int CountActive =>  _pool.CountActive;
        public int CountAll =>  _pool.CountAll;

        private void Awake() {
            _pool = new ObjectPool<GameObject>(OnCreateObject, OnGet, OnRelease, OnDestroyObj,
                collectionChecks,defaultCapacity, maxPoolSize);
            _isReusableObject = prefab.GetComponent<IReusable>() != null;
        }
        private GameObject OnCreateObject() {
            GameObject obj = Instantiate(prefab);
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
            if (_isReusableObject) {
                LoadReusableScript(getObj)?.Get();
            }
        }

        private void OnRelease(GameObject releaseObj) {
            if (_isReusableObject) {
                LoadReusableScript(releaseObj)?.Release();
            }
            releaseObj.SetActive(false);
        }

        private void OnDestroyObj(GameObject destroyObj) {
            _objects.Remove(destroyObj);
            _reusableDictionary.Remove(destroyObj.GetInstanceID());
            Destroy(destroyObj);
        }

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
        /// 取出对象
        /// </summary>
        /// <returns>取出的对象</returns>
        public GameObject Get() {
            return _pool.Get();
        }
        /// <summary>
        /// 放回对象
        /// </summary>
        /// <param name="obj"></param>
        public void Release(GameObject obj) {
            _pool.Release(obj);
        }
        /// <summary>
        /// 放回全部对象
        /// </summary>
        public void ReleaseAll() {
            foreach (var obj in _objects) {
                _pool.Release(obj);
            }
        }
        /// <summary>
        /// 清除对象池
        /// </summary>
        public void Clear() {
            _pool.Clear();
        }
    }

}

