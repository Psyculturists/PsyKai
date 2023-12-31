﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Transform ceilingCheck;
    public Transform groundCheck;
    public LayerMask groundObjects;
    public float checkRadius;
    public int maxJumpCount;
    public Animator animator;
    public float addedRunSpeed;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private float moveDirection;
    private bool isJumping = false;
    private bool isGrounded;
    private int jumpCount;
    private float runningState;

    // Awake is called after all objects are initialized, called in a random order
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        jumpCount = maxJumpCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            animator.SetFloat("Speed", 0f); ;
            return;
        }

        animator.SetFloat("Speed", Mathf.Abs(moveDirection));
        
        if (!isGrounded)
        {
            animator.SetFloat("Speed", 0f);
        }        

        // Get inputs
        ProcessInput();

        // Animate
        Animate();
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
            return;

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);
        if (isGrounded)
        {
            jumpCount = maxJumpCount;
        }

        // Move
        Move();
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection * (moveSpeed + addedRunSpeed * runningState), rb.velocity.y);

        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            jumpCount--;
        }
        isJumping = false;
    }

    private void Animate()
    {
        if (moveDirection > 0 && !facingRight)
        {
            flipCharacter();
        }
        else if (moveDirection < 0 && facingRight)
        {
            flipCharacter();
        }

        animator.SetBool("isOnAir", !isGrounded);
    }

    private void ProcessInput()
    {
        moveDirection = Input.GetAxis("Horizontal"); // scale -1 - 1
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            isJumping = true;
            animator.SetTrigger("Jump");
        }

        runningState = Input.GetAxis("Run");

        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetTrigger("SaysNo");
        }
    }

    private void flipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
