using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    public PlayerController playerController;
    public float fallSpeed;
    private GameObject playerSprite;
    Rigidbody2D rb;
    bool startGame;
    float startDir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSprite = transform.GetChild(0).gameObject;

        transform.position = new Vector2(Random.Range(-25, 25), 20); // Pick a random spawn location.

        // If the player character is on the right side of the screen...
        if (transform.position.x > 0)
        {
            // ... Move the player character in the right direction!
            startDir = 1;
            playerSprite.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            // ... Else, move the player character in the left direction!
            startDir = -1;
            playerSprite.GetComponent<SpriteRenderer>().flipX = false;
        }

        rb.gravityScale = 0.0f; // Fixes jittery effect with constant y-velocity changes.
    }

    private void FixedUpdate()
    {
        // This if-statement moves the player downwards, from the top of the screen, at the start of the game.
        if (!startGame)
        {
            rb.velocity = new Vector2(rb.velocity.x, -1 * fallSpeed);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player character has landed on a platform...
        if (collision.gameObject.layer == 7 && !startGame)
        {
            playerController.canMove = true; // Allow swipe movement.
            rb.gravityScale = 1.0f; // Enable gravity.
            playerController.SetDirection(Vector2.right * startDir); // Start moving player character in starDir's direction.
            GameObject.FindGameObjectWithTag("Music Manager").GetComponent<MusicManager>().startMusic = true; // Start BGM Music.
            startGame = true; // Stops constant y-velocity movement in FixedUpdate().        
        }
    }
}
