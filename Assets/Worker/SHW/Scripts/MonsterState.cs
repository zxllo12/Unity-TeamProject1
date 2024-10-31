using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MonsterState : MonoBehaviour
{
    // 대기, 추적, 공격, 사망, 되돌아가기, 스킬, 이동, 피격
    public enum State { Idle, Running, Attack, Return, Skill, Walking, Dead, IsHit }

    [Header("Setting")]
    [SerializeField] Player_Controller player;    // 추적할 플레이어
    [SerializeField] GameObject bulletPrefab;     // 원거리 공격시 발사할 프리팹
    [SerializeField] Transform shootPoint;        // 발사 포인트
    [SerializeField] Animator animator;           // 재생할 에니메이터
    [SerializeField] AttackTrigger trigger;       // 공격 범위 확인 트리거


    [Header("State")]
    [SerializeField] State curState;         // 현상태
    public Vector3 spawnPoint;               // 기본 위치(임시)

    [SerializeField] int id;
    [SerializeField] float attack;           // 공격력
    [SerializeField] float def;              // 방어력
    [SerializeField] float hp;               // 체력
    public float curHp;                      // 실제 현재 체력
    [SerializeField] float walkSpeed;        // 걷기이속
    [SerializeField] float runSpeed;         // 뛰기이속
    [SerializeField] float attackSpeed;      // 이속
    [SerializeField] float rage;             // 추적거리
    [SerializeField] float attackRage;       // 공격 사거리
    [SerializeField] bool canSkill;          // 스킬여부
    [SerializeField] bool attackType;        // 공격 타입 true일 경우 원거리
    [SerializeField] float bulletSpeed;      // 투사체 발사 속도

    bool canAttack = true;      // 공격 확인
    float attackTimer;          // 공격 타이머

    // 사망확인용
    bool isdead = false;

    protected MonsterData _monsterData;
    public MonsterData MonsterData { get { return _monsterData; } }

    public UnityAction<MonsterState> OnDead;

    private void Awake()
    {
        // 애니메이터 갖고오기
        animator = GetComponent<Animator>();

        LoadMonsterData(id);

        // 스폰 포인트 저장
        spawnPoint = transform.position;

        GameManager.Instance.SetMonster(this);
    }
    private void Start()
    {
        // LoadMonsterData(id);
        player = GameManager.Instance.player;
    }

    private void OnDisable()
    {
        
    }

    public void LoadMonsterData(int id)
    {
        // 오류 확인용
        // Debug.Log($"요청된 몬스터 ID: {id}");

        // id에 해당하는 데이터가 존재하는지 확인하고, 존재하지 않을 경우 오류 출력
        if (DataManager.Instance.MonsterDict.TryGetValue(id, out MonsterData data) == false)
        {
            Debug.LogError($"MonsterData를 찾을 수 없습니다. ID: {id}");
            return;
        }

        // 가져온 값은 선언한 몬스터 데이터에 할당한다.
        _monsterData = data;

        attack = _monsterData.Attack;
        def = _monsterData.Defense;
        hp = _monsterData.Hp;
        curHp = hp;  // 현재 체력 = 설정체력으로 설정
        walkSpeed = _monsterData.WalkSpeed;
        runSpeed = _monsterData.RunSpeed;
        attackSpeed = _monsterData.AttackSpeed;
        rage = _monsterData.Rage;
        attackRage = _monsterData.AttackRage;
        canSkill = _monsterData.CanSkill;
        attackType = _monsterData.AttackType;

        if (trigger != null)
        {
            trigger.SetDamage(attack);
        }
    }

    private void Update()
    {
        // 상태 확인용 디버그 로그
        // Debug.Log(curState);

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
                break;
            case State.Attack: // 공격
                Attack();
                break;
            case State.Skill: // 스킬
                Skill();
                break;
            case State.Walking: // 이동 = 배회
                Walking();
                break;
            /*
        case State.IsHit:   // 피격
            IsHit();
            return;*/
            case State.Dead:  // (임시) 사망
                Dead();
                break;
        }

        if (canAttack == false)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                canAttack = true;
            }
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

        // 플립 반복되는 부분 한번만 실행하도록 설정
        StopAllCoroutines();

        // 스폰지점으로 다시 돌아감
        transform.position = Vector3.MoveTowards(transform.position, spawnPoint, walkSpeed * Time.deltaTime);

        // 일정 범위 내에 플레이어가 들어왔을 경우
        if (Vector3.Distance(transform.position, player.transform.position) < rage)
        {
            Flip();
            animator.SetBool("isWalking", false);  // 애니메이션 취소
            curState = State.Running;   // 추적상태로 변환
        }

        // 스폰포인트에 도착했을 경우
        else if (transform.position.x == spawnPoint.x)
        {
            Flip();
            animator.SetBool("isWalking", false);
            curState = State.Idle;
        }
    }

    public void Attack()
    {
        if (canAttack == true)
        {
            // Debug.Log("공격성공");
            // 공격 애니메이션
            animator.SetBool("isAttacking", true);

            if (attackType == true)
            {
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                Bullet instance = bullet.GetComponent<Bullet>();
                instance.SetSpeed(bulletSpeed);
                instance.SetDamage(attack);
            }
            else
            {
                // 플레이어 공격
                trigger.TirggerOnOff();
            }

            attackTimer = attackSpeed;

            canAttack = false;
        }


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



    }

    public void Dead()
    {
        // 이전 어느상태든 에니메이션 끄기
        AllAnimationOff();

        // 사망 애니메이션
        if (isdead == false)
        {
            animator.SetBool("isDead", true);
            isdead = true;
        }
        else
        {
            OnDead?.Invoke(this);
           // animator.SetBool("isDead", false);
            Destroy(gameObject,3f);
        }
    }

    public void Skill()
    {
        // 스킬이 있는 몬스터의 경우
    }

    public void Walking()
    {
        // 걷기 애니메이션 
        animator.SetBool("isWalking", true);

        if (transform.rotation.eulerAngles.y >= -80)
        {
            transform.position += Vector3.left * walkSpeed * Time.deltaTime;
        }
        else if (transform.rotation.eulerAngles.y <= 80)
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
        // 이전 어느상태든 에니메이션 끄기
        AllAnimationOff();

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
        Debug.Log($"몬스터 충돌 : {collision.gameObject.name}");
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            GameManager.Instance.player.stats.TakeDamage(attack);
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
        // 이전 어느상태든 에니메이션 끄기
        AllAnimationOff();

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

    // 애니메이션 전부 끄는 함수
    public void AllAnimationOff()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isHit", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isStun", false);
    }
}

