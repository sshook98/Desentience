using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform target;
    public float alertDistance = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        if (target != null)
        {
            if ((target.position - transform.position).magnitude < alertDistance)
            {
                Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
                transform.LookAt(lookPos);
            }
        } else
        {
            Debug.LogError("Turret set up wrong, no target.  Name: " + name);
        }
    }
}
