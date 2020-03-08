using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorScript : MonoBehaviour
{
    public Transform robotModel;
    public float leanIntensity;
    
    private Vector3 speed;
    private Vector3 lastPos = Vector3.zero;

    private void FixedUpdate()
    {
        speed = (transform.position - lastPos);
        lastPos = transform.position;

        float h = speed.x;
        float v = speed.z;

        robotModel.rotation = Quaternion.Euler(v * leanIntensity, 0f, -h * leanIntensity);
    }
}
