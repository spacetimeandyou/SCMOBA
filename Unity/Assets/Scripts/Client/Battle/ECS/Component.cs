using Lockstep.Math;
//组件：数据，组件类型id，对应实体id
//输入，移动，技能，物理，敌人，实例化，打印
public enum ComponentType
{
    HealthComponent = 1 << 0,
    AttackComponent = 1 << 1,
    DefenseComponent = 1 << 2,
    MovementComponent = 1 << 3,
    CollisionComponent = 1 << 4,
    SkillComponent = 1 << 5,
    ItemComponent = 1 << 6,
}

public enum SingleComponentType
{
    InputComponent = 1 << 0,
}


//单个组件
interface ISingleComponent {}

//输入组件
class InputComponent:ISingleComponent
{
    public int dirX, dirY;
    public int skillID;
}

//移动组件
class MovementComponent
{
    public int EntityId;
    public LVector2 moveDirection;//移动方向(x,y)
};

// 生命值组件
class HealthComponent
{
    public int EntityId;
    public float currentHealth;//当前血量
    public float maxHealth;//最大血量
    //public float healthRegenRate;//血量恢复速度
};

//攻击组件
class AttackComponent
{
    public int EntityId;
    public float attackPower;//攻击力
    //public float attackSpeed;//攻击速度
    //public float attackRange;//攻击范围
};

//技能组件
class SkillComponent
{
    public int EntityId;
    public int skillId;//技能Id
    //public float cooldown; //技能冷却时间               
    //public float effect;//技能效果
    //public float cost;//技能消耗,蓝量
};

//物品组件
struct ItemComponent
{
    public ComponentType componentTypeId;
    public int EntityId;
    public string itemName; //物品名
    public float attackBonus;//攻击提升
    public float healthBonus;//生命值恢复
};

//防御组件
struct DefenseComponent {
    public ComponentType componentTypeId;
    public int EntityId;
    public float defensePower;//防御力
    public float resistance; //抗性
};

//碰撞组件
struct CollisionComponent
{
    public ComponentType componentTypeId;
    public int EntityId;
    public float radius;
    public enum CollisionType { Circle, Rectangle };
};




