using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


class Form_Room : UIBase
{
    //开始游戏
    private Button startButton;
    //列表容器
    private Transform content1;//蓝方
    private Transform content2;//红方                   
    private GameObject playerObj;//玩家信息物体
    //返回键
    private Button returnBtn;
    public Form_Room() : base(UIPanel.Form_Room, UIType.Layer.Normal) { }
    public override void OnPimitive(params object[] para) { }
    protected override void OnActive()
    {
        //寻找组件
        startButton = UIGameObject.transform.Find("Root/ListPanel/StartButton").GetComponent<Button>();
        returnBtn = UIGameObject.transform.Find("Root/ReturnBtn").GetComponent<Button>();
        content1 = UIGameObject.transform.Find("Root/ListPanel/Scroll View1/Viewport/Content");
        content2 = UIGameObject.transform.Find("Root/ListPanel/Scroll View2/Viewport/Content");
        playerObj = UIGameObject.transform.Find("Root/Player").gameObject;
       
        //不激活玩家信息
        playerObj.SetActive(false);
        //按钮事件
        startButton.onClick.AddListener(OnStartClick);
        returnBtn.onClick.AddListener(OnReturnClick);
        //协议监听
        EventCenter.Instance.AddListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
        EventCenter.Instance.AddListener("MsgLeaveRoom", OnMsgLeaveRoom);
        EventCenter.Instance.AddListener("MsgStartGame", OnMsgStartGame);
        //发送查询
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        NetManager.Send(msg);
    }

    protected override void OnDeActive()
    {
        ////协议监听
        EventCenter.Instance.RemoveListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
        EventCenter.Instance.RemoveListener("MsgLeaveRoom", OnMsgLeaveRoom);
        EventCenter.Instance.RemoveListener("MsgStartGame", OnMsgStartGame);
    }

    //收到玩家列表协议
    public void OnMsgGetRoomInfo(object sender, EventArgs args)
    {
        MsgGetRoomInfo msg = (MsgGetRoomInfo)((MsgEventArgs)args).msgBase;
        //清除玩家列表
        for (int i = content1.childCount - 1; i >= 0; i--)
        {
            GameObject o = content1.GetChild(i).gameObject;
            UnityEngine.Object.Destroy(o);
        }
        for (int i = content2.childCount - 1; i >= 0; i--)
        {
            GameObject o = content2.GetChild(i).gameObject;
            UnityEngine.Object.Destroy(o);
        }
        //重新生成列表
        if (msg.players == null)
        {
            return;
        }
        for (int i = 0; i < msg.players.Length; i++)
        {
            GeneratePlayerInfo(msg.players[i]);
        }
    }

    //创建一个玩家信息单元
    public void GeneratePlayerInfo(PlayerInfo playerInfo)
    {
        //创建物体
        GameObject o = UnityEngine.Object.Instantiate(playerObj);
        o.SetActive(true);
        o.transform.localScale = Vector3.one;
        //设置阵营 1-蓝 2-红
        if (playerInfo.camp == 1)
        {
            o.transform.SetParent(content1);
        }
        else
        {
            o.transform.SetParent(content2);
        }
        //获取组件
        Transform trans = o.transform;
        Text idText = trans.Find("IdText").GetComponent<Text>();
        Text OwnerText = trans.Find("OwnerText").GetComponent<Text>();
        //填充信息
        idText.text = playerInfo.id;
        if (playerInfo.isOwner == 1)
        {
            OwnerText.gameObject.SetActive(true);
        }
        else
        {
            OwnerText.gameObject.SetActive(false);
        }
    }

    //点击返回按钮
    public void OnReturnClick()
    {
        MsgLeaveRoom msg = new MsgLeaveRoom();
        NetManager.Send(msg);
    }

    //收到退出房间协议
    public void OnMsgLeaveRoom(object sender, EventArgs args)
    {
        MsgLeaveRoom msg = (MsgLeaveRoom)((MsgEventArgs)args).msgBase;
        //成功退出房间
        if (msg.result == 0)
        {
            UIManager.Instance.ActiveUI<Form_RoomList>(UIPanel.Form_RoomList);
            UIManager.Instance.DeActiveUI<Form_Room>(UIPanel.Form_Room);
        }
        //退出房间失败
        else
        {
            UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "退出房间失败");
        }
    }

    //点击开战按钮
    public void OnStartClick()
    {
        MsgStartGame msg = new MsgStartGame();
        NetManager.Send(msg);
    }

    //收到开战返回
    public void OnMsgStartGame(object sender, EventArgs args)
    {
        MsgStartGame msg = (MsgStartGame)((MsgEventArgs)args).msgBase;
        //开战
        if (msg.result == 0)
        {
            UIManager.Instance.DeActiveUI<Form_Room>(UIPanel.Form_Room);
        }
        //开战失败
        else
        {
            UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "开战失败！两队至少都需要一名玩家，只有队长可以开始战斗！");
        }
    }
}
