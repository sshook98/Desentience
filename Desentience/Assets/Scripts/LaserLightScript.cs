using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLightScript : MonoBehaviour
{
    public Color lightColor;
    public float flashSpeed;

    private Material thisMaterial;
    private float delay;

    private void Start()
    {
        //adds a little delay before starting light flashing
        delay = Random.Range(0f, 1 / flashSpeed);

        gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

    }

    void Update()
    {
        if (Time.time > delay)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", lightColor * (Mathf.PingPong((Time.time - delay) * flashSpeed, .25f) + .25f));
        }
    }

}
