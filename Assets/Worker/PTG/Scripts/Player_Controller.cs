癤using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public Rigidbody rigid;
    private Vector3 direction;
    public float speed = 8;
    public float jumpForce = 10;
    public float gravity = -20;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public bool AbleDoubleJump = true;

    public Animator animator;
    public Transform model;

    public SkillHandler handler;

    [SerializeField] Transform firePos;

    [SerializeField] KeyCode[] skillKeys = new KeyCode[(int)Enums.PlayerSkillSlot.Length];


    //테스트코드 스테이터스
    public PlayerStats stats = new PlayerStats();

    //private void Start()
    //{
    //    Debug.Log("占십깍옙 체占쏙옙: " + stats.currentHealth);
    //}
    //테스트코드 스테이터스


    private void Awake()
    {
        handler = GetComponent<SkillHandler>();
    }

    private void Start()
    {
        DataManager.Instance.OnLoadCompleted += testInit;
    }

    private void OnEnable()
    {
        GameManager.Instance.SetPlayer(this);
    }

    public void testInit()
    {
        handler.EquipSkill(0, Enums.PlayerSkillSlot.Slot1);
        handler.EquipSkill(1, Enums.PlayerSkillSlot.Slot2);
    }


    private void OnDisable()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.OnLoadCompleted += testInit;
        }
    }
    void Update()
    {
        //테스트코드 스테이터스
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    stats.TakeDamage(20f);
        //    animator.SetTrigger("damage");
        //
        //    Debug.Log("현재 체력: " + stats.currentHealth);
        //}
        //테스트코드 스테이터스

        for (int i = 0; i < skillKeys.Length; i++)
        {
            if (Input.GetKeyDown(skillKeys[i]))
            {
                handler.DoSkill((Enums.PlayerSkillSlot)i, firePos, stats.attackPower);
            }
        }

        //좌우 입력
        float hInput = Input.GetAxisRaw("Horizontal");
        direction.x = hInput * speed;

        animator.SetFloat("speed", Mathf.Abs(hInput));

        //占쏙옙占쏙옙 占쌍댐옙占쏙옙 확占쏙옙
        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.X))
        {
            Atk();
        }

        if (isGrounded)
        {
            direction.y = -1;
            AbleDoubleJump = true;

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            if (AbleDoubleJump && Input.GetButtonDown("Jump"))
            {
                Debug.Log("더블점프 실행");
                DoubleJump();
            }
        }

        //캐占쏙옙占쏙옙 占승울옙占쏙옙占
        if (hInput != 0)
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(hInput, 0, 0));
            model.rotation = newRotation;
        }

        //캐릭터 움직임
        rigid.AddForce(Vector3.right * direction.x);
    }

    private void Jump()
    {
        //점프
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void DoubleJump()
    {
        //더블 점프
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        AbleDoubleJump = false;
    }

    private void Atk()
    {
        //占쏙옙占쏙옙
        animator.SetTrigger("Atk");
    }
}