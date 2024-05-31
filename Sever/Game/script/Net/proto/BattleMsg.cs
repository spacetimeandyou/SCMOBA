//英雄信息
[System.Serializable]
public class HeroInfo{
	public string id = "";	//玩家id
	public int camp = 0;    //阵营
	public int HeroId = 0;  //英雄id
	public int PositionId;  //几号玩家
	//物攻，物防，法伤，法防，Buff
}

//进入战斗（服务端推送）
public class MsgEnterBattle:MsgBase {
	public MsgEnterBattle() {protoName = "MsgEnterBattle"; }
	//服务端回
	public HeroInfo[] Heroes;
	public int mapId = 1;
}

//加载战斗场景完毕
public class MsgBattleScene : MsgBase
{
    public MsgBattleScene() { protoName = "MsgBattleScene"; }
    //服务端回
    public bool AllFinishBattleScene;//全部加载完毕
}

//战斗结果（服务端推送）
public class MsgBattleResult:MsgBase {
	public MsgBattleResult() {protoName = "MsgBattleResult";}
	//服务端回
	public int winCamp = 0;	 //获胜的阵营
}

//玩家退出（服务端推送）
public class MsgLeaveBattle:MsgBase {
	public MsgLeaveBattle() {protoName = "MsgLeaveBattle";}
	//服务端回
	public string id = "";	//玩家id
}

[System.Serializable]
public class PlayerInput
{
    public string playerId; //玩家id
    public int dirX, dirY;    //方向
    public int skillId;//技能id                  
}
public class MsgPlayerInput : MsgBase
{
    public MsgPlayerInput() { protoName = "MsgPlayerInput"; }

	//客户端发
	public int locatId;
    public int CTick;
    public PlayerInput CPlayerInput;
    //服务器回
    public int STick;
    public PlayerInput[] SPlayerInput;
}