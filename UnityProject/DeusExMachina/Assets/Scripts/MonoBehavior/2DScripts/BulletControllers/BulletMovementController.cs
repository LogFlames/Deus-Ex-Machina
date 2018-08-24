using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovementController : MonoBehaviour
{

    [HideInInspector] public Vector3 targetPosition;

    [SerializeField] private float damage;
    [SerializeField] private float movementSpeed;
    [SerializeField] private LayerMask bulletEnvironmentCollisionLayers;
    [SerializeField] private LayerMask entityCollisionLayers;

    [SerializeField] private float timeUntilColliderActive;
    private float timeUntilColliderActiveTimer;

    [HideInInspector] public GameObject creator;

    private BoxCollider2D box_collider;

    private Vector3 moveDirection;

    void Start()
    {
        transform.SetParent(GameObjectPool.instance.GetObjectByName("BulletParent").transform);

        box_collider = GetComponent<BoxCollider2D>();
        box_collider.enabled = false;

        timeUntilColliderActiveTimer = timeUntilColliderActive;

        moveDirection = (targetPosition - transform.position).normalized;
    }

    void Update()
    {
        timeUntilColliderActiveTimer -= Time.deltaTime;
        if (timeUntilColliderActiveTimer <= 0f)
        {
            box_collider.enabled = true;
        }

        float desiredMovementDistance = movementSpeed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, desiredMovementDistance, (bulletEnvironmentCollisionLayers | entityCollisionLayers));

        if (hit.collider == null || hit.collider.gameObject == creator)
        {
            transform.Translate(moveDirection * desiredMovementDistance);
        }
        else
        {
            if (hit.collider.gameObject == Player2D.instance.gameObject)
            {
                PlayerHealth.instance.Hit(damage);
            }
            else
            {
                HealthManager healthManager = hit.collider.GetComponent<HealthManager>();

                if (healthManager != null)
                {
                    healthManager.Hit(damage);
                }
            }

            Destroy(gameObject);
        }
    }
}
