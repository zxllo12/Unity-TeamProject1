using System.Collections;
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

    //�׽�Ʈ�ڵ� �������ͽ�
    public PlayerStats stats = new PlayerStats();

    //private void Start()
    //{
    //    Debug.Log("�ʱ� ü��: " + stats.currentHealth);
    //}
    //�׽�Ʈ�ڵ� �������ͽ�

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
        //�׽�Ʈ�ڵ� �������ͽ�
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    stats.TakeDamage(20f);
        //    animator.SetTrigger("damage");
        //
        //    Debug.Log("���� ü��: " + stats.currentHealth);
        //}
        //�׽�Ʈ�ڵ� �������ͽ�

        for (int i = 0; i < skillKeys.Length; i++)
        {
            if (Input.GetKeyDown(skillKeys[i]))
            {
                handler.DoSkill((Enums.PlayerSkillSlot)i, firePos, stats.attackPower);
            }
        }

        //�¿� �Է�
        float hInput = Input.GetAxisRaw("Horizontal");
        direction.x = hInput * speed;

        animator.SetFloat("speed", Mathf.Abs(hInput));

        //���� �ִ��� Ȯ��
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
                Debug.Log("�������� ����");
                DoubleJump();
            }
        }

        //ĳ���� �¿����
        if (hInput != 0)
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(hInput, 0, 0));
            model.rotation = newRotation;
        }

        //ĳ���� ������
        rigid.AddForce(Vector3.right * direction.x);
    }

    private void Jump()
    {
        //����
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void DoubleJump()
    {
        //���� ����
        rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        AbleDoubleJump = false;
    }

    private void Atk()
    {
        //����
        animator.SetTrigger("Atk");
    }
}