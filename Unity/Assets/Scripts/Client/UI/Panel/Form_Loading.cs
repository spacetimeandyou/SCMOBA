using System;
using UnityEngine;
using UnityEngine.UI;

class Form_Loading : UIBase
{
    private Text textTips;
    private Image loadingFg;
    private Image imgPoint;
    private Text txtPrg;

    private float fgWidth;
    #region 生命周期
    public Form_Loading() : base(UIPanel.Form_Loading, UIType.Layer.Top){}
    
    public override void OnPimitive(params object[] para){}
    protected override void OnActive()
    {
        textTips = UIGameObject.transform.Find("Root/txtTips").GetComponent<Text>();
        loadingFg = UIGameObject.transform.Find("Root/loadingFg").GetComponent<Image>();
        imgPoint = UIGameObject.transform.Find("Root/loadingFg/imgPoint").GetComponent<Image>();
        txtPrg = UIGameObject.transform.Find("Root/txtPrg").GetComponent<Text>();
        RefreshUI();
    }
    protected override void OnDeActive(){}
    #endregion

    private void RefreshUI()
    {
        fgWidth = loadingFg.GetComponent<RectTransform>().sizeDelta.x;

        textTips.text = "Tips:带有霸体状态的技能在施放时可以规避控制";
        txtPrg.text = "0%";
        imgPoint.transform.localPosition = new Vector3(-395f, 0, 0);
        loadingFg.fillAmount = 0;
    }

    public void SetProgress(float prg)
    {
        loadingFg.fillAmount = prg;
        txtPrg.text = (int)(prg * 100) + "%";
        float posX = prg * fgWidth - 395;
        imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
    }
}

