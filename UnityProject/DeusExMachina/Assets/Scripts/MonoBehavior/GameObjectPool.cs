using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public static GameObjectPool instance;

    [SerializeField] private bool isMaster;
    [SerializeField] private string nameInPool;

    [SerializeField] private CallInFunction callInFunction;

    public static Dictionary<string, GameObject> gameObjectPool;

    private bool runSetup;

    void Awake()
    {
        if (callInFunction == CallInFunction.awake)
        {
            Setup();
        }
    }

    void OnEnable()
    {
        if (callInFunction == CallInFunction.onEnable)
        {
            Setup();
        }
    }

    void Start()
    {
        if (callInFunction == CallInFunction.start)
        {
            Setup();
        }
    }

    private void Setup()
    {
        if (runSetup)
        {
            return;
        }

        runSetup = true;

        if (isMaster)
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(transform.root.gameObject);
            }
            else
            {
                Debug.LogError("There can't be more than one GameObjectPool Master in the scene at any given time.");
            }
        }

        if (gameObjectPool == null)
        {
            gameObjectPool = new Dictionary<string, GameObject>();
        }

        gameObjectPool.Add(nameInPool, gameObject);
    }

    public GameObject GetObjectByName(string name)
    {
        if (gameObjectPool.ContainsKey(name))
        {
            return gameObjectPool[name];
        }
        else
        {
            return null;
        }
    }

    private enum CallInFunction
    {
        awake,
        start,
        onEnable
    }
}
