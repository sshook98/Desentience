using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private bool elevatorIsAvailable = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateElevator()
    {
        elevatorIsAvailable = true;
    }

    public bool IsElevatorAvailable()
    {
        return elevatorIsAvailable;
    }
}
