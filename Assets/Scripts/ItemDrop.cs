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
    private Object particleRef;
    private GameObject colliders;

    public AudioClip fruitSFX;
    public AudioClip extraLifeSFX;
    private AudioSource audioSource;

    // The Start() function handles the random drop selection.
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particleRef = Resources.Load("PickupParticle");
        colliders = transform.GetChild(0).gameObject;
        audioSource = GetComponent<AudioSource>();

        int randomNo = Random.Range(0, 20); // Pick a random value.

        // [ Extra Life Drop ]
        // 38%
        if (randomNo < 8 && PlayerLives.maxLives < 3)
        {
            itemDrop = dropType.extraLife;
            spriteRenderer.sprite = itemSprites[0];
            audioSource.clip = extraLifeSFX;
        }
        // [ Fruit Random Drop ]
        // 61%
        else
        {
            audioSource.clip = fruitSFX;

            // 33% - 100
            if (randomNo < 15)
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
                PlayerLives.maxLives++;
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
        // Disables Pick-Up.
        colliders.SetActive(false);
        spriteRenderer.enabled = false;

        // Particle Animation.
        GameObject particleEffect = (GameObject)Instantiate(particleRef);
        particleEffect.transform.position = transform.position;
        Destroy(particleEffect, 1f);

        // Play Sound Effect.
        audioSource.Play();

        // Destroys Pick-Up.
        Destroy(this.gameObject, 2f);
    }
}
