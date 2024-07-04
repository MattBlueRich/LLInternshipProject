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
    Vector2 velocitySmoothing = Vector2.zero;

    // Ladder Movement //
    public float climbSpeed;
    public Collider2D platformCollision;

    private Vector2 lastDir = Vector2.zero;
    private bool isLadder = false;
    private bool isClimbing = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(isLadder && inputDir.y != 0)
        {
            isClimbing = true;
        }
    }
    void FixedUpdate()
    {
        // Moves the player horizontally in inputDir (swipe direction), by a movement speed.
        rb.velocity = new Vector2(inputDir.x * moveSpeed, rb.velocity.y);

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, inputDir.y * moveSpeed);
        }
        else 
        {
            rb.gravityScale = 1f;
        }
    }

    public void SetDirection(Vector2 dir)
    {
        lastDir = inputDir; // Saves the direction the player was last moving in.
        rb.velocity = Vector2.zero;
        inputDir = dir; // Saves the desired direction.
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
            }        
        }
    }
}
