using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    [Header("Shield")]
    public float damageTakenFactor;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
}
