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
    private bool isDead = false;

    CircleCollider2D circleCollider;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnDeath()
    {
        if (!isDead)
        {
            isDead = true;
            Destroy(gameObject);
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
        if(Vector2.Distance(this.transform.position, player.transform.position) < 10f && !circleCollider.enabled)
        {
            circleCollider.enabled = true;
        }
        else if (Vector2.Distance(this.transform.position, player.transform.position) > 10f && circleCollider.enabled)
        {
            circleCollider.enabled = false;
        }
    }
}
