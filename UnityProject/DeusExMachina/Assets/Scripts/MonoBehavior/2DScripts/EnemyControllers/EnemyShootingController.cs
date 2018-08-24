using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingController : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] private float[] bulletDelays;
    [SerializeField] private float shootingDistance;

    [SerializeField] private GameObject bulletPrefab;

    private int bulletDelayIndex;

    private float bulletDelayTimer;

    void Start()
    {
        bulletDelayIndex = 1;
        bulletDelayTimer = bulletDelays[0];
    }

    void Update()
    {
        if (target == null)
        {
            target = Player2D.instance.transform;
            if (!target.gameObject.activeSelf)
            {
                return;
            }
        }

        if (Vector3.Distance(transform.position, target.position) <= shootingDistance)
        {
            bulletDelayTimer -= Time.deltaTime;

            if (bulletDelayTimer <= 0f)
            {
                bulletDelayIndex++;

                if (bulletDelays.Length == 1)
                {
                    bulletDelayIndex = 0;
                }
                else
                {
                    bulletDelayIndex %= bulletDelays.Length;
                }

                bulletDelayTimer = bulletDelays[bulletDelayIndex];

                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                BulletMovementController bulletMovementController = bullet.GetComponent<BulletMovementController>();

                bulletMovementController.targetPosition = target.position;
                bulletMovementController.creator = gameObject;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingDistance);
    }
}
