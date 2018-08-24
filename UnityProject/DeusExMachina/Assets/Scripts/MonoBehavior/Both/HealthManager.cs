using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float startHealth;

    [SerializeField] private float maxHealth;
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }

        set
        {
            if (value > 0f)
            {
                maxHealth = value;
            }
        }
    }

    [HideInInspector] private float health;
    public float Health
    {
        get
        {
            return health;
        }
    }

    void Start()
    {
        health = startHealth;

        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    public void Hit(float amount)
    {
        if (amount < 0f)
        {
            return;
        }

        health -= amount;

        health = Mathf.Clamp(health, 0f, maxHealth);

        if (health <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (amount < 0f)
        {
            return;
        }

        health += amount;

        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    public void Kill()
    {
        health = 0f;

        Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
