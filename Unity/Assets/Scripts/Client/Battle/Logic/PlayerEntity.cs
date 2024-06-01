using System;
using System.Collections;
using UnityEngine;
using Lockstep.Math;
using Lockstep.Collision2D;

class PlayerEntity : BaseEntity
{
    private LFloat deltaTime = (LFloat)0.05;
    
    public string playerId;//玩家id，确定消息执行对象

    public LFloat speed = 3; //移动速度
    private LVector2 dir;
    public LVector2 Dir
    {
        get { return dir;} 
        set {
            if (value == LVector2.zero) return;
            dir = value.normalized;
            transform.pos = transform.pos + dir * speed * deltaTime;
            var targetDeg = dir.ToDeg();
            transform.deg = CTransform2D.TurnToward(targetDeg, transform.deg, 360 * deltaTime, out var hasReachDeg);
        }
    }//移动方向

    public int skillID = 0;
    public int skillTime = 3;
    #region 出生点位置和旋转
    public LVector3 brithPosition;
    public LVector3 brithRotation;
    public void SetBirthPos(int positionId)
    {
        int index = positionId / 2;
        brithPosition.x = (LFloat)BattleConfig.birthConfig[camp - 1, index, 0];
        brithPosition.y = (LFloat)BattleConfig.birthConfig[camp - 1, index, 1];
        brithPosition.z = (LFloat)BattleConfig.birthConfig[camp - 1, index, 2];
        brithRotation.x = (LFloat)BattleConfig.birthConfig[camp - 1, index, 3];
        brithRotation.y = (LFloat)BattleConfig.birthConfig[camp - 1, index, 4];
        brithRotation.z = (LFloat)BattleConfig.birthConfig[camp - 1, index, 5];
        ResetPos();
        Init();
    }
    public void ResetPos()
    {
        transform.y = brithPosition.y;
        transform.pos = new LVector2(brithPosition.x, brithPosition.z);
        transform.deg = brithRotation.y;
        gameObject.transform.localPosition = transform.Pos3.ToVector3();
        gameObject.transform.localRotation = Quaternion.Euler(brithRotation.x, transform.deg, brithRotation.z);
    }
    #endregion
    
    public int damage = BattleConfig.skillConfig[0];//普攻
    protected override void Init()
    {
        //设置血量
        maxHealth = 100;
        curHealth = maxHealth;
    }

    public virtual void TakeDamage(BaseEntity atker, int amount, LVector3 hitPoint)
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

    protected virtual void OnDead()
    {
        entityView?.OnDead();
        PhysicSystem.Instance.RemoveCollider(this);
    }
    public override void OnLPTriggerEnter(ColliderProxy other) { Debug.LogError("执行碰撞进入"); }
    public override void OnLPTriggerExit(ColliderProxy other) { Debug.LogError("执行碰撞退出"); }
    public override void OnLPTriggerStay(ColliderProxy other) { Debug.LogError("执行碰撞停留"); }
}

