using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSlidingDoorButton : MonoBehaviour
{
    public Material pressedMaterial;
    public Material unpressedMaterial;
    private bool isPressed = false;
    public GameObject walls;
    private Animation slidingAnimation;
    private bool heckedUp = true;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = unpressedMaterial;
        walls = GameObject.FindGameObjectWithTag("SlidingWalls");
        slidingAnimation = walls.GetComponent<Animation>();

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
            if (walls != null && isPressed && heckedUp)
            {
                heckedUp = false;
                slidingAnimation.Play("slidingwalls");
            }
        }
    }
}
