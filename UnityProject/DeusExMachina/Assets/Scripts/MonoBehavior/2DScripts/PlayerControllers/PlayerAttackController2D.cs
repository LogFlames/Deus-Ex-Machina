using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController2D : MonoBehaviour
{

    [SerializeField] private KeyCode normalAttackKey;

    [SerializeField] private float timeBetweenAttacks;

    private float countdownBetweenAttacks;

    [SerializeField] private float attackDamage;

    void Start()
    {

    }

    void Update()
    {
        countdownBetweenAttacks -= Time.deltaTime;

        if (Input.GetKeyDown(normalAttackKey) && countdownBetweenAttacks <= 0f)
        {
            //Attack
        }
    }
}
