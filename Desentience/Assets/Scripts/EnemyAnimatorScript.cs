using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorScript : MonoBehaviour
{
    public Transform robotModel;
    public float maxLeanSpeed = 0.5f;
    public float leanIntensity;
    public float leanStep;

    private Rigidbody rb;

    private Vector3 lastPosition;
    private float oldh;
    private float oldv;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = rb.position;
    }

    private void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 velocity = rb.position - lastPosition;
        velocity.y = 0;

        float h = 0;
        if (velocity.x > 0)
        {
            h = velocity.x > maxLeanSpeed ? -1 : -velocity.x / maxLeanSpeed;
        }
        else if (velocity.x < 0)
        {
            h = Mathf.Abs(velocity.x) > maxLeanSpeed ? 1 : Mathf.Abs(velocity.x) / maxLeanSpeed;
        }
        float v = 0;
        if (velocity.z > 0)
        {
            v = velocity.z > maxLeanSpeed ? -1 : -velocity.z / maxLeanSpeed;
        }
        else if (velocity.z < 0)
        {
            v = Mathf.Abs(velocity.z) > maxLeanSpeed ? 1 : Mathf.Abs(velocity.z) / maxLeanSpeed;
        }

        Quaternion currentRotation = robotModel.rotation;

        float newh = Mathf.Lerp(oldh, h, leanStep);
        float newv = Mathf.Lerp(oldv, v, leanStep);
        Quaternion targetRotation = Quaternion.Euler(newv * leanIntensity, 0f, -newh * leanIntensity);
        robotModel.rotation *= Quaternion.Euler(newv * leanIntensity, 0f, -newh * leanIntensity);

        oldh = newh;
        oldv = newv;

        lastPosition = rb.position;
    }
}
