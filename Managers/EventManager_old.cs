using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 全局事件管理：
/// </summary>
public class EventManager : SingletonBaseNotMount<EventManager>
{
    /// <summary>
    /// 委托管理容器
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="data">数据</param>
    public delegate void OnEventHandler(string eventName,object data);
    /// <summary>
    /// 事件管理中心
    /// </summary>
    private Dictionary<string, OnEventHandler> events=
        new Dictionary<string, OnEventHandler>();

    /// <summary>
    /// 添加监听(订阅)：
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="handler">句柄</param>
    public void AddListener(string eventName, OnEventHandler handler)
    {
        if (this.events.ContainsKey(eventName))
        {
            this.events[eventName] += handler;
        }
        else
        {
            this.events.Add(eventName, handler);
        }
    }
    /// <summary>
    /// 取消监听（订阅）
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="handler">句柄</param>
    public void RemoveListener(string eventName, OnEventHandler handler)
    {
        if (this.events.ContainsKey(eventName))
        {
            this.events[eventName] -= handler;
            if (this.events[eventName]==null)
            {
                this.events.Remove(eventName);
            }
        }
    }
    /// <summary>
    /// 发布消息
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="data">数据</param>
    public void DispatchEvent(string eventName,object data)
    {   //无人监听：
        if (!this.events.ContainsKey(eventName))
        {
            return;
        }

        this.events[eventName](eventName,data);
    }
}
