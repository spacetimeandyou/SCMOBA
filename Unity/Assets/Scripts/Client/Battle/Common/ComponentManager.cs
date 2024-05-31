using System;
using System.Collections.Generic;
using UnityEngine;

public interface IComp { }

public class CompArray<T> : IComp
{
    private SparseSet<T> _sparseSet = new SparseSet<T>();

    public void Add(int EntityId, T component)
    {
        _sparseSet.Add(EntityId, component);
    }
    public void Remove(int EntityId)
    {
        _sparseSet.Remove(EntityId);
    }
    public bool Has(int EntityId)
    {
        return _sparseSet.Has(EntityId);
    }
    public T Get(int EntityId)
    {
        return _sparseSet.Get(EntityId);
    }
}

public class ComponentManager : Singleton<ComponentManager>
{
    public Dictionary<int, IComp> comps = new Dictionary<int, IComp>();
    public bool HasComponent<T>(ref Entity entity, int componentType)
    {
        if (comps.TryGetValue(componentType, out IComp component))
        {
            return ((CompArray<T>)comps[componentType]).Has(entity.EntityId);
        }
        return false;
    }

    public void AddComponent<T>(ref Entity entity, int componentType, ref T component)
    {
        if (!comps.TryGetValue(componentType, out IComp comp))
        {
            comp = new CompArray<T>();
            comps[componentType] = comp;
        }
        ((CompArray<T>)comp).Add(entity.EntityId, component);
        entity.ComponentId |= componentType;
    }

    public void RemoveComponent(ref Entity entity, int componentType)
    {
        if (comps.TryGetValue(componentType, out IComp component))
        {
            //枚举类型->组件类型
            switch (componentType)
            {
                case (int)ComponentType.HealthComponent:
                    ((CompArray<HealthComponent>)comps[componentType]).Remove(entity.EntityId);
                    break;
                case (int)ComponentType.AttackComponent:
                    ((CompArray<AttackComponent>)comps[componentType]).Remove(entity.EntityId);
                    break;
                case (int)ComponentType.DefenseComponent:
                    ((CompArray<DefenseComponent>)comps[componentType]).Remove(entity.EntityId);
                    break;
                case (int)ComponentType.MovementComponent:
                    ((CompArray<MovementComponent>)comps[componentType]).Remove(entity.EntityId);
                    break;
                case (int)ComponentType.CollisionComponent:
                    ((CompArray<CollisionComponent>)comps[componentType]).Remove(entity.EntityId);
                    break;
                case (int)ComponentType.SkillComponent:
                    ((CompArray<SkillComponent>)comps[componentType]).Remove(entity.EntityId);
                    break;
                case (int)ComponentType.ItemComponent:
                    ((CompArray<ItemComponent>)comps[componentType]).Remove(entity.EntityId);
                    break;
                default:
                    Debug.LogError(componentType + "：此组件不存在枚举定义");
                    break;
            }
            entity.ComponentId &= ~componentType;
        }
    }
    public void GetComponent<T>(Entity entity, int componentType, out T comp)
    {
        comp = ((CompArray<T>)comps[componentType]).Get(entity.EntityId);
    }
}