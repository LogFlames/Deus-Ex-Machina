using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController2D : MonoBehaviour
{

    [SerializeField] private float timeBetweenAttacks;

    private float countdownBetweenAttacks;

    [SerializeField] private float attackDamage;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float attackDistance;

    void Start()
    {

    }

    void Update()
    {
        countdownBetweenAttacks -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && countdownBetweenAttacks <= 0f)
        {
            //Attack
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Player2D.instance.xDirection * Vector3.right, attackDistance, enemyLayer);

            if (hit.collider != null)
            {
                hit.collider.GetComponent<HealthManager>().Hit(attackDamage);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
