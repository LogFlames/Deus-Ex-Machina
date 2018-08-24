using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    [SerializeField] private float startHealth;

    [SerializeField] private TMP_Text healthText;

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

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There can only be one PlayerHealth in the scene at any given time.");
        }
    }

    void Start()
    {
        health = startHealth;

        health = Mathf.Clamp(health, 0f, maxHealth);

        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = "Health: " + health + " / " + maxHealth;
    }

    public void Hit(float amount)
    {
        if (amount < 0f)
        {
            return;
        }

        health -= amount;

        health = Mathf.Clamp(health, 0f, maxHealth);

        UpdateHealthText();

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

        UpdateHealthText();
    }

    public void Kill()
    {
        health = 0f;

        Die();
    }

    private void Die()
    {
        GameObjectPool.instance.GetObjectByName("YouDied").SetActive(true);
    }
}
