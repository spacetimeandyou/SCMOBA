/****************************************************
	文件：GenericsSingleton.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/04/24 20:57   	
	功能：泛型单例(双重锁)
*****************************************************/
using System;
using System.Collections.Generic;

public abstract class Singleton<T> where T : class, new()
{
    private static T _instance = null;
    private static readonly String Singleton_Lock = new("Singleton_Lock");

    public static T Instance
    {
        get
        {
             if (_instance == null)
            {
                lock (Singleton_Lock)
                {
                    if (_instance == null) _instance = new T();
                }
            }
            return _instance;
        }
    }
}

