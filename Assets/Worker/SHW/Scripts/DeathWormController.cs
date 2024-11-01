using UnityEngine;

public class DeathWormController : MonoBehaviour
{
    // 데스 웜의 스테이터 불러오기
    // = 기존 데이터 불러오는 것과 같이 한다.

    // 직접 셋팅할 값

    //몬스터 스텟
    [Header("State")]
    public int id = 14; // 데스웜 id 14번
    public bool attackType;        // 공격 타입 true일 경우 원거리
    public float attackRage;       // 공격 사거리
    public float range;             // 추적거리
    public bool canSkill;          // 스킬여부
    public float attack;            // 공격력
    public float def;              // 방어력
    public float hp;               // 체력
    public float attackSpeed;      // 공속

    // 몬스터 데미지 입는 현제 체력 부분
    public float curHp;                      // 실제 현재 체력

    public float walkSpeed;        // 걷기이속
    public float runSpeed;         // 뛰기이속

    public float bulletSpeed;      // 투사체 발사 속도

}
