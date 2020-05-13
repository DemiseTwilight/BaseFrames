using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * 主对象池
 * */
public class ObjectPool : SingletonBase<ObjectPool> {
    //子池字典：
    private Dictionary<string, SubPool> Pools = new Dictionary<string, SubPool>();
    public void SetCashePos(string objName,Vector3 position)
    {
        if (Pools.ContainsKey(objName) == false)
        {//主池不包含，则加入主池
            RegisterSubPool(objName);
        }
        SubPool subPool = Pools[objName];
        subPool.SetCashePos(position);
    }
    /**
     * 生成对象并
     *添加子池
    */
    public GameObject Spawn(string objName)
    {
        if (Pools.ContainsKey(objName) == false)
        {//主池不包含，则加入主池
            RegisterSubPool(objName);
        }
        SubPool subPool = Pools[objName];
        return subPool.Spawn();
    }
    /**
     * 注册主池字典
     * */
    private void RegisterSubPool(string objName)
    {
        GameObject obj = Resources.Load<GameObject>(objName);   //加载对象资源
        SubPool subPool = new SubPool(obj);                     //创建子池
        Pools.Add(objName, subPool);                            //加入主池中
    }
    /**
     * 生成对象并
     *添加子池
    */
    public GameObject Spawn(GameObject obj)
    {
        if (Pools.ContainsKey(obj.name) == false)
        {//主池不包含，则加入主池
            RegisterSubPool(obj);
        }
        SubPool subPool = Pools[obj.name];
        return subPool.Spawn();
    }
    /**
     * 注册主池字典
     * */
    private void RegisterSubPool(GameObject obj)
    {
        SubPool subPool = new SubPool(obj);                     //创建子池
        Pools.Add(obj.name, subPool);                            //加入主池中
    }

    /**
     * 放回主池中,如果对象不在对象池中则返回false;
     * */
    public bool UnSpawn(GameObject obj)
    {
        foreach (SubPool subPool in Pools.Values)
        {//属于子池，就放回子池：
            if (subPool.ContainObjectInSubPool(obj))
            {
                subPool.UnSpawn(obj);
                return true;
            }
        }
        return false;
    }

   /**
    * 全部放回
    * */
    public void UnSpawnAllObject()
    {
        foreach (SubPool subPool in Pools.Values)
        {
            subPool.UnSpawnAllObject();
        }
    }
    /// <summary>
    /// 清理，当场景切换时请调用
    /// </summary>
    public void Clear()
    {
        foreach (SubPool subPool in Pools.Values)
        {
            subPool.Clear();
        }
        Pools.Clear();
    }
}
