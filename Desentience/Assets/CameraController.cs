using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothTime = 1f;
    public float maxSpeed = 50f;
    public Transform desiredPose;

    protected Vector3 currentPositionCorrectionVelocity;
    protected Vector3 currentFacingCorrectionVelocity;

    void LateUpdate()
    {
        if (desiredPose != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredPose.position, ref currentPositionCorrectionVelocity, smoothTime, maxSpeed, Time.deltaTime);
            transform.forward = Vector3.SmoothDamp(transform.forward, desiredPose.forward, ref currentFacingCorrectionVelocity, smoothTime, maxSpeed, Time.deltaTime);
        }
    }
}
