using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyLightScript : MonoBehaviour
{
    public GameObject[] bodyLights;
    public Color lightColor;
    public float flashSpeed;

    private Material thisMaterial;
    private float delay;

    private void Start()
    {
        //adds a little delay before starting light flashing
        delay = Random.Range(0f, 1 / flashSpeed);

        thisMaterial = new Material(Shader.Find("Standard"));
        thisMaterial.EnableKeyword("_EMISSION");
        for (int i = 0; i < bodyLights.Length; i++)
        {
            bodyLights[i].GetComponent<Renderer>().material = thisMaterial;
        }
    }

    void Update()
    {
        if (Time.time > delay)
        {
            thisMaterial.SetColor("_EmissionColor", lightColor * Mathf.PingPong((Time.time - delay) * flashSpeed, 1));
        }
    }

}
