using System;
using System.Collections.Generic;
using UnityEngine.UI;

class Form_Tips : UIBase
{
    //提示文本
    private Text text;
    //确定按钮
    private Button okBtn;
    //传入字符串
    private string argsStr;

    public Form_Tips() : base(UIPanel.Form_Tips, UIType.Layer.Top)
    {

    }
    public override void OnPimitive(params object[] args)
    {
        if (args.Length == 1)
        {
            argsStr = (string)args[0];
        }
    }

    protected override void OnActive()
    {
        //寻找组件
        text = UIGameObject.transform.Find("Root/Text").GetComponent<Text>();
        okBtn = UIGameObject.transform.Find("Root/OkBtn").GetComponent<Button>();
        //监听
        okBtn.onClick.AddListener(OnOkClick);
        //提示语
        text.text = argsStr;
    }

    protected override void OnDeActive(){}

    //当按下确定按钮
    public void OnOkClick()
    {
        UIManager.Instance.DeActiveUI<Form_Tips>(UIPanel.Form_Tips);
    }
}

