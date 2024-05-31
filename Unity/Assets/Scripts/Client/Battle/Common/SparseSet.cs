using System;
using System.Collections.Generic;
using System.Linq;

public class SparseSet<T>
{
    private List<T> dense; //存储组件
    private List<int> sparse; //存储对应实体id
    private int count;

    public SparseSet(int capacity = 10)
    {
        dense = new List<T>(capacity);
        sparse = new List<int>(Enumerable.Repeat(-1, capacity));
        count = 0;
    }

    public void Add(int entityId, T component)
    {
        if (entityId >= sparse.Count)
        {
            int newSize = (entityId + 1) * 2;
            int addCount = newSize - sparse.Count;
            sparse.AddRange(Enumerable.Repeat(-1, addCount));
        }

        dense.Add(component);
        sparse[entityId] = count++;//实体id和sparse下标对应，dense下标和sparse值对应
    }

    public bool Has(int entityId)
    {
        return entityId < sparse.Count && sparse[entityId] != -1 && sparse[entityId] < count;
    }

    public T Get(int entityId)
    {
        if (Has(entityId))
        {
            return dense[sparse[entityId]];
        }
        else
        {
            throw new Exception("Entity ID does not have this component.");
        }
    }

    public void Remove(int entityId)
    {
        if (!Has(entityId)) return;

        int lastEntity = dense.Count - 1;
        dense[sparse[entityId]] = dense[lastEntity];//组件交换
        sparse[entityId] = -1;
        dense.RemoveAt(lastEntity);//移除组件和对应实体id
        count--;
    }
}