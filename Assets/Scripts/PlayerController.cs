using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Movement
    public float moveSpeed;
    private float minMoveSpeed;
    Rigidbody2D rb;
    Vector2 inputDir = Vector2.zero;
    private bool boost = false;

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
        minMoveSpeed = moveSpeed;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        velocityHash = Animator.StringToHash("Velocity");
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        // This if-statement switches the player character from moving to climbing, when in proximity of ladder while swiping vertically.
        if(isLadder && inputDir.y != 0 && !isClimbing)
        {
            isClimbing = true;
            animator.SetBool("isClimbing", true);
            Debug.Log("Start Climbing!");    
            rb.isKinematic = true;
        }

        animator.SetFloat(velocityHash, Mathf.Abs(inputDir.x)); // This switches from idle to moving animations, depending on inputDir.x.

        // Enabled in SetDirection(), this if-statement applies a small speed boost when swiping in a direction.
        if (boost)
        {            
            if(moveSpeed > minMoveSpeed)
            {
                moveSpeed -= Time.deltaTime * 10f;
            }
            else
            {
                moveSpeed = minMoveSpeed;
                boost = false;
            }
        }
    }
    void FixedUpdate()
    {
        // Moves the player horizontally in inputDir (swipe direction), by a movement speed.
        rb.velocity = new Vector2(inputDir.x * moveSpeed, rb.velocity.y);

        // This if-statement moves the player's y-velocity while climbing.
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

        // These variables enable a short boost of speed when swiping in a direction.
        moveSpeed = 12f;
        boost = true;
        
        // This if-statement flips the sprite according to the desired swipe direction.
        if(inputDir.x > 0)
        {
            spriteRenderer.flipX = true;
            animator.SetTrigger("RollRight");
        }
        else if(inputDir.x < 0)
        {
            spriteRenderer.flipX = false;
            animator.SetTrigger("RollLeft");
        }   
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // This if-statement checks when the player character is by a ladder.
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // This if-statement checks when the player character has exited the ladder.
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
        // This if-statement moves the player character in the opposite direction, when colliding with a wall.
        if (collision.gameObject.CompareTag("Walls"))
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            inputDir = new Vector2(-inputDir.x, inputDir.y);
        }
    }
}
