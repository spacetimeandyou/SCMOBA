using System;
using UnityEngine;
using UnityEngine.UI;

class Form_GameMain : UIBase
{
    //进入房间按钮
    private Button enterRoomBtn;
    private Text name;
    public Form_GameMain() : base(UIPanel.Form_GameMain, UIType.Layer.Normal) { }
    public override void OnPimitive(params object[] para){}
    protected override void OnActive()
    {
        enterRoomBtn = UIGameObject.transform.Find("Root/EnterRoomBtn").GetComponent<Button>();
        name = UIGameObject.transform.Find("Root/Name").GetComponent<Text>();

        enterRoomBtn.onClick.AddListener(OnEnterRoomBtnClick);
        name.text = GameRoot.id;
    }
    private void OnEnterRoomBtnClick()
    {
        UIManager.Instance.ActiveUI<Form_RoomList>(UIPanel.Form_RoomList);
        UIManager.Instance.DeActiveUI<Form_GameMain>(UIPanel.Form_GameMain);
    }
    protected override void OnDeActive(){}
}

