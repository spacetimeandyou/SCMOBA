using System;
using System.Collections.Generic;

class EntityManager: Singleton<EntityManager>
{
    //保存所有实体
    private List<Entity> entities = new List<Entity>();
    private int EntityId = 0;

    public void CreateEntity(out Entity entity)
    {
        entity = new Entity();
        entity.EntityId = EntityId++;
        entity.ComponentId = 0;
        entities.Add(entity);
    }

    public void DestroyEntity(ref Entity entity)
    {
        //实体对应所有组件全部删掉
        foreach (var item in ComponentManager.Instance.comps)
        {
            if((entity.ComponentId & item.Key) != 0)
            {
                ComponentManager.Instance.RemoveComponent(ref entity,item.Key);
            }
        } 
        entity.ComponentId = 0;
        entities.Remove(entity);
    }

    public List<Entity> GetEntities()
    {
        return this.entities;
    }
}

