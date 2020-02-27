using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour
{
    private GameObject target;
    public bool inCombat = false;
    private Vector3 aimPosition;
    
    public float detectionRadius = 15.0f;

    public Transform playerModel;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float fireRate;
    public float bulletSpeed;
    public float timeBetweenShots;
    private float shotTimer = 0;

    NavMeshAgent agent;

    public float wanderRadius;
    public float wanderTimer;
    private float timer;

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
        agent.isStopped = true;
        target = GameObject.FindWithTag("Player");
        timer = wanderTimer;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!inCombat) 
        {
            inCombat = ((target.transform.position - transform.position).magnitude < detectionRadius) ? true : false;
        }

        if (inCombat) 
        {
            agent.isStopped = false;
            transform.LookAt(target.transform);
            timer += Time.deltaTime;
            if (timer >= wanderTimer) 
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
        
        shotTimer -= Time.deltaTime;
        if (inCombat && shotTimer <= 0)
        {
            Fire();
        }
    }
    private void Fire()
    {
        aimPosition = target.transform.position;
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = projectileSpawnPoint.position;
        Vector3 velocity = (aimPosition - transform.position).normalized * bulletSpeed;
        projectile.transform.LookAt(aimPosition);

        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        projRb.velocity = velocity;

        shotTimer = timeBetweenShots;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) 
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
