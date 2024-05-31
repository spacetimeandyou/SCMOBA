using System;
using UnityEngine;
using UnityEngine.UI;

class Form_Login : UIBase
{
    //账号输入框
    private InputField idInput;
    //密码输入框
    private InputField pwInput;
    //登陆按钮
    private Button loginBtn;
    //注册按钮
    private Button regBtn;
    //Socket
    private string ip = "127.0.0.1";
    private int port = 8888;

    #region 生命周期
    public Form_Login() : base(UIPanel.Form_Login, UIType.Layer.Normal){}
    
    public override void OnPimitive(params object[] para){}
    protected override void OnActive()
    {
        //寻找组件
        idInput = UIGameObject.transform.Find("Root/IdInput").GetComponent<InputField>();
        pwInput = UIGameObject.transform.Find("Root/PwInput").GetComponent<InputField>();
        loginBtn = UIGameObject.transform.Find("Root/LoginBtn").GetComponent<Button>();
        regBtn = UIGameObject.transform.Find("Root/RegisterBtn").GetComponent<Button>();
        //监听
        loginBtn.onClick.AddListener(OnLoginClick);
        regBtn.onClick.AddListener(OnRegClick);
        //网络协议监听
        EventCenter.Instance.AddListener("MsgLogin", OnMsgLogin);
        //网络事件监听
        EventCenter.Instance.AddListener(EventName.ConnectSucc, OnConnectSucc);
        EventCenter.Instance.AddListener(EventName.ConnectFail, OnConnectFail);
        //连接服务器
        NetManager.Connect(ip, port);
    }

    protected override void OnDeActive()
    {
        //网络协议监听
        EventCenter.Instance.RemoveListener("MsgLogin", OnMsgLogin);
        //网络事件监听
        EventCenter.Instance.RemoveListener(EventName.ConnectSucc, OnConnectSucc);
        EventCenter.Instance.RemoveListener(EventName.ConnectFail, OnConnectFail);
    }
    #endregion

    //连接成功回调
    void OnConnectSucc(object sender, EventArgs args)
    {
        Debug.Log("OnConnectSucc");
    }

    //连接失败回调
    void OnConnectFail(object sender, EventArgs args)
    {
        UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips,"网络连接失败");
    }

    //当按下注册按钮
    public void OnRegClick()
    {
        UIManager.Instance.ActiveUI<Form_Register>(UIPanel.Form_Register);
    }

    //当按下登陆按钮
    public void OnLoginClick()
    {
        //用户名密码为空
        if (idInput.text == "" || pwInput.text == "")
        {
            UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "用户名和密码不能为空");
            return;
        }
        //发送
        MsgLogin msgLogin = new();
        msgLogin.id = idInput.text;
        msgLogin.pw = pwInput.text;
        NetManager.Send(msgLogin);
    }

    //收到登陆协议
    public void OnMsgLogin(object sender, EventArgs args)
    {

        MsgLogin msg = (MsgLogin)((MsgEventArgs)args).msgBase;
        if (msg.result == 0)
        {
            Debug.Log("登陆成功");
            //设置id
            GameRoot.id = msg.id;
            //打开主界面
            UIManager.Instance.ActiveUI<Form_GameMain>(UIPanel.Form_GameMain);
            //关闭界面
            UIManager.Instance.DeActiveUI<Form_Login>(UIPanel.Form_Login);
        }
        else
        {
            UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "登陆失败");
        }
    }
}

