/****************************************************
	文件：EventCenter.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/05/04 19:59   	
	功能：事件中心
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter : Singleton<EventCenter>
{
    private Dictionary<string, EventHandler> dic = new Dictionary<string, EventHandler>();

    /// <summary>
    /// 事件已经添加进字典
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    public bool EventIsAdded(string eventName)
    {
        return dic.ContainsKey(eventName);
    }
    //添加监听
    public void AddListener(string eventName, EventHandler eventHandler)
    {
        if (dic.ContainsKey(eventName))
        {
            dic[eventName] += eventHandler;
        }
        else
        {
            dic.Add(eventName, eventHandler);
        }
    }

    public void RemoveListener(string eventName, EventHandler eventHandler)
    {
        if (dic.ContainsKey(eventName))
        {
            dic[eventName] -= eventHandler;
        }
    }

    public void TriggerEvent(string eventName, object sender)
    {
        if (dic.ContainsKey(eventName))
        {
            dic[eventName]?.Invoke(sender, EventArgs.Empty);
        }

    }
    
    public void TriggerEvent(string eventName, object sender, EventArgs args)
    {
        if (dic.ContainsKey(eventName))
        {
            if (args is null)
            {
                dic[eventName]?.Invoke(sender, EventArgs.Empty);
            }
            else
            {
                dic[eventName]?.Invoke(sender, args);
            }
        }
        else
        {
            Debug.LogError(eventName + "：事件未注册进字典");
        }
    }
}
