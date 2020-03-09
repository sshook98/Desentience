using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamController : MonoBehaviour
{
    public bool isOnTimer = true;
    public float timeOn = 1f;
    public float timeOff = 1f;
    private float timer = 0;
    private bool isOn = true;
    private GameObject beam;

    // Start is called before the first frame update
    void Start()
    {
        if (beam == null)
        {
            foreach (Transform t in gameObject.transform)
            {
                if (t.tag == "Laserbeam")
                {
                    beam = t.gameObject;
                    break;
                }
            }
        }

        if (beam == null)
        {
            Debug.LogError("Laserbeam is not setup correctly, requires child object tagged as Laserbeam");
        } else
        {
            beam.SetActive(isOn);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (beam != null)
        {
            if (isOnTimer)
            {
                if (isOn)
                {
                    if (timer > timeOn)
                    {
                        activateBeam();
                    }
                }
                else
                {
                    if (timer > timeOff)
                    {
                        activateBeam();
                    }
                }
            }
        }
        
    }

    public void activateBeam()
    {
        if (beam != null)
        {
            isOn = !isOn;
            timer = 0;
            beam.SetActive(isOn);
        }
    }
}
