using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


class Form_BattleResult : UIBase
{
    //当前图片名
    private string imageName = "victory";
    //胜利图片
    private string victoryName = "victory";
    private Sprite victory;
    //失败图片
    private string failName = "fail";
    private Sprite fail;
    private Image imageBG;
    private Button ReturnBtn;
    public Form_BattleResult() : base(UIPanel.Form_BattleResult, UIType.Layer.Normal) { }
    //传入胜利阵营，判断胜利或者失败
    public override void OnPimitive(params object[] args)
    {
        int victoryCamp = (int)args[0];
        if (GameRoot.camp == victoryCamp)
        {
            imageName = victoryName;
        }
        else imageName = failName;
        if (victory == null) victory= Resources.Load<Sprite>("UI/"+ victoryName);
        if (fail == null) fail= Resources.Load<Sprite>("UI/" + failName);   
    }
    protected override void OnActive()
    {
        //寻找组件
        imageBG = UIGameObject.transform.Find("BG").GetComponent<Image>();
        ReturnBtn = UIGameObject.transform.Find("Root/ReturnBtn").GetComponent<Button>();
        //监听
        ReturnBtn.onClick.AddListener(OnOkClick);
        if (imageName == victoryName)
        {
            imageBG.sprite = victory;
        }
        else imageBG.sprite = fail;
    }
    protected override void OnDeActive()
    {
        ReturnBtn.onClick.RemoveListener(OnOkClick);
    }

    //当按下确定按钮
    public void OnOkClick()
    {
        ResourcesManager.Instance.AsyncLoadScene(SceneName.GameStart, () =>
        {
            UIManager.Instance.ActiveUI<Form_RoomList>(UIPanel.Form_RoomList);
            UIManager.Instance.DeActiveUI<Form_BattleResult>(UIPanel.Form_BattleResult);
        });
    }
}