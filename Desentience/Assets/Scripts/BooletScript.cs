using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooletScript : MonoBehaviour
{
    public float destroyDelay = 10f;
    public GameObject shooter;

    public bool hasCollided = false;
    private bool setDestroy = false;

    void Update()
    {
        if (!setDestroy)
        {
            if (destroyDelay > 0)
            {
                Destroy(gameObject, destroyDelay);
                setDestroy = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
