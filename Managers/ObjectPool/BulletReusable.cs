using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReusable : ReusableObject
{
    public override void Spawn()
    {
        print("生成Bullet");
        Invoke("DelayDestroy", 3.0f);
    }

    public override void UnSpawn()
    {
        print("销毁Bullet");
    }

    private void DelayDestroy()
    {
        ObjectPool.Instance.UnSpawn(gameObject);
    }
}
