using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] List<GameObject> uI;

    void Start()
    {
        Instantiate(uI[0]);
    }

    void Update()
    {
        
    }
}
