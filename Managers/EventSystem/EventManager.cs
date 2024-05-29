using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 全局事件管理：
/// 直接调用不安全，请使用EventDesc
/// </summary>
public class EventManager : SingletonBaseNotMount<EventManager>
{
    /// <summary>
    /// 委托管理容器
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="data">数据</param>
    public delegate void OnEventHandler();
    public delegate void OnEventHandler<T1>(T1 arg);
    public delegate void OnEventHandler<T1,T2>(T1 arg1,T2 arg2);
    public delegate void OnEventHandler<T1,T2,T3>(T1 arg1,T2 arg2,T3 arg3);
    public delegate void OnEventHandler<T1,T2,T3,T4>(T1 arg1,T2 arg2,T3 arg3,T4 arg4);
    public delegate void OnEventHandler<T1,T2,T3,T4,T5>(T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5);
    public delegate void OnEventHandler<T1,T2,T3,T4,T5,T6>(T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6);
    /// <summary>
    /// 事件管理中心
    /// </summary>
    private Dictionary<EventDescBase, Delegate> events=
        new Dictionary<EventDescBase, Delegate>();
    
    
    /// <summary>
    /// 添加监听(订阅)：
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="handler">句柄</param>
    public void AddListener(EventDescBase eventName, Delegate handler)
    {
        if (events.ContainsKey(eventName))
            events[eventName] = Delegate.Combine(events[eventName],handler);
        else
            events.TryAdd(eventName, handler);
    }
    /// <summary>
    /// 取消监听（订阅）
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="handler">句柄</param>
    public void RemoveListener(EventDescBase eventName, Delegate handler)
    {
        if (events.ContainsKey(eventName)) {
            events[eventName]=Delegate.Remove(events[eventName], handler);
            if (events[eventName]==null)
            {
                events.Remove(eventName);
            }
        }
    }
    /// <summary>
    /// 发布消息
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="data">数据</param>
    public void DispatchEvent(EventDescBase eventName)
    {   
        if (events.TryGetValue(eventName,out var eventDesc))
        {
            var handler = eventDesc as OnEventHandler;
            handler?.Invoke();
        }
    }
    
    //泛型
    public void DispatchEvent<T>(EventDesc<T> eventName,T arg1)
    {   
        if (events.TryGetValue(eventName,out var eventDesc))
        {
            var handler = eventDesc as OnEventHandler<T>;
            handler?.Invoke(arg1);
        }
    }
    public void DispatchEvent<T1,T2>(EventDesc<T1,T2> eventName,T1 arg1,T2 arg2)
    {   
        if (events.TryGetValue(eventName,out var eventDesc))
        {
            var handler = eventDesc as OnEventHandler<T1,T2>;
            handler?.Invoke(arg1,arg2);
        }
    }
    public void DispatchEvent<T1,T2,T3>(EventDesc<T1,T2,T3> eventName,T1 arg1,T2 arg2,T3 arg3)
    {   
        if (events.TryGetValue(eventName,out var eventDesc))
        {
            var handler = eventDesc as OnEventHandler<T1,T2,T3>;
            handler?.Invoke(arg1,arg2,arg3);
        }
    }
    public void DispatchEvent<T1,T2,T3,T4>(EventDesc<T1,T2,T3,T4> eventName,T1 arg1,T2 arg2,T3 arg3,T4 arg4)
    {   
        if (events.TryGetValue(eventName,out var eventDesc))
        {
            var handler = eventDesc as OnEventHandler<T1,T2,T3,T4>;
            handler?.Invoke(arg1,arg2,arg3,arg4);
        }
    }
    public void DispatchEvent<T1,T2,T3,T4,T5>(EventDesc<T1,T2,T3,T4,T5> eventName,T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5)
    {   
        if (events.TryGetValue(eventName,out var eventDesc))
        {
            var handler = eventDesc as OnEventHandler<T1,T2,T3,T4,T5>;
            handler?.Invoke(arg1,arg2,arg3,arg4,arg5);
        }
    }
    public void DispatchEvent<T1,T2,T3,T4,T5,T6>(EventDesc<T1,T2,T3,T4,T5,T6> eventName,T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6)
    {   
        if (events.TryGetValue(eventName,out var eventDesc))
        {
            var handler = eventDesc as OnEventHandler<T1,T2,T3,T4,T5,T6>;
            handler?.Invoke(arg1,arg2,arg3,arg4,arg5,arg6);
        }
    }
    public void DispatchEvent<T>(EventDesc<T> eventName,params T[] args)
    {   
        if (events.TryGetValue(eventName,out var eventDesc))
        {
            var handler = eventDesc as OnEventHandler<T[]>;
            handler?.Invoke(args);
        }
    }
}
