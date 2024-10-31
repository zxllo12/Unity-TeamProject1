using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Vector3 direction;

    public Rigidbody rigid;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [SerializeField] float speed = 200;
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
        GameManager.Instance.SetPlayer(this);
    }

    private void Start()
    {
        GameManager.Instance.player.stats.OnChangedHP += TakeDamageAnimation;
        GameManager.Instance.player.stats.Dead += PlayerDead;
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

        //占쏙옙占쏙옙 占쌍댐옙占쏙옙 확占쏙옙
        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

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

        //캐占쏙옙占쏙옙 占승울옙占쏙옙占
        if (hInput != 0)
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(hInput, 0, 0));
            model.rotation = newRotation;
        }

        //무적 상태
        stats.UpdateInvincibleTime(Time.deltaTime);

        // Get Item Test
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (rewardChest != null)
            {
                rewardChest.OpenChest();
                rewardChest = null;
            }
            else if(curDropItem != null)
            {
                curDropItem.GetItem();
                curDropItem = null;
            }
            else if (gate != null)
            {
                gate.MoveNextScene();
                gate = null;
            }
        }
    }

    private void FixedUpdate()
    {
        //캐릭터 움직임
        rigid.AddForce(Vector3.right * direction.x);
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        GameManager.Instance.player.stats.OnChangedHP -= TakeDamageAnimation;
        GameManager.Instance.player.stats.Dead -= PlayerDead;
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

    private void TakeDamageAnimation()
    {
        animator.SetTrigger("damage");
    }

    private void PlayerDead()
    {
        animator.SetTrigger("die");

        Destroy(gameObject, 3f);
    }

    DropItem curDropItem;
    RewardChest rewardChest;
    Gate gate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RewardChest"))
        {
            RewardChest reward = other.GetComponent<RewardChest>();
            if (reward.isOpened == false)
            {
                rewardChest = other.GetComponent<RewardChest>();
            }
        }
        else if (other.gameObject.CompareTag("DropItem"))
        {
            curDropItem = other.GetComponent<DropItem>();
            Debug.Log($"{other.gameObject.name} 등록");
        }
        else if (other.gameObject.CompareTag("Gate"))
        {
            gate = other.GetComponent<Gate>();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (rewardChest != null && rewardChest.gameObject == other.gameObject) 
        {
            rewardChest = null;
        }
        else if (curDropItem != null && curDropItem.gameObject == other.gameObject)
        {
            curDropItem = null;
            Debug.Log($"{other.gameObject.name} 해제");
        }
        else if (gate != null && gate.gameObject == other.gameObject)
        {
            gate = null;
        }
    }
}
