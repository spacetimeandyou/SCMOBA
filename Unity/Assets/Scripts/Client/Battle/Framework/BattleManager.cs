/****************************************************
	文件：BattleManager.cs
	作者：孙淳
	邮箱: 1832259562@qq.com
	日期：2024/05/09 18:45   	
	功能：战斗管理器
*****************************************************/
using NetMsg.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

   
class BattleManager : Singleton<BattleManager>
{
    public static bool StartTick { get; private set; }//是否开始Tick
    public static int CurTick { get; set; }//表现帧
    public static int Tick { get; set; }//逻辑帧

    public static Dictionary<int, PlayerInput[]> _allHistoryFrames = new Dictionary<int, PlayerInput[]>(); //历史帧

    private World world = null;
    /// <summary>
    /// 频繁创建比较麻烦，后续会用引用池
    /// </summary>
    private PlayerInput playerInput = null;
    private MsgPlayerInput msgPlayerInput = null;

    /// <summary>
    /// 帧同步
    /// </summary>
    private const double UpdateInterval = 50;
    private DateTime _lastUpdateTime;
    private double _deltaTime;

    /// <summary>
    /// 玩家所需数据
    /// </summary>
    public int PlayerCount;
    public HeroInfo[] configInfo; //索引即几号玩家


    public void Init()
    {
        EventCenter.Instance.AddListener("MsgEnterBattle", MsgEnterBattle);
        EventCenter.Instance.AddListener("MsgBattleScene", MsgBattleScene);
        EventCenter.Instance.AddListener("MsgBattleResult", MsgBattleResult);
        EventCenter.Instance.AddListener("MsgLeaveBattle", MsgLeaveBattle);
        EventCenter.Instance.AddListener("MsgPlayerInput", MsgPlayerInput);
    }

    private void MsgEnterBattle(object sender, EventArgs e)
    {
        ResourcesManager.Instance.AsyncLoadScene(SceneName.GameBattle, () =>
        {
            UIManager.Instance.ActiveUI<Form_Battle>(UIPanel.Form_Battle);
            //解析数据
            MsgEnterBattle msg = (MsgEnterBattle)((MsgEventArgs)e).msgBase;
            SaveAndSetBattlePosition(msg);
            //发送加载战斗完毕消息
            MsgBattleScene msgBattleScene = new MsgBattleScene();
            NetManager.Send(msgBattleScene);
        });
    }

    private void SaveAndSetBattlePosition(MsgEnterBattle msg)
    {
        PlayerCount = msg.Heroes.Length;
        configInfo = new HeroInfo[PlayerCount];
        for (int i = 0; i < PlayerCount; i++)
        {
            //将数据按照位置索引保存到数组中
            configInfo[msg.Heroes[i].PositionId] = msg.Heroes[i];
            if (msg.Heroes[i].id == GameRoot.id)
            {
                GameRoot.camp = msg.Heroes[i].camp;
                GameRoot.locatId = msg.Heroes[i].PositionId;
            }
        }
    }

    private void MsgBattleScene(object sender, EventArgs e)
    {
        //加载场景完毕，开始tick
        MsgBattleScene msg = (MsgBattleScene)((MsgEventArgs)e).msgBase;
        if (msg.AllFinishBattleScene)
        {
            UIManager.Instance.ActiveUI<Form_Battle>(UIPanel.Form_Battle);
            this.DoAwake();
        }
    }

    private void MsgBattleResult(object sender, EventArgs e)
    {
        //TODO
        //检测到水晶破碎
        MsgBattleResult msg = (MsgBattleResult)((MsgEventArgs)e).msgBase;
        //展示成功失败界面，点击后返回，加载GameStart
        UIManager.Instance.ActiveUI<Form_BattleResult>(UIPanel.Form_BattleResult, msg.winCamp);
        this.DoDestory();
    }

    private void MsgLeaveBattle(object sender, EventArgs e)
    {
        //成功离开，返回主界面
        this.DoDestory();
    }

    private void MsgPlayerInput(object sender, EventArgs e)
    {
        MsgPlayerInput msg = (MsgPlayerInput)((MsgEventArgs)e).msgBase;
        _allHistoryFrames[msg.STick] = msg.SPlayerInput;

        // 将字典的信息格式化为字符串
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("当前帧" + msg.STick);
        for (int i = 0; i < msg.SPlayerInput.Length; i++)
        {
            int x = msg.SPlayerInput[i].dirX;
            int y = msg.SPlayerInput[i].dirY;
            int id = msg.SPlayerInput[i].skillId;
            sb.AppendLine("第" + i + "个: " + x + "/" + y + "/" + id);
        }
        // 将字符串写入到文件中
        File.AppendAllText("info.txt", sb.ToString());
    }

    private void DoAwake()
    {
        world = new World();
        playerInput = new PlayerInput();
        msgPlayerInput = new MsgPlayerInput();
        world.DoAwake();
        _lastUpdateTime = DateTime.Now;
        StartTick = true;
        CurTick = 0;
        Tick = 5;
    }

    public void Update()
    {
        if (StartTick)
        {
            //逻辑帧
            //_deltaTime += Time.deltaTime;
            _deltaTime = (DateTime.Now - _lastUpdateTime).TotalMilliseconds;
            while (_deltaTime> UpdateInterval)
            {
                _deltaTime -= UpdateInterval;
                _lastUpdateTime = _lastUpdateTime.AddMilliseconds(UpdateInterval);
                SendPlayerInput();
                Tick++;
                //// 将字典的信息格式化为字符串
                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("当前帧" + Tick+"最后更新时间"+DateTime.Now);
                //// 将字符串写入到文件中
                //File.AppendAllText("client.txt", sb.ToString());
            }
            world.DoUpdate();
        }
    }

    private void DoDestory()
    {
        StartTick = false;
        world.DoDestroy();
        world = null;
    }

    public void SendPlayerInput()
    {
        InputComponent inputComponent = (InputComponent)SingleSystem.SingleComponents[(int)SingleComponentType.InputComponent];
        playerInput.playerId = GameRoot.id;
        playerInput.skillId = inputComponent.skillID;
        playerInput.dirX = inputComponent.dirX;
        playerInput.dirY = inputComponent.dirY;

        //发送网络消息
        msgPlayerInput.CPlayerInput = playerInput;
        msgPlayerInput.CTick = Tick;
        msgPlayerInput.locatId = GameRoot.locatId;
        NetManager.Send(msgPlayerInput);
    }
}