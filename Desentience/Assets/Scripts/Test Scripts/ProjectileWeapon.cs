using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float bulletSpeed;
    public float bulletLifetime;
    public int bulletDamage;
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
        Vector3 velocity = projectile.transform.rotation * Vector3.forward;
        velocity.y = 0;
        velocity = velocity.normalized * bulletSpeed;
        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        projRb.velocity = velocity;
        
        projectile.GetComponent<BooletScript>().destroyDelay = bulletLifetime;

        
        AudioManager.Instance.PlayClipAtPoint(gunshot, projectileSpawnPoint.position, volume: 0.15f);
        // end of Action()
        return true;
    }
}
