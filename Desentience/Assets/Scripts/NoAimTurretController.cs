using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAimTurretController : MonoBehaviour
{
    public bool isAlwaysFiring = true;
    public bool isDestructible = false;
    public float bulletLifetime = 30f;
    public float timeBetweenShots = 1f;
    public float bulletSpeed = 5f;
    public int damageFromBullets = 20;
    public int maxHealth = 10;
    public int minShrapnel = 10;
    public int maxShrapnel = 20;

    public Transform aimTarget;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public ParticleSystem smokeEmitter;
    public GameObject shrapnelPrefab;
    public Material[] shrapnelMaterials;
    public Transform turretModel;

    private int currentHealth;
    private bool dying = false;
    private float shotTimer = 0;

    public GameObject explosionPrefab;
    public int explosionDamage = 40;
    private bool exploded = false;
    private bool explosionGrace = false;

    public int incomingLaserDamage = 4;



    void Start()
    {
        if (aimTarget == null)
        {
            Debug.LogError("No Aim turret configured wrong, needs aimTarget. Name: " + name);
        }

        currentHealth = maxHealth;

    }

    void Update()
    {
        if (currentHealth <= 0 && !dying)
        {
            dying = true;
        }

        if (!dying)
        {
            shotTimer -= Time.deltaTime;

            if (shotTimer <= 0)
            {
                Fire();
            }

            float healthRatio = (float)currentHealth / maxHealth;
            if (healthRatio < 0.5f && smokeEmitter.isStopped)
            {
                smokeEmitter.Play();
            }
            if (healthRatio >= 0.5f && smokeEmitter.isPlaying)
            {
                smokeEmitter.Stop();
            }
        } else
        {
            if (!exploded)
            {
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = gameObject.transform.position;
                explosion.GetComponent<ExplosionScript>().damage = explosionDamage;
                exploded = true;
                Destroy(gameObject, 0.25f);
            }
        }
    }

    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab);
        Vector3 direction = (aimTarget.transform.position - projectileSpawnPoint.position);
        projectile.transform.position = projectileSpawnPoint.position;
        projectile.transform.LookAt(aimTarget.position);
       /* projectile.GetComponent<BooletScript>().destroyDelay = bulletLifetime;*/
        Rigidbody projectileRB =  projectile.GetComponent<Rigidbody>().GetComponent<Rigidbody>();
        projectileRB.velocity = direction.normalized * bulletSpeed;

        shotTimer = timeBetweenShots;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (isDestructible)
            {
                BulletScript bs = collision.collider.GetComponent<BulletScript>();
                if (bs != null)
                {
                    if (bs.damage > 0)
                    {
                        Debug.Log("Turret is taking " + bs.damage + " damage");
                        currentHealth -= (int) bs.damage;
                        SpawnShrapnel();
                    }
                }
            }            
        }
    }

    private void SpawnShrapnel()
    {
        for (int i = 0; i < Random.Range(10, 20); i++)
        {
            GameObject shrapnel = Instantiate(shrapnelPrefab);
            shrapnel.transform.SetParent(transform);
            shrapnel.transform.position = turretModel.position + Random.onUnitSphere;
            shrapnel.transform.localScale = Vector3.Scale(new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)), shrapnelPrefab.transform.localScale) * 2;
            Vector3 velocity = Random.insideUnitSphere * 5;
            Rigidbody projRb = shrapnel.GetComponent<Rigidbody>();
            projRb.velocity = velocity;
            shrapnel.GetComponent<Renderer>().material = shrapnelMaterials[Random.Range(0, shrapnelMaterials.Length)];
            shrapnel.GetComponent<ShrapnelScript>().destroyDelay = Random.Range(1.0f, 3.0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (aimTarget != null)
        {
            Gizmos.DrawSphere(aimTarget.position, 0.25f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDestructible)
        {
            if (other.tag == "Explosion" && explosionGrace == false)
            {
                if (other.GetComponent<ExplosionScript>().damage > 0)
                {
                    explosionGrace = true;
                    StartCoroutine(ExplosionCoroutine());
                    currentHealth -= other.GetComponent<ExplosionScript>().damage;
                    SpawnShrapnel();
                }

            }
            else if (other.tag == "Laserbeam")
            {
                if (incomingLaserDamage > 0)
                {
                    currentHealth -= incomingLaserDamage;
                    SpawnShrapnel();
                }
            }
        }
  
    }

    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(1);

        explosionGrace = false;
    }
}
