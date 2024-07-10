using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Movement
    public float moveSpeed;
    Rigidbody2D rb;
    Vector2 inputDir = Vector2.zero;
    
    // Ladder Movement //
    public float climbSpeed;

    private Vector2 lastDir = Vector2.zero;
    private bool isLadder = false;
    private bool isClimbing = false;

    // Animation
    SpriteRenderer spriteRenderer;
    private Animator animator;
    int velocityHash; // This is the Animator's Velocity parameter.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        velocityHash = Animator.StringToHash("Velocity");
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        if(isLadder && inputDir.y != 0 && !isClimbing)
        {
            isClimbing = true;
            animator.SetBool("isClimbing", true);
            Debug.Log("Start Climbing!");    
            rb.isKinematic = true;
        }

        animator.SetFloat(velocityHash, Mathf.Abs(inputDir.x));
    }
    void FixedUpdate()
    {
        // Moves the player horizontally in inputDir (swipe direction), by a movement speed.
        rb.velocity = new Vector2(inputDir.x * moveSpeed, rb.velocity.y);

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, inputDir.y * climbSpeed);
        }
        else if (!isClimbing) 
        {
            rb.gravityScale = 1f;
        }
    }

    public void SetDirection(Vector2 dir)
    {
        lastDir = inputDir; // Saves the direction the player was last moving in.
        rb.velocity = Vector2.zero;
        inputDir = dir; // Saves the desired direction.

        if(inputDir.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if(inputDir.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;

            if (isClimbing)
            {
                isClimbing = false;
                inputDir = lastDir;
                animator.SetBool("isClimbing", false);
                Debug.Log("Stop Climbing!");
                rb.isKinematic = false;
            }     
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            inputDir = new Vector2(-inputDir.x, inputDir.y);
        }
    }
}
