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
    public float bulletLifetime;
    private float shotTimer = 0;

    public float maxHealth = 100;
    public float currentHealth = 100;

    public GameObject shrapnelPrefab;
    public Material[] shrapnelMaterials;
    private Transform turretModel;

    public ParticleSystem smoke_emitter;
    public ParticleSystem explosion_emitter;

    private bool dying;

    NavMeshAgent agent;

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
        turretModel = gameObject.transform;

        if (smoke_emitter == null)
        {
            Debug.LogError("No smoke emitter connected");
        }
        else
        {
            smoke_emitter.Stop();
        }

        if (explosion_emitter == null)
        {
            Debug.LogError("No explosion emitter connected");
        }
        else
        {
            explosion_emitter.Stop();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentHealth <= 0 && !dying)
        {
            dying = true;
        }

        if (!dying)
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

            float healthRatio = currentHealth / maxHealth;
            if (healthRatio < 0.5f && smoke_emitter.isStopped) smoke_emitter.Play();
            if (healthRatio >= 0.5f && smoke_emitter.isPlaying) smoke_emitter.Stop();

        } else
        {
            if (!explosion_emitter.isPlaying)
            {
                explosion_emitter.Play();
                Destroy(gameObject, 0.25f);
            }
        }
        
    }
    private void Fire()
    {
        aimPosition = target.transform.position;
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = projectileSpawnPoint.position;
        Vector3 velocity = (aimPosition - transform.position).normalized * bulletSpeed;
        projectile.transform.LookAt(aimPosition);

        projectile.GetComponent<BooletScript>().destroyDelay = bulletLifetime;

        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        projRb.velocity = velocity;

        shotTimer = timeBetweenShots;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            BooletScript hitBooletScript = collision.collider.GetComponent<BooletScript>();

            if (!hitBooletScript.hasCollided)
            {
                hitBooletScript.hasCollided = true;
                if (currentHealth > 0)
                {
                    currentHealth -= 20;

                    for (int i = 0; i < Random.Range(10, 20); i++)
                    {
                        GameObject shrapnel = Instantiate(shrapnelPrefab);
                        shrapnel.transform.position = turretModel.position + Random.onUnitSphere;
                        shrapnel.transform.localScale = Vector3.Scale(new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)), shrapnelPrefab.transform.localScale) * 2;
                        Vector3 velocity = Random.insideUnitSphere * 5;
                        Rigidbody projRb = shrapnel.GetComponent<Rigidbody>();
                        projRb.velocity = velocity;
                        shrapnel.GetComponent<Renderer>().material = shrapnelMaterials[Random.Range(0, shrapnelMaterials.Length)];
                        shrapnel.GetComponent<ShrapnelScript>().destroyDelay = Random.Range(1.0f, 3.0f);
                    }
                }
            }

        }
    }
}