using System;
using System.Collections.Generic;
using Lockstep.Math;
using UnityEngine;

class MoveSystem : BaseSystem
{
    //0：MovementComponent
    public MoveSystem() : base(ComponentType.MovementComponent) { }
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
                ComponentManager.Instance.GetComponent<MovementComponent>(entities[i], componentType[0],out MovementComponent component);
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
    public void ExecuteMoveComponent(ref MovementComponent component, PlayerEntity playerEntity)
    {
        if(playerEntity.camp == 1) playerEntity.Dir = - component.moveDirection;
        else playerEntity.Dir = component.moveDirection;
    }
}

