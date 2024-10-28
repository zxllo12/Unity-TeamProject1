using System.Xml.Linq;

public class MonsterData : IDataLoader
{
    public int ID;  // 몬스터 NO.
    public string Name;     // 몬스터 이름
    public bool AttackType;  // 공격 타입 true일 경우 원거리
    public float AttackRage;   // 공격 사거리
    public int Rage;    // 추적거리
    public bool CanSkill; // 스킬여부 : true일 경우 스킬 존재
    public int Attack;  // 공격력
    public int Defense; // 방어력
    public float Hp;  // 체력
    public float AttackSpeed; // 공속
    public float WalkSpeed;   // 걷기속도
    public float RunSpeed;   // 뛰기이속

    public void Load(string[] fields)
    {
       int id = int.Parse(fields[0]);                  // Parse No.
       string name = fields[1];                           // Parse 이름
       bool attackType = bool.Parse(fields[2]);         // Parse 공격 타입
       float attackRage = float.Parse(fields[3]);        // 공격 범위
       int rage = int.Parse(fields[4]);                // 추적거리
       bool canSkill = bool.Parse(fields[5]);           // 스킬여부
       int attack = int.Parse(fields[6]);              // 공격력
       int defense = int.Parse(fields[7]);                 // 방어력
       float hp = float.Parse(fields[8]);                // 체력
       float attackSpeed = float.Parse(fields[9]);       // 공속
       float walkSpeed = float.Parse(fields[10]);        // 걷기속도
       float runSpeed = float.Parse(fields[11]);         // 뛰기속도
    }

    public MonsterData(int id, string name,bool attackType, int attack, int defense, float hp, float walkSpeed, float runSpeed, float attackSpeed, int rage, float attackRage)
    {
        ID = id;
        Name = name;
        AttackType = attackType;    
        Attack = attack;
        Defense = defense;
        Hp = hp;
        WalkSpeed = walkSpeed;
        RunSpeed = runSpeed;
        AttackSpeed = attackSpeed;
        Rage = rage;
        AttackRage = attackRage;
    }
}
