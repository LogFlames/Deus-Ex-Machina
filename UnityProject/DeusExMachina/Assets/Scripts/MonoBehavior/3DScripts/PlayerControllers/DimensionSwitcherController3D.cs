using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class DimensionSwitcherController3D : MonoBehaviour
{
    [Header("This is a 3D object with 3D colliders")]
    [SerializeField] private float timeUntilWarp;

    [SerializeField] private KeyCode warpKey;

    [SerializeField] private RawImage fadeImage;

    private float timeUntilWarpCountdown;

    private BoxCollider box_collider;

    private bool playerInside;

    private bool playerWarping;

    void Start()
    {
        box_collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (Input.GetKey(warpKey))
        {
            playerInside = box_collider.bounds.Intersects(Player3D.instance.box_collider.bounds);

            if (playerInside)
            {
                if (playerWarping)
                {
                    timeUntilWarpCountdown -= Time.deltaTime;

                    Color fadeImageColor = fadeImage.color;
                    fadeImageColor.a = (1f - (timeUntilWarpCountdown / timeUntilWarp));
                    fadeImage.color = fadeImageColor;

                    if (timeUntilWarpCountdown <= 0f)
                    {
                        Warp();
                    }
                }
                else
                {
                    timeUntilWarpCountdown = timeUntilWarp;

                    Color fadeImageColor = fadeImage.color;
                    fadeImageColor.a = 0f;
                    fadeImage.color = fadeImageColor;

                    playerWarping = true;
                }
            }
        }
        else if (playerWarping)
        {
            Color fadeImageColor = fadeImage.color;
            fadeImageColor.a = 0f;
            fadeImage.color = fadeImageColor;

            playerWarping = false;
        }

        if (!playerInside)
        {
            Color fadeImageColor = fadeImage.color;
            fadeImageColor.a = 0f;
            fadeImage.color = fadeImageColor;

            playerWarping = false;
        }
    }

    private void Warp()
    {
        playerWarping = false;

        Color fadeImageColor = fadeImage.color;
        fadeImageColor.a = 0f;
        fadeImage.color = fadeImageColor;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        GameObjectPool.instance.GetObjectByName("FadeImage").GetComponent<Animator>().Play("Fade");

        GameObjectPool.instance.GetObjectByName("2D").SetActive(true);
        GameObjectPool.instance.GetObjectByName("3D").SetActive(false);
    }
}
