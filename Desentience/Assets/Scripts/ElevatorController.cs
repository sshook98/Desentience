using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.elevator = this;
    }

    private bool elevatorActivated = false;

    public bool isElevatorActivated()
    {
        return elevatorActivated;
    }

    public void activateElevator()
    {
        elevatorActivated = true;
    }
}
