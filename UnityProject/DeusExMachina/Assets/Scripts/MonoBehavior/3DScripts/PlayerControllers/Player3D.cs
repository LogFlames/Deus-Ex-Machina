using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : MonoBehaviour {

    public static Player3D instance;

    [HideInInspector] public BoxCollider box_collider;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There can't be more than one player3D in the scene at any given time.");
        }

        box_collider = GetComponent<BoxCollider>();
    }
}
