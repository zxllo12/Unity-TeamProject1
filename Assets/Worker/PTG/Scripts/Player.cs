using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Animator animator;

    [SerializeField] float movePower;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float maxFallSpeed;

    [SerializeField] bool isGrounded;

    private float x;

    private static int idleHash = Animator.StringToHash("idle");
    private static int runHash = Animator.StringToHash("run");
    private static int jumpHash = Animator.StringToHash("jump");

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        Idle();

        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Idle()
    {
        if (rigid.velocity.sqrMagnitude < 0.01f)
        {
            animator.Play(idleHash);
            isGrounded = true;
        }
    }

    private void Move()
    {
        rigid.AddForce(Vector2.right * x * movePower, ForceMode.Force);
        if (rigid.velocity.x > maxMoveSpeed)
        {
            rigid.velocity = new Vector2(maxMoveSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < -maxMoveSpeed)
        {
            rigid.velocity = new Vector2(-maxMoveSpeed, rigid.velocity.y);
        }

        if (rigid.velocity.y < -maxFallSpeed)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -maxFallSpeed);
        }

        //if (x < 0)
        //{
        //    render.flipX = true;
        //}
        //else if (x > 0)
        //{
        //    render.flipX = false;
        //}

        if (rigid.velocity.sqrMagnitude > 0.01f)
        {
            animator.Play(runHash);
            isGrounded = true;
        }

    }
    private void Jump()
    {
        if (isGrounded == false)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }

        if (rigid.velocity.y > 0.01f)
        {
            animator.Play(jumpHash);
            isGrounded = false;
        }

    }
}
