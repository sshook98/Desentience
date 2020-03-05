using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNavigator : MonoBehaviour
{
    public bool patrolsRandomly = false;
    public float closeEnoughDistance = 0.5f;
    public Transform[] patrolPoints;
    public float detectionRadius = 10.0f;
    public float chaseRadius = 15.0f;

    // TODO
    // create singleton player manager that can distribute player "target" to all enemies
    public Transform target;
    public bool isChasing = false;
    private Vector3 chaseOrigin;
    private float distanceChased = 0.0f;
    private bool isReturning = false;

    private int currentTargetIndex;
    NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Patrol navigator does not have NavMeshAgent: " + gameObject.name);
        }

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
            currentTargetIndex = 0;
        } else
        {
            Debug.LogError("Patrol Navigator setup incorrectly");
        }

        target = GameManager.Instance.player.transform;
        if (target == null)
        {
            Debug.LogError("target not initialized");
        }

    }

    void Update()
    {
        // if the target is within detectionRadius from the patrolbot, begin chasing
        if (!isChasing)
        {
            isChasing = ((target.position - transform.position).magnitude < detectionRadius) ? true : false;
            //we have this so we only set the chaseOrigin once per "chase"
            if (isChasing)
            {
                chaseOrigin = transform.position;
            }
        }
        if (isChasing)
        {
            distanceChased = (target.position - chaseOrigin).magnitude;
            if (!isReturning && distanceChased < chaseRadius)
            {
                // do appropriate action: shoot, take cover, etc.
                agent.SetDestination(target.position);
            }
            else
            {
                // return to spot of detection
                isReturning = true;
                agent.SetDestination(chaseOrigin);
                // 
                if ((transform.position - chaseOrigin).magnitude < closeEnoughDistance)
                {
                    isReturning = false;
                    isChasing = false;
                }
            }
        }
        else
        {
            agent.SetDestination(patrolPoints[currentTargetIndex].position);
            if ((transform.position - patrolPoints[currentTargetIndex].position).magnitude < closeEnoughDistance)
            {
                if (patrolsRandomly)
                {
                    currentTargetIndex = Random.Range(0, patrolPoints.Length);
                }
                else
                {
                    currentTargetIndex++;
                    currentTargetIndex %= patrolPoints.Length;
                }
                agent.SetDestination(patrolPoints[currentTargetIndex].position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (patrolPoints != null)
        {
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Gizmos.DrawSphere(patrolPoints[i].position, 0.5f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}