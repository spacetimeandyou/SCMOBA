using Lockstep.Collision2D;
using Lockstep.Math;
using System;
using System.Collections.Generic;
using UnityEngine;

class PhysicSystem : BaseSystem
{
    private static PhysicSystem _instance;
    public static PhysicSystem Instance => _instance;
    ICollisionSystem collisionSystem;

    static Dictionary<int, ColliderPrefab> _fabId2ColPrefab = new Dictionary<int, ColliderPrefab>();
    static Dictionary<int, int> _fabId2Layer = new Dictionary<int, int>();

    static Dictionary<ILPTriggerEventHandler, ColliderProxy> _mono2ColProxy =
        new Dictionary<ILPTriggerEventHandler, ColliderProxy>();

    static Dictionary<ColliderProxy, ILPTriggerEventHandler> _colProxy2Mono =
        new Dictionary<ColliderProxy, ILPTriggerEventHandler>();

    public bool[] collisionMatrix => new bool[9];
    public LVector3 pos => new LVector3(0, 0, 0);
    public LFloat worldSize => new LFloat(100);
    public LFloat minNodeSize => new LFloat(1);
    public LFloat loosenessval => new LFloat(true, 1250);

    private int[] allTypes = new int[] { 0, 1, 2 };

    public int showTreeId = 0;

    public override void DoAwake()
    {
        _instance = this;
        if (_instance != this)
        {
            Debug.LogError("Duplicate CollisionSystemAdapt!");
            return;
        }

        var collisionSystem = new CollisionSystem()
        {
            worldSize = worldSize,
            pos = pos,
            minNodeSize = minNodeSize,
            loosenessval = loosenessval
        };
        this.collisionSystem = collisionSystem;
        collisionSystem.DoStart(collisionMatrix, allTypes);
        collisionSystem.funcGlobalOnTriggerEvent += GlobalOnTriggerEvent;
    }

    public override void DoUpdate()
    {
        collisionSystem.ShowTreeId = showTreeId;
        collisionSystem.DoUpdate((LFloat)0.05);
    }

    public static void GlobalOnTriggerEvent(ColliderProxy a, ColliderProxy b, ECollisionEvent type)
    {
        if (_colProxy2Mono.TryGetValue(a, out var handlera))
        {
            CollisionSystem.TriggerEvent(handlera, b, type);
        }

        if (_colProxy2Mono.TryGetValue(b, out var handlerb))
        {
            CollisionSystem.TriggerEvent(handlerb, a, type);
        }
    }


    public static ColliderProxy GetCollider(int id)
    {
        return _instance.collisionSystem.GetCollider(id);
    }

    public void RigisterPrefab(int prefabId, int val)
    {
        _fabId2Layer[prefabId] = val;
    }

    public void RegisterEntity(BaseEntity entity)
    {
        int prefabId = entity.gameObjectEntityID;
        ColliderPrefab prefab;
        var fab = entity.gameObject;
        RigisterPrefab(prefabId, prefabId % 2==0 ? (int)BattleConfig.CollionLayer.Blue : (int)BattleConfig.CollionLayer.Red);
        if (!_fabId2ColPrefab.TryGetValue(prefabId, out prefab))
        {
            prefab = CollisionSystem.CreateColliderPrefab(fab, entity.colliderData);
        }

        AttachToColSystem(_fabId2Layer[prefabId], prefab, entity);
    }

    public void AttachToColSystem(int layer, ColliderPrefab prefab, BaseEntity entity)
    {
        var proxy = new ColliderProxy();
        proxy.EntityObject = entity;
        proxy.Init(prefab, entity.transform);
        proxy.IsStatic = false;
        proxy.LayerType = layer;
        var eventHandler = entity;
        if (eventHandler != null)
        {
            _mono2ColProxy[eventHandler] = proxy;
            _colProxy2Mono[proxy] = eventHandler;
        }

        collisionSystem.AddCollider(proxy);
    }

    public void RemoveCollider(ILPTriggerEventHandler handler)
    {
        if (_mono2ColProxy.TryGetValue(handler, out var proxy))
        {
            RemoveCollider(proxy);
            _mono2ColProxy.Remove(handler);
            _colProxy2Mono.Remove(proxy);
        }
    }

    public void RemoveCollider(ColliderProxy collider)
    {
        collisionSystem.RemoveCollider(collider);
    }

    void OnDrawGizmos()
    {
        collisionSystem?.DrawGizmos();
    }
}


