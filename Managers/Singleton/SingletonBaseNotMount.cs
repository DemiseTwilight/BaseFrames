using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//单例基类：
public class SingletonBaseNotMount<T> where T : new(){

    private static T instance;
	
	public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
}
