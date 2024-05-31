using Lockstep.Math;
using System;
using System.Collections.Generic;
using UnityEngine;

class EntityView:MonoBehaviour, IEntityView
{
    public UIFloatBar uiFloatBar;
    public BaseEntity entity;
    protected bool isDead => entity?.isDead ?? true;

    public void BindEntity(BaseEntity entity)
    {
        this.entity = entity;
        uiFloatBar = FloatBarManager.CreateFloatBar(transform, this.entity.curHealth, this.entity.maxHealth);
    }

    public void OnTakeDamage(int amount, LVector3 hitPoint)
    {
        uiFloatBar.UpdateHp(entity.curHealth, entity.maxHealth);
        FloatTextManager.CreateFloatText(hitPoint.ToVector3(), -amount);
    }

    public void OnDead()
    {
        if (uiFloatBar != null) FloatBarManager.DestroyText(uiFloatBar);
        GameObject.Destroy(gameObject);
    }

    public void OnRollbackDestroy()
    {
        if (uiFloatBar != null) FloatBarManager.DestroyText(uiFloatBar);
        GameObject.Destroy(gameObject);
    }

    private void Update()
    {
        var pos = entity.transform.Pos3.ToVector3();
        transform.position = Vector3.Lerp(transform.position, pos, 0.3f);
        var deg = entity.transform.deg.ToFloat();
        if (entity.camp == 1) deg += 90;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, deg, 0), 0.2f);
    }
}

