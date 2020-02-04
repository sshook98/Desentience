using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooletScript : MonoBehaviour
{
    public float destroyDelay = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (destroyDelay > 0) Destroy(gameObject, destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
