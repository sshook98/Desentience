using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float bulletSpeed;

    //Don't want this
    public float bulletLifetime;
    
    public AudioClip gunshot;
    public float timeOfLastShot;
    public float fireRate;

    public override void Update()
    {
        
    }

    public override bool Action()
    {
        if ((Time.time - timeOfLastShot) < (1 / fireRate)) {
            return false;
        }
        timeOfLastShot = Time.time;
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = projectileSpawnPoint.position;
        projectile.transform.rotation = projectileSpawnPoint.rotation;
        Vector3 forward_vector = projectile.transform.rotation * Vector3.forward;
        forward_vector.y = 0;
        Vector3 velocity = forward_vector.normalized * bulletSpeed;
        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        projRb.velocity = velocity;
        
        //Don't want this at all
        projectile.GetComponent<BooletScript>().destroyDelay = bulletLifetime;

        AudioManager.Instance.PlayClipAtPoint(gunshot, projectileSpawnPoint.position, volume: 0.15f);
        // end of Action()
        return true;
    }
}
