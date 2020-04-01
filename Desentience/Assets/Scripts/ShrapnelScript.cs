using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrapnelScript : MonoBehaviour
{
    public float destroyDelay = 10f;

    void Start()
    {
        if (destroyDelay > 0)
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}
