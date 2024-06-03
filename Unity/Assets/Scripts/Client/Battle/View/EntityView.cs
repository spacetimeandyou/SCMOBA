using Lockstep.Collision2D;
using Lockstep.Math;
using System;
using System.Collections.Generic;
using UnityEngine;

class EntityView : MonoBehaviour, IEntityView
{
    public UIFloatBar uiFloatBar;
    public BaseEntity entity;
    //攻击冷却
    List<BaseEntity> tempEnities = new List<BaseEntity>();
    public LFloat cDTime = 1;
    public LFloat curSkillTime = 0;
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
        if (transform.position != pos)
        {
            transform.position = Vector3.Lerp(transform.position, pos, 0.3f);
            var deg = entity.transform.deg.ToFloat();
            deg += 90;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, deg, 0), 0.2f);
        }
        if (curSkillTime > 0) curSkillTime -= (LFloat)Time.deltaTime;//技能冷却中
        else if (entity.SkillID > 0)
        {
            tempEnities.Clear();
            PlayerEntity[] entities = PlayerManager.Instance.GetAllPlayer();
            TowerEntity[] towers = PlayerManager.Instance.GetAllTower();
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].gameObjectEntityID == entity.gameObjectEntityID) continue;
                if ((entities[i].transform.pos - entity.transform.pos).sqrMagnitude < 25)
                {
                    tempEnities.Add(entities[i]);
                }
            }
            for (int i = 0; i < towers.Length; i++)
            {
                if ((towers[i].transform.pos - entity.transform.pos).sqrMagnitude < 25)
                {
                    tempEnities.Add(towers[i]);
                }
            }
            Attack();
            curSkillTime = cDTime;
        }
    }

    private void Attack()
    {
        for (int i = 0; i < tempEnities.Count; i++)
        {
            tempEnities[i].TakeDamage(entity, entity.damage, tempEnities[i].transform.pos.ToLVector3());
        }
    }

}

