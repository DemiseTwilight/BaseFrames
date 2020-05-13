using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 协程管理 :用于非继承MonoBehaviour对象委托协程
/// </summary>
public class CoroutineManager : SingletonBase<CoroutineManager>
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    ///延时
    public void Invoke(Action method, float waitSeconds)
    {
        if (waitSeconds < 0 || method == null)
        {
            return;
        }
        StartCoroutine(RunLaterCoroutine(method,waitSeconds));
    }

    IEnumerator RunLaterCoroutine(Action method, float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        method();
    }
    /// <summary>
    /// 重复
    /// </summary>
    /// <param name="method"></param>
    /// <param name="number">次数</param>
    /// <param name="periodically">周期</param>
    public void Repeat(Action method,int number, float period)
    {
        if (number < 0 || method == null)
        {
            return;
        }
        StartCoroutine(RunRepeatCoroutine(method,number, period));
    }

    IEnumerator RunRepeatCoroutine(Action method,int number, float period)
    {
        for(int i = 0; i < number; i++)
        {
            method();
            yield return new WaitForSeconds(period);
        }
        
    }
}
