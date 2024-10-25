using UnityEngine;

public class MonsterState : MonoBehaviour
{
    // 대기, 추적, 공격, 사망, 되돌아가기, 스킬, 이동, 피격
    public enum State { Idle, Running, Attack, Return, Skill, Walking, Dead, ISHit }

    // 현상태
    [SerializeField] State curState;
    // 추적할 플레이어(임시)
    [SerializeField] GameObject player;
    // 기본 위치(임시)
    public Vector3 spawnPoint;

    // 재생할 에니메이터
    [SerializeField] Animator animator;
    // 사용할때 아래처럼 해시화 해서 사용
    private static int idleHash = Animator.StringToHash("Idle");
    private static int walkHash = Animator.StringToHash("Walk");


    // 데이터 테이블로 블러올 내용들
    // 사거리, 스킬여부, 공격력, 방어력, 체력, 공속, 이속, 스킬 쿨

    // 이하는 임시적으로 작성된 스텟들임
    [Header("State")]
    [SerializeField] float attack;  // 공격력
    [SerializeField] float def; // 방어력
    [SerializeField] float hp;  // 체력
    [SerializeField] float speed;   // 공속
    [SerializeField] float attackSpeed; // 이속
    [SerializeField] float rage;    // 추적거리
    [SerializeField] float attackRage;   // 공격 사거리
    [SerializeField] bool canSkill; // 스킬여부
    [SerializeField] bool attackType;  // 공격 타입

    private void Awake()
    {
        // 데이터 테이블에서 정보 가져오기

        // 초기 지점 설정
        spawnPoint = transform.position;

        // 애니메이터 갖고오기
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 추적할 플레이어 오브젝트?컴포넌트 찾기
        // Transform playerPos = player.GetComponent<Transform>();

        Debug.Log(curState);
   

        // 상태패턴
        switch (curState)
        {
            case State.Idle:    // 대기 = 숨쉬기
                Idle();
                break;
            case State.Running:     // 뛰기 = 추적
                Running();
                break;
            case State.Return:  // 리턴 = 되돌아가기
                Return();
                return;
            case State.Attack: // 공격
                Attack();
                return;
            case State.Skill: // 스킬
                Skill();
                return;
            case State.Walking: // 이동 = 배회
                Walking();
                return;
            case State.ISHit:   // 피격
                IsHit();
                return;
            case State.Dead:  // (임시) 사망
                Dead();
                return;
        }
    }

    public void Idle()
    {
        // 대기 애니메이션 모션 출력
        animator.SetBool("isIdle", true);

        // 플레이어가 범위 내로 들어왔을 경우
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < rage)
        {
            curState = State.Running;
        }

        // 

        // 일정 시간이 되면 
        // 코루틴
    }

    public void Running()
    {
        // 뛰는 애니메이션 출력
        // animator.SetFloat("speed", 1);

        // 타겟 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        // 플레이어가 범위를 벗어났을 경우
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) > rage)
        {
            curState = State.Return;
        }

        // 플레이어가 공격범위로 들어왔을 경우
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < attackRage)
        {
            curState = State.Attack;
        }
    }

    public void Return()
    {
        // (TODO)걷기 애니메이션 출력

        // 원래 자리로 이동
        transform.position = Vector3.MoveTowards(transform.position, spawnPoint, speed * Time.deltaTime);

        // 원래 자리로 이동 완료시 
        if (transform.position == spawnPoint)
        {
            curState = State.Idle;
        }
    }

    public void Attack()
    {
        // 공격 애니메이션 출력

        // 근거리 원거리 확인

        // 플레이어 체력 감소

    }

    public void Dead()
    {
        // 체력이 다 닳을경우 사망
        Destroy(gameObject);
    }

    public void Skill()
    {
        // 스킬쪽과 혐력해서 작성
        // (이하는 임시) 
        // 스킬북이 존재한다면
        // 스킬 북 컴포넌트 중 이름이 같은 부분을 가져와
        // 오브젝트의 이름과 비교하여 스킬을 가져오는 방식으로?
    }

    public void Walking()
    {
        // 이동 애니메이션 출력

        // 이동
        transform.position += Vector3.left * speed * Time.deltaTime;
        // 후 리턴상태로 제자리로 돌아간다?
    }

    public void IsHit()
    {
        // 피격
        // 몬스터의 체력 감소
        // 피격 에니메이션 재성
        // 스턴
    }

    // IEnumerator 

    // (TODO)스턴상태일 경우 불러올 함수 작성
}
