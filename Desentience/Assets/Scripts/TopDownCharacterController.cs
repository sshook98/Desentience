using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    public float speed;
    public Transform playerModel;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float fireRate;
    public float bulletSpeed;

    public float timeBetweenShots;
    private float shotTimer = 0;
    private Rigidbody rb;
    private Vector3 aimPosition = Vector3.zero;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeBetweenShots = 1f / fireRate;
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(h, 0, v) * speed;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray , out hit))
        {
            Vector3 lookPosition = hit.point - transform.position;
            lookPosition.y = 0;
            aimPosition = hit.point;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            playerModel.rotation = rotation;
        }

        shotTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && shotTimer <= 0) // Left Mouse button down,  later switch to a virtual buton "Fire1"
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = projectileSpawnPoint.position;
        Vector3 velocity = (aimPosition - transform.position).normalized * bulletSpeed;
        projectile.transform.LookAt(aimPosition);

        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        projRb.velocity = velocity;

        shotTimer = timeBetweenShots;
    }
}
