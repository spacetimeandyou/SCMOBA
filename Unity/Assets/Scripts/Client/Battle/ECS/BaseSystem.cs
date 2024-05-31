/****************************************************
	文件：ISystem.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/05/09 18:40   	
	功能：系统基类
*****************************************************/
using Lockstep.Math;
using System;
using System.Collections.Generic;

//系统:筛选对应组件类型的实体
abstract class BaseSystem : BaseLifeCycle
{
    public bool enable = true;

    public int[] componentType;

    public BaseSystem(params object[] args)
    {
        if (args.Length == 0 || args[0].GetType() != typeof(ComponentType)) return;
        componentType = new int[args.Length];
        for (int i = 0; i < args.Length; i++)
        {
            this.componentType[i] = (int)args[i];
        }
    }
    public virtual void IterateComponent() { }
}
