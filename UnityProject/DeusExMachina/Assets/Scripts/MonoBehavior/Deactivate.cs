using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate : MonoBehaviour
{
    [SerializeField] private bool deactivate = true;

    void Start()
    {
        if (deactivate)
        {
            gameObject.SetActive(false);
        }
    }
}
