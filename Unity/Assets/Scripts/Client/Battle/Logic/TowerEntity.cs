using Lockstep.Math;
using System;
using System.Collections.Generic;
using UnityEngine;

class TowerEntity:BaseEntity
{
    public void ResetPos(GameObject gO)
    {
        //转换定点数
        LVector3 pos = gO.transform.position.ToLVector3();
        transform.y = pos.y;
        transform.pos = new LVector2(pos.x, pos.z);
        gO.transform.localPosition = pos.ToVector3();

        transform.deg = (LFloat)gO.transform.rotation.y;
        gO.transform.localRotation = Quaternion.Euler(gO.transform.rotation.x, transform.deg, gO.transform.rotation.z);
        Init();
    }

    protected override void Init()
    {
        //设置血量
        maxHealth = 200;
        curHealth = maxHealth;
        damage = 40;
    }
    public override void TakeDamage(BaseEntity atker, int amount, LVector3 hitPoint)
    {
        if (isDead) return;
        if (camp == atker.camp) return;
        curHealth -= amount;
        entityView?.OnTakeDamage(amount, hitPoint);
        if (isDead)
        {
            OnDead();
        }
    }

    protected override void OnDead()
    {
        entityView?.OnDead();
        PhysicSystem.Instance.RemoveCollider(this);
        //在死亡会发送网络消息
        MsgBattleResult msg = new MsgBattleResult();
        if (this.camp == 1) msg.winCamp = 2;
        else msg.winCamp = 1;
        NetManager.Send(msg);
    }
}

