using Lockstep.Collision2D;
using Lockstep.Math;
using System;
using System.Collections.Generic;
using UnityEngine;

class BaseEntity : ILPTriggerEventHandler
{
    public int camp;
    public Entity entity;//实体
    public GameObject gameObject;//游戏对象
    public int gameObjectEntityID;
    public CTransform2D transform = new CTransform2D();
    public ColliderData colliderData = new ColliderData();

    public int maxHealth;//血量
    public int curHealth;//当前血量
    public int damage;
    private int skillID = 0;
    public int SkillID
    {
        get { return skillID; }
        set
        {
            skillID = value;
        }
    }
    public EntityView entityView;
    public bool isDead => curHealth <= 0;
    protected virtual void Init() { }
    public virtual void TakeDamage(BaseEntity atker, int amount, LVector3 hitPoint) { }
    protected virtual void OnDead() { }

    public virtual void OnLPTriggerEnter(ColliderProxy other) { }
    public virtual void OnLPTriggerExit(ColliderProxy other) { }
    public virtual void OnLPTriggerStay(ColliderProxy other) { }
}

