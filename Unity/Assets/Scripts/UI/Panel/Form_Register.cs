using System;
using System.Collections.Generic;
using UnityEngine.UI;

class Form_Register : UIBase
{
    //账号输入框
    private InputField idInput;
    //密码输入框
    private InputField pwInput;
    //重复输入框
    private InputField repInput;
    //注册按钮
    private Button regBtn;
    //关闭按钮
    private Button closeBtn;

    #region 生命周期
    public Form_Register() : base(UIPanel.Form_Register, UIType.Layer.Normal){}
    public override void OnPimitive(params object[] para){ }
    protected override void OnActive()
    {
        //寻找组件
        idInput = UIGameObject.transform.Find("Root/IdInput").GetComponent<InputField>();
        pwInput = UIGameObject.transform.Find("Root/PwInput").GetComponent<InputField>();
        repInput = UIGameObject.transform.Find("Root/RepInput").GetComponent<InputField>();
        regBtn = UIGameObject.transform.Find("Root/RegisterBtn").GetComponent<Button>();
        closeBtn = UIGameObject.transform.Find("Root/CloseBtn").GetComponent<Button>();
        //监听
        regBtn.onClick.AddListener(OnRegClick);
        closeBtn.onClick.AddListener(OnCloseClick);
        //网络协议监听
        EventCenter.Instance.AddListener("MsgRegister", OnMsgRegister);
    }

    protected override void OnDeActive()
    {
        EventCenter.Instance.RemoveListener("MsgRegister", OnMsgRegister);
    }
    #endregion

    //当按下注册按钮
    public void OnRegClick()
    {
        //用户名密码为空
        if (idInput.text == "" || pwInput.text == "")
        {
            UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "用户名和密码不能为空");
            return;
        }
        //两次密码不同
        if (repInput.text != pwInput.text)
        {
            UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "两次输入的密码不同");
            return;
        }
        //发送
        MsgRegister msgReg = new MsgRegister();
        msgReg.id = idInput.text;
        msgReg.pw = pwInput.text;
        NetManager.Send(msgReg);
    }

    //收到注册协议
    public void OnMsgRegister(object sender, EventArgs args)
    {
        MsgRegister msg = (MsgRegister)((MsgEventArgs)args).msgBase;
        if (msg.result == 0)
        {
            UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "注册成功");
            UIManager.Instance.DeActiveUI<Form_Register>(UIPanel.Form_Register);
        }
        else
        {
            UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "注册失败");
        }
    }

    //当按下关闭按钮
    public void OnCloseClick()
    {
        UIManager.Instance.DeActiveUI<Form_Register>(UIPanel.Form_Register);
    }
}

