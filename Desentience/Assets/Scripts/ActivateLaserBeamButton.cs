using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLaserBeamButton : MonoBehaviour
{
    public Material pressedMaterial;
    public Material unpressedMaterial;
    private bool isPressed = false;
    public LaserBeamController[] laserBeams;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = unpressedMaterial;

        if (laserBeams != null)
        {
            for (int i = 0; i < laserBeams.Length; i++)
            {
                laserBeams[i].isOnTimer = false;
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPressed = !isPressed;
            if (isPressed)
            {
                rend.material = pressedMaterial;
            } else
            {
                rend.material = unpressedMaterial;
            }

            if (laserBeams != null)
            {
                for (int i = 0; i < laserBeams.Length; i++)
                {
                    laserBeams[i].activateBeam();
                }
            }
        }
    }
}
