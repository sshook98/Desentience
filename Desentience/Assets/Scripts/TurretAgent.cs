﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Healthy))]
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
    
    public Healthy healthComponent;

    public GameObject shrapnelPrefab;
    public Material[] shrapnelMaterials;
    private Transform turretModel;

    public ParticleSystem smoke_emitter;
    public GameObject explosionPrefab;
    public int explosionDamage = 40;
    private bool exploded = false;
    private bool explosionGrace = false;

    private bool dying;

    private NavMeshAgent agent;
    public int incomingLaserDamage = 4;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        healthComponent = GetComponent<Healthy>();
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
    }

    private void Update()
    {
        if (healthComponent.currentHealth <= 0 && !dying)
        {
            dying = true;
        }

        if (target == null)
        {
            target = GameManager.Instance.player;
            if (target == null)
            {
                return;
            }
        }

        if (!dying)
        {
            inCombat = ((target.transform.position - transform.position).magnitude < detectionRadius) ? true : false;
            if (!inCombat)
            {
                // Move this to just before the if statement? So it stops firing if it's too far away
                // inCombat = ((target.transform.position - transform.position).magnitude < detectionRadius) ? true : false;
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

            float healthRatio = healthComponent.currentHealth / healthComponent.maxHealth;
            if (healthRatio < 0.5f && smoke_emitter.isStopped) smoke_emitter.Play();
            if (healthRatio >= 0.5f && smoke_emitter.isPlaying) smoke_emitter.Stop();

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
        aimPosition = target.transform.position;
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = projectileSpawnPoint.position;
        Vector3 velocity = (aimPosition - transform.position).normalized * bulletSpeed;
        projectile.transform.LookAt(aimPosition);
/*
        BooletScript booletScript = projectile.GetComponent<BooletScript>();
        if (booletScript != null)
        {
            booletScript.destroyDelay = bulletLifetime;
            booletScript.shooter = gameObject;
        }*/
        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        projRb.velocity = velocity;

        shotTimer = timeBetweenShots;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            BulletScript bs = collision.collider.GetComponent<BulletScript>();
            if (bs != null)
            {
                if (bs.damage > 0)
                {
                    Debug.Log("Turret is taking " + bs.damage + " damage");
                    healthComponent.TakeDamage(bs.damage);
                    SpawnShrapnel();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Explosion" && explosionGrace == false)
        {
            if (other.GetComponent<ExplosionScript>().damage > 0)
            {
                explosionGrace = true;
                StartCoroutine(ExplosionCoroutine());
                healthComponent.TakeDamage(other.GetComponent<ExplosionScript>().damage);
                SpawnShrapnel();
            }

        }
        else if (other.tag == "Laserbeam")
        {
            if (incomingLaserDamage > 0)
            {
                healthComponent.TakeDamage(incomingLaserDamage);
                SpawnShrapnel();
            }
        }
    }

    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(1);

        explosionGrace = false;
    }

}