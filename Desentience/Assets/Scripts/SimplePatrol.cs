using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int curIndex = 0;
    private float closeEnough = 0.1f;
    public float speed = 1f;
    public bool useLerp = false;
    public float timeBetweenPoints = 1f;

    private float timer = 0;
    private Vector3 lastPosition;

    void Start()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogError("Configured SimplePatrol wrong on " + name + " need to assign patrol points");
        }
        lastPosition = transform.position;
    }

    void Update()
    {
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            timer += Time.deltaTime;
            Vector3 target = patrolPoints[curIndex].position;
            Vector3 direction = (target - transform.position);
            float dist = direction.magnitude;
            direction.Normalize();
            if (useLerp)
            {
                transform.position = Vector3.Lerp(lastPosition, target, timer / timeBetweenPoints);
            } else
            {
                transform.position += direction * speed * Time.deltaTime;
            }

            if (dist < closeEnough)
            {
                lastPosition = patrolPoints[curIndex].position;
                curIndex++;
                curIndex %= patrolPoints.Length;
                timer = 0;
            }

        }
    }
}
