using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;
/// <summary>
/// 使用对象池的一个例子
/// </summary>
public class BulletReusable : ReusableObject
{
    public override void Get()
    {
        print("生成Bullet");
    }

    public override void Release()
    {
        print("销毁Bullet");
    }
    
}
