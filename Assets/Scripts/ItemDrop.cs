using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dropType
{
    extraLife,
    cherry100,
    strawberry300,
    orange500
}

public class ItemDrop : MonoBehaviour
{
    public dropType itemDrop;
    public int scoreValue;
    public Sprite[] itemSprites; 
    private SpriteRenderer spriteRenderer;

    // The Start() function handles the random drop selection.
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        int randomNo = Random.Range(0, 20); // Pick a random value.

        // [ Extra Life Drop ]
        // 38%
        if (randomNo < 8 /* && Player has less than maxLives */)
        {
            itemDrop = dropType.extraLife;
            spriteRenderer.sprite = itemSprites[0];

        }
        // [ Fruit Random Drop ]
        // 61%
        else
        { 
            // 33% - 100
            if(randomNo < 15)
            {
                itemDrop = dropType.cherry100;
                scoreValue = 100;
                spriteRenderer.sprite = itemSprites[1];
            }
            // 19% - 300
            else if(randomNo >= 15 && randomNo < 19) 
            {
                itemDrop = dropType.strawberry300;
                scoreValue = 300;
                spriteRenderer.sprite = itemSprites[2];
            }
            // 9% - 500
            else if(randomNo >= 19)
            {
                itemDrop = dropType.orange500;
                scoreValue = 500;
                spriteRenderer.sprite = itemSprites[3];
            }
        }       
    }

    // The OnTriggerEnter2D() function handles the collision and the corresponding score/lives increase.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(itemDrop == dropType.extraLife)
            {
                // Update the player's current lives, if dropType.extraLife is selected.
                // LivesManager.instance.UpdateLives(1);
            }
            else
            {
                // Update the player's current score, by the randomly selected score value.
                ScoreManager.instance.UpdateScore(scoreValue);
            }

            // Destroy GameObject on collision.
            DestroyAnimation();
        }
    }

    private void DestroyAnimation()
    {
        Destroy(this.gameObject);
    }
}
