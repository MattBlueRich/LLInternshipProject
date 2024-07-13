using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    [Header("Shield")]
    public float damageTakenFactor;
    [Header("Drop Loot")]
    public GameObject itemDrop;  
    [Header("Audio")]
    public AudioClip holdAttackSFX;
    public AudioClip deathSFX;

    private bool isDead = false;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        itemDrop.SetActive(false);
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnDeath()
    {
        if (!isDead)
        {
            isDead = true;
            itemDrop.SetActive(true);

            audioSource.volume = 0.5f;
            audioSource.clip = deathSFX;
            audioSource.loop = false;
            audioSource.Play();

            Vector2 dir = (player.transform.position - transform.position).normalized;
            itemDrop.GetComponent<Rigidbody2D>().AddForce(dir * 3f, ForceMode2D.Impulse);

            spriteRenderer.gameObject.SetActive(false);
            circleCollider.enabled = false;
            Destroy(this.gameObject, 5f);
        }
    }
    public void DecreaseHealth()
    {       
        if (currentHealth > 0)
        {
            currentHealth -= Time.deltaTime * damageTakenFactor;
        }
        else
        {
            currentHealth = 0;
            OnDeath();
        }
    }
    private void Update()
    {
        // This only allows the player to attack the ghost enemy when in range.
        if(Vector2.Distance(this.transform.position, player.transform.position) < 10f && !circleCollider.enabled && !isDead)
        {
            circleCollider.enabled = true;
        }
        else if (Vector2.Distance(this.transform.position, player.transform.position) > 10f && circleCollider.enabled && !isDead)
        {
            circleCollider.enabled = false;
        }
    }

    public void PlayAttackSound()
    {
        audioSource.volume = 0.15f;
        audioSource.clip = holdAttackSFX;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopAttackSound()
    {
        audioSource.Stop();
    }

}
