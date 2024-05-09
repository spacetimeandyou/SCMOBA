using System;
using UnityEngine;
using UnityEngine.UI;


class Form_RoomList : UIBase
{
    //账号文本
    private Text idText;
    //创建房间按钮
    private Button createButton;
    //刷新列表按钮
    private Button reflashButton;
    //列表容器
    private Transform content;
    //房间物体
    private GameObject roomObj;
    //返回键
    private Button returnBtn;

    public Form_RoomList() : base(UIPanel.Form_RoomList, UIType.Layer.Normal) { }
    public override void OnPimitive(params object[] para) { }
    protected override void OnActive()
    {
        //寻找组件
        idText = UIGameObject.transform.Find("Root/InfoPanel/Name").GetComponent<Text>();
        createButton = UIGameObject.transform.Find("Root/ListPanel/CtrlPanel/CreateButton").GetComponent<Button>();
        reflashButton = UIGameObject.transform.Find("Root/ListPanel/CtrlPanel/ReflashButton").GetComponent<Button>();
        content = UIGameObject.transform.Find("Root/ListPanel/Scroll View/Viewport/Content");
        roomObj = UIGameObject.transform.Find("Root/Room").gameObject;
        returnBtn = UIGameObject.transform.Find("Root/ReturnBtn").GetComponent<Button>();
        //不激活房间
        roomObj.SetActive(false);
        //显示id
        idText.text = GameRoot.id;
        //按钮事件
        createButton.onClick.AddListener(OnCreateClick);
        reflashButton.onClick.AddListener(OnReflashClick);
        returnBtn.onClick.AddListener(OnReturnClick);
        //协议监听
        EventCenter.Instance.AddListener("MsgGetRoomList", OnMsgGetRoomList);
        EventCenter.Instance.AddListener("MsgCreateRoom", OnMsgCreateRoom);
        EventCenter.Instance.AddListener("MsgEnterRoom", OnMsgEnterRoom);
        //发送查询
        MsgGetRoomList msgGetRoomList = new MsgGetRoomList();
        NetManager.Send(msgGetRoomList);
    }
    protected override void OnDeActive()
    {
        //协议监听
        EventCenter.Instance.RemoveListener("MsgGetRoomList", OnMsgGetRoomList);
        EventCenter.Instance.RemoveListener("MsgCreateRoom", OnMsgCreateRoom);
        EventCenter.Instance.RemoveListener("MsgEnterRoom", OnMsgEnterRoom);
    }
    //收到房间列表协议
    public void OnMsgGetRoomList(object sender, EventArgs args)
    {
        MsgGetRoomList msg = (MsgGetRoomList)((MsgEventArgs)args).msgBase;
        //清除房间列表
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            UnityEngine.Object.Destroy(o);
        }
        //重新生成列表
        if (msg.rooms == null)
        {
            return;
        }
        for (int i = 0; i < msg.rooms.Length; i++)
        {
            GenerateRoom(msg.rooms[i]);
        }
    }

    //创建一个房间单元
    public void GenerateRoom(RoomInfo roomInfo)
    {
        //创建物体
        GameObject o = UnityEngine.Object.Instantiate(roomObj);
        o.transform.SetParent(content);
        o.SetActive(true);
        o.transform.localScale = Vector3.one;
        //获取组件
        Transform trans = o.transform;
        Text idText = trans.Find("IdText").GetComponent<Text>();
        Text countText = trans.Find("CountText").GetComponent<Text>();
        Text statusText = trans.Find("StatusText").GetComponent<Text>();
        Button btn = trans.Find("JoinButton").GetComponent<Button>();
        //填充信息
        idText.text = roomInfo.id.ToString();
        countText.text = roomInfo.count.ToString();
        if (roomInfo.status == 0)
        {
            statusText.text = "准备中";
        }
        else
        {
            statusText.text = "战斗中";
        }
        //按钮事件
        btn.name = idText.text;
        btn.onClick.AddListener(delegate ()
        {
            OnJoinClick(btn.name);
        });
    }
    //点击返回按钮
    public void OnReturnClick()
    {
        UIManager.Instance.ActiveUI<Form_GameMain>(UIPanel.Form_GameMain);
        UIManager.Instance.DeActiveUI<Form_RoomList>(UIPanel.Form_RoomList);
    }

    //点击刷新按钮
    public void OnReflashClick()
    {
        MsgGetRoomList msg = new MsgGetRoomList();
        NetManager.Send(msg);
    }

    //点击加入房间按钮
    public void OnJoinClick(string idString)
    {
        MsgEnterRoom msg = new MsgEnterRoom();
        msg.id = int.Parse(idString);
        NetManager.Send(msg);
    }

    //收到进入房间协议
    public void OnMsgEnterRoom(object sender, EventArgs args)
    {
        MsgEnterRoom msg = (MsgEnterRoom)((MsgEventArgs)args).msgBase;
        //成功进入房间
        if (msg.result == 0)
        {
            UIManager.Instance.ActiveUI<Form_Room>(UIPanel.Form_Room);
            UIManager.Instance.DeActiveUI<Form_RoomList>(UIPanel.Form_RoomList);
        }
        //进入房间失败
        else
        {
            UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "进入房间失败");
        }
    }

    //点击新建房间按钮
    public void OnCreateClick()
    {
        MsgCreateRoom msg = new MsgCreateRoom();
        NetManager.Send(msg);
    }

    //收到新建房间协议
    public void OnMsgCreateRoom(object sender, EventArgs args)
    {
        MsgCreateRoom msg = (MsgCreateRoom)((MsgEventArgs)args).msgBase;
        //成功创建房间
        if (msg.result == 0)
        {
            UIManager.Instance.ActiveUI<Form_Room>(UIPanel.Form_Room);
            UIManager.Instance.DeActiveUI<Form_RoomList>(UIPanel.Form_RoomList);
        }
        else
        {
            UIManager.Instance.ActiveUI<Form_Tips>(UIPanel.Form_Tips, "房间失败");
        }
    }
}

