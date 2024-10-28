public class MonsterData : IDataLoader
{
    public int id;  // 몬스터 NO.
    public string name;     // 몬스터 이름
    public bool attackType;  // 공격 타입 true일 경우 원거리
    public float attackRage;   // 공격 사거리
    public int rage;    // 추적거리
    public bool canSkill; // 스킬여부 : true일 경우 스킬 존재
    public int attack;  // 공격력
    public int def; // 방어력
    public float hp;  // 체력
    public float attackSpeed; // 공속
    public float walkSpeed;   // 걷기속도
    public float runSpeed;   // 뛰기이속

    public void Load(string[] fields)
    {
        id = int.Parse(fields[0]);                  // Parse No.
        name = fields[1];                           // Parse 이름
        attackType = bool.Parse(fields[2]);         // Parse 공격 타입
        attackRage = float.Parse(fields[3]);        // 공격 범위
        rage = int.Parse(fields[4]);                // 추적거리
        canSkill = bool.Parse(fields[5]);           // 스킬여부
        attack = int.Parse(fields[6]);              // 공격력
        def = int.Parse(fields[7]);                 // 방어력
        hp = float.Parse(fields[8]);                // 체력
        attackSpeed = float.Parse(fields[9]);       // 공속
        walkSpeed = float.Parse(fields[10]);        // 걷기속도
        runSpeed = float.Parse(fields[11]);         // 뛰기속도
    }
}
