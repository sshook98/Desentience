using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage;
    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        Healthy healthComponent = collider.gameObject.GetComponent<Healthy>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
