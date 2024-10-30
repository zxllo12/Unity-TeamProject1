using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Vector3 direction;

    public Rigidbody rigid;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [SerializeField] float speed = 8;
    [SerializeField] float jumpForce = 10;

    public Animator animator;

    public Transform model;

    public SkillHandler handler;

    bool AbleDoubleJump = true;

    [SerializeField] Transform firePos;

    [SerializeField] KeyCode[] skillKeys = new KeyCode[(int)Enums.PlayerSkillSlot.Length];

    public PlayerStats stats = new PlayerStats();

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

        //땅에 있는지 확인
        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.X))
        {
            Atk();
        }

        if (isGrounded)
        {
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
                DoubleJump();
            }
        }

        //캐릭터 좌우반전
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
        //공격
        animator.SetTrigger("Atk");
    }
}
