using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorScript : MonoBehaviour
{
    public float speed;
    public Transform robotModel;
    public float leanIntensity;
    public float maxSpeed;

    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        float h = rb.velocity.x / maxSpeed;
        float v = rb.velocity.z / maxSpeed;

        robotModel.rotation = Quaternion.Euler(v * leanIntensity, 0f, -h * leanIntensity);
    }
}
