using System.Collections;
using UnityEngine;

public class MonsterState : MonoBehaviour
{
    // 대기, 추적, 공격, 사망, 되돌아가기, 스킬, 이동, 피격
    public enum State { Idle, Running, Attack, Return, Skill, Walking, Dead, IsHit }

    // 현상태
    [SerializeField] State curState;
    // 추적할 플레이어(임시)
    [SerializeField] GameObject player;
    // 기본 위치(임시)
    public Vector3 spawnPoint;
    // 원거리 공격시 발사할 프리팹
    [SerializeField] GameObject bulletPrefab;
    // 발사 포인트
    [SerializeField] Transform shootPoint;

    // 재생할 에니메이터
    [SerializeField] Animator animator;

    // 이하는 임시적으로 작성된 스텟들임
    [Header("State")]
    [SerializeField] int id;
    [SerializeField] float attack;  // 공격력
    [SerializeField] float def; // 방어력
    [SerializeField] float hp;  // 체력
    public float curHp;
    [SerializeField] float walkSpeed;   // 걷기이속
    [SerializeField] float runSpeed;   // 뛰기이속
    [SerializeField] float attackSpeed; // 이속
    [SerializeField] float rage;    // 추적거리
    [SerializeField] float attackRage;   // 공격 사거리
    [SerializeField] bool canSkill; // 스킬여부
    [SerializeField] bool attackType;  // 공격 타입 true일 경우 원거리

    protected MonsterData _monsterData;
    public MonsterData MonsterData { get { return _monsterData; } }

    private void Awake()
    {
        // 애니메이터 갖고오기
        animator = GetComponent<Animator>();

        // 스폰 포인트 저장
        spawnPoint = transform.position;

        // 플레이어 확인(일단 이름으로 플레이어 오브젝트 찾는다)
        player = GameObject.Find("testPlayer");

        // 현재 체력 = 설정체력으로 설정
        curHp = hp;
    }
    private void Start()
    {
        LoadMonsterData(id);
    }

    public void LoadMonsterData(int id)
    {
        // id 지정
        // id로 데이터메이저에 딕셔너리 접근 value값을 가져온다

        // 오류 확인용
        Debug.Log($"MonsterDict에 저장된 데이터 개수: {_monsterData.ID}");
        Debug.Log($"요청된 몬스터 ID: {id}");

        // id에 해당하는 데이터가 존재하는지 확인하고, 존재하지 않을 경우 오류 출력
        if (DataManager.Instance.MonsterDict.TryGetValue(id, out MonsterData data) == false)
        {
            Debug.LogError($"MonsterData를 찾을 수 없습니다. ID: {id}");
            return;
        }

        _monsterData = data;

        // 가져온 값은 선언한 몬스터 데이터에 할당한다.
        attack = _monsterData.Attack;
        def = _monsterData.Defense;
        hp = _monsterData.Hp;
        walkSpeed = _monsterData.WalkSpeed;
        runSpeed = _monsterData.RunSpeed;
        attackSpeed = _monsterData.AttackSpeed;
        rage = _monsterData.Rage;
        attackRage = _monsterData.AttackRage;
        canSkill = _monsterData.CanSkill;
        attackType = _monsterData.AttackType;
    }

    private void Update()
    {
        // 상태 확인용 디버그 로그
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
            /*
        case State.IsHit:   // 피격
            IsHit();
            return;*/
            case State.Dead:  // (임시) 사망
                Dead();
                return;
        }

        if (curState == State.Attack && attackType == true)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    public void Idle()
    {
        // 대기 애니메이션 모션 출력
        animator.SetBool("isIdle", true);

        StartCoroutine(WalkCoroutine());

        // 일정 범위 내에 플레이어가 들어왔을 경우
        if (Vector3.Distance(transform.position, player.transform.position) < rage)
        {
            StopAllCoroutines();
            animator.SetBool("isIdle", false);  // 애니메이션 취소
            curState = State.Running;   // 추적상태로 변환
        }
    }

    // 대기모션과 배회모션의 코루틴
    IEnumerator WalkCoroutine()
    {
        yield return new WaitForSeconds(3f);
        curState = State.Walking;
        animator.SetBool("isIdle", false);
    }

    public void Running()
    {
        // 추적 애니메이션 실행
        animator.SetBool("isRunning", true);

        StopCoroutine(WalkCoroutine());

        // 타겟(플레이어)를 향해서 이동
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, runSpeed * Time.deltaTime);

        // 공격범위 내로 들어왔을 경우
        if (Vector3.Distance(transform.position, player.transform.position) < attackRage)
        {
            animator.SetBool("isRunning", false);
            curState = State.Attack;
        }

        // 일정 범위 내에 플레이어가 넘어갈 경우
        else if (Vector3.Distance(transform.position, player.transform.position) > rage)
        {
            animator.SetBool("isRunning", false);  // 애니메이션 취소
            curState = State.Idle;   // 추적상태로 변환
        }
    }

    // 이동시 스폰지점으로 돌아가는 Return
    public void Return()
    {
        // 되돌아가는 상태 = 걷는 모션
        animator.SetBool("isWalking", true);
        // 스폰지점으로 다시 돌아감
        transform.position = Vector3.MoveTowards(transform.position, spawnPoint, walkSpeed * Time.deltaTime);

        // 플립 반복되는 부분 한번만 실행하도록 설정
        StopAllCoroutines();


        // 일정 범위 내에 플레이어가 들어왔을 경우
        if (Vector3.Distance(transform.position, player.transform.position) < rage)
        {
            Flip();
            animator.SetBool("isWalking", false);  // 애니메이션 취소
            curState = State.Running;   // 추적상태로 변환
        }

        // 스폰포인트에 도착했을 경우
        else if (transform.position == spawnPoint)
        {
            Flip();
            animator.SetBool("isWalking", false);
            curState = State.Idle;
        }
    }

    public void Attack()
    {
        // 공격 애니메이션
        animator.SetBool("isAttacking", true);

        // 공격시 플레이어 공격에 대해 어떤식으로 할지 논의 필요
        // (임시) 가져온 플레이어의 체력을 가져와서 깎는 방식

        // 원거리 일 경우
        // 한번만 실행시켜야하는데 공격상태일때 계속 반복(수정필요)


        // 공격범위 내로 들어왔을 경우
        if (Vector3.Distance(transform.position, player.transform.position) > attackRage)
        {
            animator.SetBool("isAttacking", false);
            curState = State.Running;
        }
    }

    // 원거리 공격용 코루틴 작성?
    // n초 뒤에 공격을 한번 실행?

    IEnumerator ShootCoroutine()
    {
        Debug.Log("코루틴 시작");

        yield return new WaitForSeconds(2f);

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.velocity = bullet.transform.forward * attackSpeed;
    }

    public void Dead()
    {
        // 사망 애니메이션
        animator.SetBool("isDead", true);
        animator.SetBool("isDead", false);
    }

    public void Skill()
    {
        // 스킬이 있는 몬스터의 경우
    }

    public void Walking()
    {
        // 걷기 애니메이션 
        animator.SetBool("isWalking", true);

        if (transform.rotation.eulerAngles.y <= -80)
        {
            transform.position += Vector3.left * walkSpeed * Time.deltaTime;
        }
        else if (transform.rotation.eulerAngles.y >= 80)
        {
            transform.position += Vector3.right * walkSpeed * Time.deltaTime;
        }

        // 걷기 코루틴 정지 & 되돌아가기 코루틴 시작
        StopCoroutine(WalkCoroutine());
        StartCoroutine(returnCoroutine());

        // 일정 범위 내에 플레이어가 들어왔을 경우
        if (Vector3.Distance(transform.position, player.transform.position) < rage)
        {

            animator.SetBool("isWalking", false);  // 애니메이션 취소
            curState = State.Running;   // 추적상태로 변환
        }
    }

    // 대기모션과 배회모션의 코루틴
    IEnumerator returnCoroutine()
    {
        yield return new WaitForSeconds(3f);
        curState = State.Return;
        animator.SetBool("isWalking", false);
        Flip();
    }

    // 피격시 출력할 함수
    public void IsHit(float damage)
    {
        // 피격 애니메이션 출력
        animator.SetBool("isHit", true);
        animator.SetBool("isHit", false);

        // Hp 감소
        curHp -= damage;

        // 죽었을 경우
        if (curHp <= 0)
        {
            curState = State.Dead;
        }

        // 스턴 공격을 맞았을 경우
        // 스턴함수 실행

    }

    // 충돌 감지
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 물체가 발사체일 경우(플레이어의 공격일 경우)
        if (collision.gameObject.tag == "attack")   // 임의로 정해놓은 상태
        {
            curState = State.IsHit;
        }
    }

    // 회전
    public void Flip()
    {
        // 원래 지점으로 이동을 위한 회전
        if (transform.rotation.eulerAngles.y <= -80)
        {
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180f, 0);
        }
        else if (transform.rotation.eulerAngles.y >= 80)
        {
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180f, 0);
        }
    }

    public void Stun()
    {
        animator.SetBool("isStun", true);
        animator.SetBool("isStun", false);
    }

    // 둔화 
    public void Delay(float ice)
    {
        // 둔화 스킬에 걸렸을 경우 이속 감소?
        // 원래대로 돌릴 방법 필요
        walkSpeed -= ice;
        runSpeed -= ice;
    }

}

