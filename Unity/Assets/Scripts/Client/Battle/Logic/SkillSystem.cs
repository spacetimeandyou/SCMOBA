using System;
using System.Collections.Generic;
using UnityEngine;

class SkillSystem:BaseSystem
{
    //0：MovementComponent
    public SkillSystem() : base(ComponentType.SkillComponent) { }
    public override void DoUpdate()
    {
        IterateComponent();
    }
    /// <summary>
    /// 迭代组件
    /// </summary>

    public override void IterateComponent()
    {
        List<Entity> entities = EntityManager.Instance.GetEntities();
        for (int i = 0; i < entities.Count; i++)
        {
            if ((entities[i].ComponentId & componentType[0]) != 0)
            {
                ComponentManager.Instance.GetComponent<SkillComponent>(entities[i], componentType[0], out SkillComponent component);
                PlayerEntity playerEntity = PlayerManager.Instance.GetPlayer(entities[i].GameObjectEntityID);
                ExecuteMoveComponent(ref component, playerEntity);
            }
        }
    }
    /// <summary>
    /// 处理实体单个组件逻辑
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    public void ExecuteMoveComponent(ref SkillComponent component, PlayerEntity playerEntity)
    {
        playerEntity.SkillID = component.skillId;
        Debug.Log("后:" + playerEntity.SkillID + "前:" + component.skillId);
    }
}

