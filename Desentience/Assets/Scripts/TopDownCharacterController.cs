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
    public float leanIntensity;
    public float leanStep;

    public float timeBetweenShots;
    private float shotTimer = 0;
    private Rigidbody rb;
    private Vector3 aimPosition = Vector3.zero;
    private Animator anim;

    private float oldh;
    private float oldv;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeBetweenShots = 1f / fireRate;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(h, 0, v) * speed;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Quaternion currentRotation = playerModel.rotation;
        float newh = Mathf.Lerp(oldh, h, leanStep);
        float newv = Mathf.Lerp(oldv, v, leanStep);
        Quaternion targetRotation = Quaternion.Euler(newv * leanIntensity, 0f, -newh * leanIntensity);

        if (Physics.Raycast(ray , out hit))
        {
            Vector3 lookPosition = hit.point - transform.position;
            lookPosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            targetRotation *= rotation;
        }

        playerModel.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, Time.deltaTime * 1000);

        shotTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && shotTimer <= 0) // Left Mouse button down,  later switch to a virtual buton "Fire1"
        {
            Fire();
        }

        oldh = newh;
        oldv = newv;

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

        anim.Play("shoot");
    }
}
