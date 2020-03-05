using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurretAgent : MonoBehaviour
{
    private GameObject target;
    public bool inCombat = false;
    private Vector3 aimPosition;
    
    public float detectionRadius = 20.0f;

    public Transform playerModel;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float fireRate;
    public float bulletSpeed;
    public float timeBetweenShots;
    private float shotTimer = 0;

    NavMeshAgent agent;

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
        agent.isStopped = true;
        target = GameObject.FindWithTag("Player");
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
}