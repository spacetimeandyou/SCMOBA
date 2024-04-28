using System;

class Form_Loading : UIBase
{
    public Form_Loading() : base(UIPanel.Form_Loading, Config.UIPrefabPath, UIType.Layer.Normal)
    {

    }

    public override void Init()
    {
        base.Init();
    }

    protected override void OnActive()
    {
        throw new NotImplementedException();
    }

    protected override void OnDeActive()
    {
        throw new NotImplementedException();
    }
}

