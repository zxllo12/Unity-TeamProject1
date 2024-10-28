using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 direction;
    public float speed = 8;
    public float jumpForce = 10;
    public float gravity = -20;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public Animator animator;
    public Transform model;

    //테스트코드
    public PlayerStats stats = new PlayerStats();

    private void Start()
    {
        Debug.Log("초기 체력: " + stats.currentHealth);
    }
    //테스트코드

    void Update()
    {
        //테스트코드
        if (Input.GetKeyDown(KeyCode.C))
        {
            stats.TakeDamage(20f);
            Debug.Log("현재 체력: " + stats.currentHealth);
        }
        //테스트코드

        //Take the horizontal input to move the player
        float hInput = Input.GetAxis("Horizontal");
        direction.x = hInput * speed;
        animator.SetFloat("speed", Mathf.Abs(hInput));

        //Check if the player is on the ground
        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.15f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.X))
        {
            Atk();
        }

        if (isGrounded)
        {
            direction.y = -1;
            if (Input.GetButton("Jump"))
            {
                Jump();
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;//Add Gravity
        }

        //Flip the player
        if (hInput != 0)
        {
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(hInput, 0, 0));
            model.rotation = newRotation;
        }

        //Move the player using the character controller
        controller.Move(direction * Time.deltaTime);
    }

    private void Jump()
    {
        //Jump
        direction.y = jumpForce;
    }

    private void Atk()
    {
        //Attack
        animator.SetTrigger("Atk");
    }
}
