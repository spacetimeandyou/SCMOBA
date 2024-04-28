/****************************************************
	文件：UIType.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/04/24 20:14   	
	功能：定义关于UI的类型
*****************************************************/
using System;

public class UIType
{
    /// <summary>
    /// UI层级类型
    /// </summary>
    public enum Layer
    {
        Top,
        Normal,
        Bottom,
    }

    /// <summary>
    /// 加载方式
    /// </summary>
    public enum Load
    {
        sync,//同步
        Async,//异步
    }
}

public class UIPanel
{
    public static string Form_Loading = "Form_Loading";
}

