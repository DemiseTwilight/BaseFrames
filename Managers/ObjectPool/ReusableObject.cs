using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool {

    /// <summary>
    /// 可重用对象
    ///可通过继承该对象来实现可重用对象的初始化及析构化
    /// </summary>
    public abstract class ReusableObject : MonoBehaviour, IReusable {
        /// <summary>
        /// 该可重用对象存在的控制对象池
        /// </summary>
        internal ObjectPoolComponent belong;

        ObjectPoolComponent IReusable.Belong {
            get => belong;
            set => belong = value;
        }

        public abstract void Get();

        public abstract void Release();
        
    }
}
