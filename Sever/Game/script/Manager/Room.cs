using System;
using System.Collections.Generic;

public class Room
{
    //id
    public int id = 0;
    //玩家列表,是否加载完毕
    public Dictionary<string, bool> playerIds = new Dictionary<string, bool>();
    //状态
    public enum Status
    {
        PREPARE = 0,
        FIGHT = 1,
    }
    public Status status = Status.PREPARE;
    //当前玩家数
    private int curPlayerCount;
    public int CurPlayerCount
    {
        get
        {
            return curPlayerCount > playerIds.Count ? curPlayerCount : playerIds.Count;
        }
        set
        {
            curPlayerCount = value;
        }
    }
    #region 房间
    //最大玩家数
    public int maxPlayer = 10;
    //房主id
    public string ownerId = "";
    //加载完毕数
    public int LoadSceneFinish = 0;
    //战斗完毕数
    public int BattleFinish = 0;
    //胜利阵营
    public int winCamp = 0;
    //添加玩家
    public bool AddPlayer(string id)
    {
        //获取玩家
        Player player = PlayerManager.GetPlayer(id);
        if (player == null)
        {
            Console.WriteLine("room.AddPlayer fail, player is null");
            return false;
        }
        //房间人数
        if (playerIds.Count >= maxPlayer)
        {
            Console.WriteLine("room.AddPlayer fail, reach maxPlayer");
            return false;
        }
        //准备状态才能加人
        if (status != Status.PREPARE)
        {
            Console.WriteLine("room.AddPlayer fail, not PREPARE");
            return false;
        }
        //已经在房间里
        if (playerIds.ContainsKey(id))
        {
            Console.WriteLine("room.AddPlayer fail, already in this room");
            return false;
        }
        playerIds[id] = false;
        //设置玩家数据
        player.camp = SwitchCamp();
        player.roomId = this.id;
        //设置房主
        if (ownerId == "")
        {
            ownerId = player.id;
        }
        //广播
        Broadcast(ToMsg());
        return true;
    }

    //分配阵营
    public int SwitchCamp()
    {
        //计数
        int count1 = 0;
        int count2 = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1) { count1++; }
            if (player.camp == 2) { count2++; }
        }
        //选择
        if (count1 <= count2)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    //是不是房主
    public bool isOwner(Player player)
    {
        return player.id == ownerId;
    }

    //删除玩家
    public bool RemovePlayer(string id)
    {
        //获取玩家
        Player player = PlayerManager.GetPlayer(id);
        if (player == null)
        {
            Console.WriteLine("room.RemovePlayer fail, player is null");
            return false;
        }
        //没有在房间里
        if (!playerIds.ContainsKey(id))
        {
            Console.WriteLine("room.RemovePlayer fail, not in this room");
            return false;
        }
        //删除列表
        playerIds.Remove(id);
        //设置玩家数据
        player.camp = 0;
        player.roomId = -1;
        //设置房主
        if (ownerId == player.id)
        {
            ownerId = SwitchOwner();
        }
        //战斗状态退出
        if (status == Status.FIGHT)
        {
            MsgLeaveBattle msg = new MsgLeaveBattle();
            msg.id = player.id;
            Broadcast(msg);
        }
        //房间为空
        if (playerIds.Count == 0)
        {
            RoomManager.RemoveRoom(this.id);
        }
        //广播
        Broadcast(ToMsg());
        return true;
    }

    //选择房主
    public string SwitchOwner()
    {
        //选择第一个玩家
        foreach (string id in playerIds.Keys)
        {
            return id;
        }
        //房间没人
        return "";
    }


    //广播消息
    public void Broadcast(MsgBase msg)
    {
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.Send(msg);
        }
    }

    //生成MsgGetRoomInfo协议
    public MsgBase ToMsg()
    {
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        int count = playerIds.Count;
        msg.players = new PlayerInfo[count];
        //players
        int i = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            PlayerInfo playerInfo = new PlayerInfo();
            //赋值
            playerInfo.id = player.id;
            playerInfo.camp = player.camp;
            playerInfo.isOwner = 0;
            if (isOwner(player))
            {
                playerInfo.isOwner = 1;
            }

            msg.players[i] = playerInfo;
            i++;
        }
        return msg;
    }

    //能否开战
    public bool CanStartBattle()
    {
        //已经是战斗状态
        if (status != Status.PREPARE)
        {
            return false;
        }
        //统计每个队伍的玩家数
        int count1 = 0;
        int count2 = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1) { count1++; }
            else { count2++; }
        }
        //每个队伍至少要有1名玩家
        //if (count1 < 1 || count2 < 1)
        //{
        //    return false;
        //}
        return true;
    }

    //玩家数据转成HeroInfo
    public HeroInfo PlayerToHeroInfo(Player player, int i)
    {
        HeroInfo heroInfo = new HeroInfo();
        heroInfo.camp = player.camp;
        heroInfo.id = player.id;
        //英雄id绑定,TODO
        heroInfo.HeroId = 0;
        heroInfo.PositionId = i;
        return heroInfo;
    }

    //开始游戏
    public bool StartGame()
    {
        if (!CanStartBattle())
        {
            return false;
        }
        //返回数据
        MsgEnterBattle msg = new MsgEnterBattle();
        //目前只有房间模式地图，TODO
        msg.mapId = 1;
        msg.Heroes = new HeroInfo[playerIds.Count];

        int i = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            msg.Heroes[i] = PlayerToHeroInfo(player, i);
            i++;
        }
        Broadcast(msg);
        return true;
    }
    #endregion

    #region 游戏
    private const double UpdateInterval = 50;
    private DateTime _lastUpdateTime;
    private double _deltaTime;
    public int Tick = 0;//房间Tick

    private List<MsgPlayerInput> _allHistoryFrames;//历史帧
    public void Start()
    {
        CurPlayerCount = playerIds.Count;
        _lastUpdateTime = DateTime.Now;
        _allHistoryFrames = new List<MsgPlayerInput>();
    }
    //每帧更新
    public void Update()
    {
        _deltaTime = (DateTime.Now - _lastUpdateTime).TotalMilliseconds;
        if (_deltaTime > UpdateInterval)
        {
            _lastUpdateTime = _lastUpdateTime.AddMilliseconds(UpdateInterval);
            _CheckBorderServerFrame(true);
        }
    }

    private bool _CheckBorderServerFrame(bool isForce = false)
    {
        var frame = GetOrCreateFrame(Tick);
        var inputs = frame.SPlayerInput;
        if (!isForce)
        {
            //是否所有的输入  都已经等到
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] == null)
                {
                    return false;
                }
            }
        }

        //将所有未到的包 给予默认的输入
        int k = 0;
        foreach (var item in playerIds)
        {
            
            if (inputs[k] == null)
            {
                PlayerInput playerInput = new PlayerInput();
                playerInput.playerId = item.Key;
                playerInput.dirX = 0;
                playerInput.dirY = 0;
                playerInput.skillId = -1;
                inputs[k] = playerInput;
                Console.WriteLine("---服务器Tick---" + Tick);
            }
            k++;
        }
        //开始发送
        for (int i = 0; i < inputs.Length; i++)
        {
            Player player = PlayerManager.GetPlayer(inputs[i].playerId);
            player.Send(frame);
        }
        Tick++;
        
        return true;
    }
    private MsgPlayerInput GetOrCreateFrame(int tick)
    {
        //扩充帧队列
        var frameCount = _allHistoryFrames.Count;
        if (frameCount <= tick)
        {
            var count = tick - _allHistoryFrames.Count + 1;
            for (int i = 0; i < count; i++)
            {
                _allHistoryFrames.Add(null);
            }
        }

        if (_allHistoryFrames[tick] == null)
        {
            _allHistoryFrames[tick] = new MsgPlayerInput() { STick = tick };
        }

        var frame = _allHistoryFrames[tick];
        if (frame.SPlayerInput == null || frame.SPlayerInput.Length != playerIds.Count)
        {
            frame.SPlayerInput = new PlayerInput[CurPlayerCount];
        }

        return frame;
    }
    //添加帧玩家输入
    public void AddFrameInput(int tick, PlayerInput cPlayerInput, int LocatId)
    {
        var frame = GetOrCreateFrame(tick);
        var inputs = frame.SPlayerInput;
        inputs[LocatId] = cPlayerInput;
    }
     
    #endregion
}

