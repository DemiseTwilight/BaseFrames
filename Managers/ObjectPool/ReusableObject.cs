using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * 可重用对象
 * 可通过继承该对象来实现可重用对象的初始化及析构化
 * */
public abstract class ReusableObject : MonoBehaviour, IReusable

{

    public abstract void Spawn();

    public abstract void UnSpawn();

}
