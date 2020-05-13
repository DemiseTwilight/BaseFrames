using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动挂载的单例基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonBase<T> : MonoBehaviour where T :SingletonBase<T>{

    private static T instance;
	
	public static T Instance
    {
        get
        {
            if (instance == null)
            {//生成一个gameObjective并挂载脚本
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);//防止场景销毁时 销毁
            }

            return instance;
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }

    protected virtual void Awake()
    {
        instance = this as T;
        
    }
}
