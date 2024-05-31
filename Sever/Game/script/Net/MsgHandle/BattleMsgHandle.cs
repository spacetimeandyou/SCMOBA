using System;


public partial class MsgHandler
{
    //加载战斗场景完毕
    public static void MsgBattleScene(ClientState c, MsgBase msgBase)
    {
        Room room = RoomManager.GetRoom(c.player.roomId);
        //战斗已开始
        if (room.status == Room.Status.FIGHT) return;
        room.LoadSceneFinish++;
        room.playerIds[c.player.id] = true;
        //房间玩家未加载完毕
        if (room.LoadSceneFinish < room.playerIds.Count) return;
        foreach(bool isLoadFinsih in room.playerIds.Values)
        {
            if (!isLoadFinsih) return;
        }
        //满足全部加载完毕条件
        room.status = Room.Status.FIGHT;
        room.Start();
        MsgBattleScene msg = (MsgBattleScene)msgBase;
        msg.AllFinishBattleScene = true;
        //分发消息
        foreach (string id in room.playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.Send(msg);
        }
    }

    //战斗结果
    public static void MsgBattleResult(ClientState c, MsgBase msgBase)
    {
        MsgBattleResult msg = (MsgBattleResult)msgBase;
        Room room = RoomManager.GetRoom(c.player.roomId);
        if (room.winCamp == 0) room.winCamp = msg.winCamp;
        //TODO校验，有作弊玩家或者确定性出现问题
        if (room.winCamp != msg.winCamp) return;
        //房间玩家未全部战斗完毕
        if (++room.BattleFinish < room.playerIds.Count) return;
        //满足全部条件
        foreach (string id in room.playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.roomId = -1;
            player.camp = 0;
            player.HeroId = 0;
            player.Send(msg);
        }
        RoomManager.RemoveRoom(room.id);
    }
    //玩家退出，断开连接才能退出
    public static void MsgLeaveBattle(ClientState c, MsgBase msgBase)
    {

    }

    //玩家输入
    public static void MsgPlayerInput(ClientState c, MsgBase msgBase)
    {
        Room room = RoomManager.GetRoom(c.player.roomId);
        MsgPlayerInput msg = (MsgPlayerInput)msgBase;
        room.AddFrameInput(msg.CTick, msg.CPlayerInput, msg.locatId);
    }
}


