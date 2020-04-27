using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 5;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Shrapnel")
        {
            return;
        }
        // Not working properly on all enemies
        //Healthy healthComponent = collision.collider.gameObject.GetComponent<Healthy>();
        //if (healthComponent != null)
        //{
        //    healthComponent.TakeDamage(damage);
        //}
        Destroy(gameObject);
    }
}
