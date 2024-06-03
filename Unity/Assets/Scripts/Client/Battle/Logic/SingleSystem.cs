/****************************************************
	文件：SingleSystem.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/05/26 17:34   	
	功能：单组件系统
*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 单组件系统执行单个组件，不需要挂载到实体上，可以直接在系统注册组件并执行逻辑
/// </summary>
class SingleSystem : BaseSystem
{
    //外部访问当前组件数据
    public static Dictionary<int, ISingleComponent> SingleComponents = new Dictionary<int, ISingleComponent>();
    #region 生命周期
    public override void DoAwake()
    {
        RegisterSingleComponent();
    }

    public override void DoUpdate()
    {
        //TODO:执行组件逻辑
        ExecuteComponent<InputComponent>((int)SingleComponentType.InputComponent);
    }

    public override void DoDestroy()
    {
        RemoveSingleComponent();
    }
    #endregion
    private void RegisterSingleComponent()
    {
        //TODO:此处添加单件
        SingleComponents[(int)SingleComponentType.InputComponent] = new InputComponent();
    }

    public void RemoveSingleComponent()
    {
        SingleComponents.Remove((int)SingleComponentType.InputComponent);
    }

    //处理单个组件逻辑
    public void ExecuteComponent<T>(int componentType)
    {
        if (SingleComponents.ContainsKey(componentType))
        {
            //执行特定前缀的方法
            string methodName = "OnExecute" + typeof(T).Name;

            // 获取MethodInfo对象
            MethodInfo methodInfo = typeof(SingleSystem).GetMethod(methodName);

            // 调用MethodInfo
            if (methodInfo != null)
            {
                methodInfo.Invoke(this, new object[] { componentType });
            }
        }
    }
    //输入组件执行
    public void OnExecuteInputComponent(int componentType)
    {
        if (!SingleComponents.TryGetValue(componentType, out ISingleComponent component)) return;
        InputComponent inputComponent = (InputComponent)component;
        inputComponent.dirX = (int)Input.GetAxisRaw("Horizontal");
        inputComponent.dirY = (int)Input.GetAxisRaw("Vertical");
        Debug.Log("dirX:" + inputComponent.dirX+ "dirY:" + inputComponent.dirY);
        //skill:普攻 1技能 2技能 3技能 额外技能槽1,2,3 先一个普攻
        inputComponent.skillID = 0;
        if (Input.GetMouseButtonDown(0))
        {
            inputComponent.skillID = 1;
        }    
    }
}

