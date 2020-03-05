using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamButton : MonoBehaviour
{
    public LaserBeamController laserBeamController;
    public Material pressedMaterial;
    private Material unpressedMaterial;
    private Renderer rend;
    private bool isPressed = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        unpressedMaterial = rend.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            laserBeamController.activateBeam();

            isPressed = !isPressed;
            if (isPressed)
            {
                rend.material = pressedMaterial;
            } else
            {
                rend.material = unpressedMaterial;
            }
        }
    }
}
