using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPool {
    public interface IReusable {

        internal ObjectPoolComponent Belong { get; set; }
        void Get();

        void Release();

    }

}
