using System;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance = null;
    public static string id = "";
    public static int locatId;
    public static int camp;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Init();
    }

    private void Init()
    {
        //网络监听
        EventCenter.Instance.AddListener(EventName.ConnectClose, OnConnectClose);
        EventCenter.Instance.AddListener("MsgKick", OnMsgKick);
        UIManager.Instance.Init();
        BattleManager.Instance.Init();
        UIManager.Instance.ActiveUI<Form_Login>(UIPanel.Form_Login);
    }

    private void Update()
    {
        NetManager.Update();
        UIManager.Instance.Update(Time.deltaTime);
        ResourcesManager.Instance.Update();
        BattleManager.Instance.Update();
    }
    
    void OnConnectClose(object sender, EventArgs args)
    {
        Debug.Log("断开连接");
    }

    //被踢下线
    void OnMsgKick(object sender, EventArgs args)
    {
        UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "被踢下线");
    }
}
