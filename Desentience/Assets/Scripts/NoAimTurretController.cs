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
    public ParticleSystem explosionEmitter;
    public GameObject shrapnelPrefab;
    public Material[] shrapnelMaterials;
    public Transform turretModel;

    private int currentHealth;
    private bool dying = false;
    private float shotTimer = 0;


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
            if (!explosionEmitter.isPlaying)
            {
                explosionEmitter.Play();
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
        projectile.GetComponent<BooletScript>().destroyDelay = bulletLifetime;
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
                BooletScript hitBulletScript = collision.gameObject.GetComponent<BooletScript>();

                if (!hitBulletScript.hasCollided)
                {
                    hitBulletScript.hasCollided = true;
                    if (currentHealth > 0)
                    {
                        currentHealth -= damageFromBullets;

                        int numShrapnel = Random.Range(minShrapnel, maxShrapnel);
                        for (int i = 0; i < numShrapnel; i++)
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

    private void OnDrawGizmos()
    {
        if (aimTarget != null)
        {
            Gizmos.DrawSphere(aimTarget.position, 0.25f);
        }
    }
}
