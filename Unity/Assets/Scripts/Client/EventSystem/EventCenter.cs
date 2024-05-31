/****************************************************
	�ļ���EventCenter.cs
	���ߣ��﴾
	����: 1832259562@qq.com
	���ڣ�2024/05/04 19:59   	
	���ܣ��¼�����
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter : Singleton<EventCenter>
{
    private Dictionary<string, EventHandler> dic = new Dictionary<string, EventHandler>();

    /// <summary>
    /// �¼��Ѿ���ӽ��ֵ�
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    public bool EventIsAdded(string eventName)
    {
        return dic.ContainsKey(eventName);
    }
    //��Ӽ���
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
            Debug.LogError(eventName + "���¼�δע����ֵ�");
        }
    }
}
