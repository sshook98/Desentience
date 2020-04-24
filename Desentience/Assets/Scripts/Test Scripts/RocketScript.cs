using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    public float damage;
    public float radius;
    public float force;
    public bool exploding = false;
    public GameObject explosionEffect;

    private void Start()
    {
        
    }

    private void Update()
    {
        // Rocket trail effect. Probably using Unity Particle Pack
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        if (exploding == true)
        {
            return;
        }
        exploding = true;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
            Healthy healthComponent = collider.gameObject.GetComponent<Healthy>();
            if (healthComponent != null)
            {
                float distance = (collider.transform.position - transform.position).magnitude;
                float scaledDamage = damage * (1 - (distance / radius));
                healthComponent.TakeDamage(scaledDamage);
            }
        }
        Destroy(gameObject);
    }
}
