/****************************************************
	文件：World.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/05/09 18:40   	
	功能：World执行System
*****************************************************/
using Lockstep.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

class World:BaseLifeCycle
{
    //创建实体
    public override void DoAwake()
	{
        RegisterSystems();
        foreach (var system in SystemManager.Instance.GetAllSystems())
        {
            system.DoAwake();
        }
        CreatPlayerEntity();
        CreatTowerEntity();
    }
    private void RegisterSystems()
    {
        SystemManager.Instance.RegisterSystem(new SingleSystem());
        SystemManager.Instance.RegisterSystem(new MoveSystem());
        SystemManager.Instance.RegisterSystem(new PhysicSystem());
        
    }
    //创建玩家实体，确定出生点位置
    private void CreatPlayerEntity()
    {
        int maxCount = BattleManager.Instance.PlayerCount;
        HeroInfo[] data = BattleManager.Instance.configInfo;
        for (int i = 0; i < maxCount; i++)
        {
            PlayerEntity player = PlayerManager.Instance.CreatPlayer(i, data[i].id,data[i].camp);
            //添加进物理系统
            PhysicSystem.Instance.RegisterEntity(player);
            if(GameRoot.locatId == i)
            {
                Camera mainCamera = Camera.main;
                if (data[i].camp == 1) mainCamera.transform.localPosition = new Vector3(-4.68f, 6.66f, -2.36f);
                else mainCamera.transform.localPosition = new Vector3(-95.83f, 4.56f, -103.17f);
                mainCamera.gameObject.AddComponent<CameraFollow>();
                mainCamera.transform.LookAt(player.gameObject.transform);
                CameraFollow cameraFollow = mainCamera.gameObject.GetComponent<CameraFollow>();
                cameraFollow.target = player.gameObject.transform;
            }
        }
    }

    private void CreatTowerEntity()
    {
        for (int i = 0; i < 2; i++)
        {
            TowerEntity tower = PlayerManager.Instance.CreatTowerEntity(i);
            //添加进物理系统
            PhysicSystem.Instance.RegisterEntity(tower);
        }
    }
    public override void DoUpdate()
	{
        //表现
        if (BattleManager.CurTick < BattleManager.Tick && BattleManager._allHistoryFrames.ContainsKey(BattleManager.CurTick))
        {
            WorldExecute(BattleManager._allHistoryFrames[BattleManager.CurTick]);
            //执行历史帧数据
            ++BattleManager.CurTick;
        }
    }
    public override void DoDestroy()
	{
        foreach (var system in SystemManager.Instance.GetAllSystems())
        {
            system.DoDestroy();
        }
        SystemManager.Instance.ClearSystem();//清空系统
    }
    /// <summary>
    /// TODO执行逻辑
    /// </summary>
    /// <param name="playerInputs"></param>
    private void WorldExecute(PlayerInput[] playerInputs)
    {
        PlayerEntity[] players = PlayerManager.Instance.GetAllPlayer();

        // 将字典的信息格式化为字符串
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < playerInputs.Length; i++)
        {
            int x = playerInputs[i].dirX;
            int y = playerInputs[i].dirY;
            int id = playerInputs[i].skillId;
            sb.AppendLine("第" + i + "个: " + x + "/" + y + "/" + id);
        }
        // 将字符串写入到文件中
        File.AppendAllText("input.txt", sb.ToString());

        //执行对应玩家操作
        for (int i = 0; i < playerInputs.Length; i++)
        {
            //移动
            ComponentManager.Instance.GetComponent<MovementComponent>(players[i].entity,(int)ComponentType.MovementComponent,out MovementComponent moveComp);
            moveComp.moveDirection = new LVector2(playerInputs[i].dirX,playerInputs[i].dirY);
            //moveComp.moveDirection = new LVector2(1, 1);
            Debug.Log(moveComp.moveDirection.x + moveComp.moveDirection.y);

            //技能
            ComponentManager.Instance.GetComponent<SkillComponent>(players[i].entity, (int)ComponentType.SkillComponent, out SkillComponent skillComp);
            skillComp.skillId = playerInputs[i].skillId;
        }
        foreach (var system in SystemManager.Instance.GetAllSystems())
        {
            system.DoUpdate();
        }
    }
}

