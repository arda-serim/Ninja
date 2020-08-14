using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{
    void Update()
    {
        if (transform.position.x < (Camera.main.transform.position + new Vector3(-30,0,10)).x)
        {
            Destroy(gameObject);
        }
    }
}
