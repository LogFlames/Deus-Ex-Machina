using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DimensionSwitcherController2D : MonoBehaviour
{
    [Header("This is a 2D object with 2D colliders")]

    [SerializeField] private float timeUntilWarp;

    [SerializeField] private KeyCode warpKey;

    [SerializeField] private GameObject warpParticlePrefab;

    [SerializeField] private Vector3 particleSpawnOffset;

    private float timeUntilWarpCountdown;

    private BoxCollider2D box_collider;

    private bool playerInside;

    private bool playerWarping;

    private GameObject warpParticle;

    void Start()
    {
        box_collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetKey(warpKey))
        {
            playerInside = box_collider.bounds.Intersects(Player2D.instance.box_collider.bounds);
            
            if (playerInside)
            {
                if (playerWarping)
                {
                    timeUntilWarpCountdown -= Time.deltaTime;

                    if (timeUntilWarpCountdown <= 0f)
                    {
                        Warp();
                    }
                }
                else
                {
                    timeUntilWarpCountdown = timeUntilWarp;

                    warpParticle = Instantiate(warpParticlePrefab);
                    warpParticle.transform.SetParent(transform);
                    warpParticle.transform.localPosition = particleSpawnOffset;

                    playerWarping = true;
                }
            }
        }
        else if (playerWarping)
        {
            Destroy(warpParticle);

            playerWarping = false;
        }

        if (!playerInside)
        {
            Destroy(warpParticle);

            playerWarping = false;
        }
    }

    private void Warp()
    {
        playerWarping = false;
        Destroy(warpParticle);

        GameObjectPool.instance.GetObjectByName("FadeImage").GetComponent<Animator>().Play("Fade");

        GameObjectPool.instance.GetObjectByName("3D").SetActive(true);
        GameObjectPool.instance.GetObjectByName("2D").SetActive(false);
    }
}
