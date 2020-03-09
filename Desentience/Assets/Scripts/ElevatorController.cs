using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private bool elevatorActivated = false;

    public bool isElevatorActivated()
    {
        return elevatorActivated;
    }

    public void actiateElevator()
    {
        elevatorActivated = true;
    }
}
