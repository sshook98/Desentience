using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAI : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    public bool inCombat = false;
    private Vector3 aimPosition;
    public float detectionRadius = 15.0f;

    public Transform playerModel;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float bulletSpeed;
    public float bulletLifetime;
    public float timeBetweenShots;
    private float shotTimer = 0;

    NavMeshAgent agent;

    public float wanderRadius;
    public float wanderTimer;
    private float timer;

    public Healthy healthComponent;

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
        agent.isStopped = true;
        try
        {
            target = GameManager.Instance.player;
        }
        catch
        {
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
        timer = wanderTimer;
    }

    private void Awake()
    {
        healthComponent = GetComponentInChildren<Healthy>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (healthComponent.currentHealth < 0)
        {
            return;
        }
        if (target == null)
        {
            target = GameManager.Instance.player;
            if (target == null)
            {
                return;
            }
        }
        if (!inCombat)
        {
            inCombat = ((target.transform.position - playerModel.position).magnitude < detectionRadius) ? true : false;
        }
        
        if (inCombat) 
        {
            agent.isStopped = false;

            playerModel.LookAt(target.transform);


            timer += Time.deltaTime;
            if (timer >= wanderTimer) 
            {
                Vector3 newPos = RandomNavSphere(playerModel.position, wanderRadius, -1);
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
        Vector3 velocity = (aimPosition - playerModel.position).normalized * bulletSpeed;
        projectile.transform.LookAt(aimPosition);

        BooletScript booletScript = projectile.GetComponent<BooletScript>();
        if (booletScript != null)
        {
            projectile.GetComponent<BooletScript>().destroyDelay = bulletLifetime;
            projectile.GetComponent<BooletScript>().shooter = gameObject;
        }

        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        projRb.velocity = velocity;

        shotTimer = timeBetweenShots;

        Animator anim = GetComponentInChildren<Animator>();
        anim.Play("shoot");
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
