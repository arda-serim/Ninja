using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this as T;
    }
}
