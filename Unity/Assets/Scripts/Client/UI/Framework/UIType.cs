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
/// <summary>
/// UI界面名
/// </summary>
public class UIPanel
{
    public static string Form_Loading = "Form_Loading";
    public static string Form_Login = "Form_Login";
    public static string Form_Tips = "Form_Tips";
    public static string Form_Register = "Form_Register"; 
    public static string Form_GameMain = "Form_GameMain";
    public static string Form_RoomList = "Form_RoomList";
    public static string Form_Room = "Form_Room";
    public static string Form_Battle = "Form_Battle"; 
    public static string Form_BattleResult = "Form_BattleResult";

}

