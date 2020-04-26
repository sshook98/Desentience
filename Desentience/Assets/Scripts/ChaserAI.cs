using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserAI : MonoBehaviour
{
    private GameObject target;

    public Transform Robotmodel;

    public float detectionRadius = 15.0f;
    public float explosionActivationRadius = 3.0f;

    public NavMeshAgent agent;

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        try
        {
            Debug.Log("trying to access player from GameMangager");
            target = GameManager.Instance.player;
        }
        catch
        {
            Debug.Log("trying to access player from FindGameObjectWithTag");
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
            {
                target = go;
            }
            else
            {
                Debug.LogError("Could not find a target with tag player");
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
        if ((target.transform.position - gameObject.transform.position).magnitude < detectionRadius)
        {
            Robotmodel.LookAt(target.transform);
            NavMeshHit navHit;
            NavMesh.SamplePosition(target.transform.position, out navHit, 100, -1);
            agent.SetDestination(navHit.position);
        }
        if ((target.transform.position - gameObject.transform.position).magnitude < explosionActivationRadius)
        {
            gameObject.GetComponentInChildren<ChaserRobotScript>().dying = true;
        }
    }

}
