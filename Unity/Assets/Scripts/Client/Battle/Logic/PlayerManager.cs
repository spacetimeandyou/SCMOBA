using System;
using System.Collections.Generic;
using UnityEngine;

class PlayerManager:Singleton<PlayerManager>
{
    //绑定实体和游戏对象，索引就是几号玩家
    private PlayerEntity[] playerEntities = new PlayerEntity[BattleManager.Instance.PlayerCount];
    //先两个主水晶
    private TowerEntity[] towerEntities = new TowerEntity[2];
    private int gameObjectEntityID = 0;
    #region 玩家
    //创建时需要创建游戏对象，实体，并添加相应组件
    public PlayerEntity CreatPlayer(int LocatId,string id,int camp)
    {
        PlayerEntity playerEntity = new PlayerEntity();
        //实体
        playerEntity.entity = PlayerAddComponent();
        playerEntity.entity.GameObjectEntityID = gameObjectEntityID;
        playerEntity.gameObjectEntityID = gameObjectEntityID;
        gameObjectEntityID++;
        //创建玩家游戏对象
        playerEntity.gameObject = CreatGameObject();
        BindEntityView(playerEntity);
        //玩家id
        playerEntity.playerId = id;
        playerEntity.camp = camp;
        playerEntity.SetBirthPos(LocatId);
        playerEntities[LocatId] = playerEntity;
        return playerEntity;
    }
    public PlayerEntity GetPlayer(int LocatId)
    {
        return playerEntities[LocatId];
    }
    public PlayerEntity[] GetAllPlayer()
    {
        return playerEntities;
    }
    private GameObject CreatGameObject()
    {
        //TODO：接入对象池
        GameObject prefab = Resources.Load<GameObject>("Prefabs/HeroPrefabs/hero_1");
        GameObject result = UnityEngine.Object.Instantiate(prefab);
        return result;
    }
    private Entity PlayerAddComponent()
    {
        //TODO:在此处添加实体和组件
        EntityManager.Instance.CreateEntity(out Entity entity);

        MovementComponent moveComponent = new MovementComponent();
        moveComponent.EntityId = entity.EntityId;
        ComponentManager.Instance.AddComponent(ref entity, (int)ComponentType.MovementComponent,ref moveComponent);

        SkillComponent skillComponent = new SkillComponent();
        skillComponent.EntityId = entity.EntityId;
        ComponentManager.Instance.AddComponent(ref entity, (int)ComponentType.SkillComponent,ref skillComponent);

        HealthComponent hpComponent = new HealthComponent();
        hpComponent.EntityId = entity.EntityId;
        ComponentManager.Instance.AddComponent(ref entity, (int)ComponentType.HealthComponent, ref hpComponent);
        return entity;
    }
    #endregion
    public TowerEntity CreatTowerEntity(int index)
    {
        //实体
        TowerEntity towerEntity = new TowerEntity();
        towerEntity.camp =  (index + 1)%2 == 1? 1 : 2;
        towerEntities[index] = towerEntity;

        towerEntity.entity = TowerAddCompent();
        towerEntity.entity.GameObjectEntityID = gameObjectEntityID;
        towerEntity.gameObjectEntityID = gameObjectEntityID;
        gameObjectEntityID++;
        
        towerEntity.gameObject = GetMainTowerObject(towerEntity.camp);
        BindEntityView(towerEntity);
        towerEntity.ResetPos(towerEntity.gameObject);
        return towerEntity;
    }
    private GameObject GetMainTowerObject(int camp)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Tower");
        GameObject target = null;
        string towerName;
        if (camp == 1) towerName = "BlueMainTower";
        else towerName = "RedMainTower";
        foreach (GameObject obj in objects)
        {
            if (obj.name == towerName)
            {
                target = obj;
                break;
            }
        }
        return target;
    }
    private Entity TowerAddCompent()
    {
        EntityManager.Instance.CreateEntity(out Entity entity);

        HealthComponent hpComponent = new HealthComponent();
        hpComponent.EntityId = entity.EntityId;
        ComponentManager.Instance.AddComponent(ref entity, (int)ComponentType.HealthComponent, ref hpComponent);
        return entity;
    }
    private void BindEntityView(BaseEntity entity)
    {
        entity.gameObject.AddComponent<EntityView>();
        EntityView entityView = entity.gameObject.GetComponent<EntityView>();
        entity.entityView = entityView;
        entityView.BindEntity(entity);
    }
}

