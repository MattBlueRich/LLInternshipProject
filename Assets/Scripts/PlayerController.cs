using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    [HideInInspector] public bool canMove = false; 

    // Ladder Movement //
    public float climbSpeed;

    private Vector2 lastDir = Vector2.zero;
    private bool isLadder = false;
    private bool isClimbing = false;

    // Animation //
    SpriteRenderer spriteRenderer;
    private Animator animator;
    int velocityHash; // This is the Animator's Velocity parameter.

    // Audio //
    [Header("Audio")]
    public AudioClip rollSFX;
    public AudioClip ladderStepSFX;
    private AudioSource audioSource;

    void Start()
    {
        // GameObject components:
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // GameObject Child components:
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();   
        animator = transform.GetChild(0).GetComponent<Animator>();

        velocityHash = Animator.StringToHash("Velocity"); // References the animator's idle - moving blend tree value.

        minMoveSpeed = moveSpeed; // Set default move speed value.
    }

    private void Update()
    {
        // This if-statement switches the player character from moving to climbing, when in proximity of ladder while swiping vertically.
        if(isLadder && inputDir.y != 0 && !isClimbing)
        {
            isClimbing = true; // Allow constant y-velocity movement.
            animator.SetBool("isClimbing", true); // Play climbing animation.   
            rb.isKinematic = true; // Disable collisions with RigidBody2D (allows for passing through platforms!).
            InvokeRepeating("PlayLadderSFX", 0f, 0.3f); // Play a looping ladder step sound effect.
        }

        animator.SetFloat(velocityHash, Mathf.Abs(inputDir.x)); // This switches from idle to moving animations, depending on inputDir.x.

        // Enabled in SetDirection(), this if-statement applies a small speed boost when swiping in a direction.
        if (boost)
        {
            // This if-statement acts as a timer, ticking moveSpeed back to minMoveSpeed by Time.deltaTime.
            if(moveSpeed > minMoveSpeed)
            {
                moveSpeed -= Time.deltaTime * 10f;
            }
            else
            {
                moveSpeed = minMoveSpeed; // Set moveSpeed to default.
                boost = false; // End this if-statement.
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
            rb.gravityScale = 0f; // Disable gravity.
            rb.velocity = new Vector2(rb.velocity.x, inputDir.y * climbSpeed); // Apply y-velocity.
        }
        else if (!isClimbing) 
        {
            rb.gravityScale = 1f; // Enable gravity.
        }
    }

    // This function takes the direction, from the Swipe Controller's Lean Finger Swipe (LFS) Components.
    // The LFS gets the direction and outputs it to SetDirection() from the On Delta Unity Event.
    public void SetDirection(Vector2 dir)
    {
        // canMove is controlled by PlayerStart.cs, only allowing movement once the player character is grounded upon spawning.
        if (!canMove)
        {
            return;
        }

        lastDir = inputDir; // Saves the direction the player was last moving in.
        rb.velocity = Vector2.zero;
        inputDir = dir; // Saves the desired direction.

        // These variables enable a short boost of speed when swiping in a direction.
        moveSpeed = 12f;
        boost = true;

        // This plays a roll sound effect, at a random pitch.
        audioSource.volume = 0.5f;
        audioSource.pitch = UnityEngine.Random.Range(0.7f, 1f);
        audioSource.clip = rollSFX;
        audioSource.Play();

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
            isLadder = true; // The player character is in contact with the ladder.
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // This if-statement checks when the player character has exited the ladder.
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false; // The player character is no longer in contact with the ladder.

            // If the player character is still climbing while outside of the ladder...
            if (isClimbing)
            {
                isClimbing = false; // Disable constant y-velocity movement.

                // Start moving the player character in the last direction they were moving, before entering the ladder.
                inputDir = lastDir; 


                animator.SetBool("isClimbing", false); // Stop the climbing animation.
                rb.isKinematic = false; // Re-enable collisions with RigidBody2D.

                // This stops all the ladder climbing sound effects. 
                CancelInvoke(); 
                audioSource.Stop();
            }     
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // This if-statement moves the player character in the opposite direction, when colliding with a wall.
        if (collision.gameObject.CompareTag("Walls"))
        {
            spriteRenderer.flipX = !spriteRenderer.flipX; // Flip the player character sprite in the opposite direction.
            inputDir = new Vector2(-inputDir.x, inputDir.y); // Flip the player character's movement in the opposite direction.

            // This plays a roll sound effect, at a random pitch.
            audioSource.volume = 0.5f;
            audioSource.pitch = UnityEngine.Random.Range(0.7f, 1f);
            audioSource.clip = rollSFX;
            audioSource.Play();
        }
    }

    // This function is repeated every fixed interval by InvokeRepeating(), to play consecutive ladder stepping sounds.
    public void PlayLadderSFX()
    {
        // This plays a random ladder step sound effect, at a random pitch.
        audioSource.volume = 0.3f;
        audioSource.pitch = UnityEngine.Random.Range(0.7f, 1f);
        audioSource.clip = ladderStepSFX;
        audioSource.Play();
    }
}
