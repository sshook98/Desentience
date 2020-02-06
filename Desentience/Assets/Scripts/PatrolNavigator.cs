using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNavigator : MonoBehaviour
{
    public bool patrolsRandomly = false;
    public float closeEnoughDistance = 0.5f;
    public Transform[] patrolPoints;
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

    }

    void Update()
    {
        if ((transform.position - patrolPoints[currentTargetIndex].position).magnitude < closeEnoughDistance)
        {
            if (patrolsRandomly)
            {
                currentTargetIndex = Random.Range(0, patrolPoints.Length);
            } else
            {
                currentTargetIndex++;
                currentTargetIndex %= patrolPoints.Length;
            }
            agent.SetDestination(patrolPoints[currentTargetIndex].position);
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
}
