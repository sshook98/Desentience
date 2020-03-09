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
    public float bulletLifetime;
    public float leanIntensity;
    public float leanStep;

    public float timeBetweenShots;
    private float shotTimer = 0;
    private Rigidbody rb;
    private Vector3 aimPosition = Vector3.zero;
    private Animator anim;

    private float oldh;
    private float oldv;

    // *** Robot health variables ***
    public float maxHealth = 100;
    public float currentHealth = 100;

    public ParticleSystem smoke_emitter;
    public ParticleSystem explosion_emitter;
    public Renderer head;
    public Material flashMaterialOff;
    public Material flashMaterialOn;
    public float flashTime = 1;

    private bool healthLow = false;
    private bool healthCritical = false;
    private float flashCount;
    private bool flashState;
    private bool dying = false;
    // *** ***

    public GameObject shrapnelPrefab;
    public Material[] shrapnelMaterials;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeBetweenShots = 1f / fireRate;

        if (smoke_emitter == null)
        {
            Debug.LogError("No smoke emitter connected");
        }
        else
        {
            smoke_emitter.Stop();
        }

        if (explosion_emitter == null)
        {
            Debug.LogError("No explosion emitter connected");
        }
        else
        {
            explosion_emitter.Stop();
        }

        if (head == null)
        {
            Debug.LogError("No head renderer connected");
        }
        else
        {
            flashCount = 0;
            flashState = false;
        }

        if (flashMaterialOff == null)
        {
            Debug.LogError("No flash material (off) connected");
        }

        if (flashMaterialOn == null)
        {
            Debug.LogError("No flash material (on) connected");
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0 && !dying)
        {
            Material[] matArray = head.materials;
            matArray[0] = flashMaterialOn;
            head.materials = matArray;
            dying = true;
        }
        if (!dying)
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

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 lookPosition = hit.point - transform.position;
                lookPosition.y = 0;
                aimPosition = hit.point;
                aimPosition.y = transform.position.y;
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


            // *** Robot health code ***
            float healthRatio = currentHealth / maxHealth;

            if (!healthLow && healthRatio < 0.5f) healthLow = true;
            if (healthLow && healthRatio >= 0.5f) healthLow = false;
            if (!healthCritical && healthRatio < 0.25f) healthCritical = true;
            if (healthCritical && healthRatio >= 0.25f) healthCritical = false;

            if (healthLow && smoke_emitter.isStopped) smoke_emitter.Play();
            if (!healthLow && smoke_emitter.isPlaying) smoke_emitter.Stop();

            if (healthCritical && flashState == false)
            {
                if (flashCount >= flashTime)
                {
                    flashState = true;
                    flashCount = 0;

                    Material[] matArray = head.materials;
                    matArray[0] = flashMaterialOn;
                    head.materials = matArray;
                }
                else
                {
                    flashCount += Time.deltaTime;
                }
            }
            else if (healthCritical && flashState == true)
            {
                if (flashCount >= flashTime)
                {
                    flashState = false;
                    flashCount = 0;

                    Material[] matArray = head.materials;
                    matArray[0] = flashMaterialOff;
                    head.materials = matArray;
                }
                else
                {
                    flashCount += Time.deltaTime;
                }
            }
            else if (!healthCritical)
            {
                flashState = false;
                flashCount = 0;

                Material[] matArray = head.materials;
                matArray[0] = flashMaterialOff;
                head.materials = matArray;
            }
            // *** ***
        } else
        {
            anim.SetBool("dying", true);
            rb.velocity = Vector3.zero;
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f && !explosion_emitter.isPlaying)
                {
                    explosion_emitter.Play();
                }
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    Destroy(gameObject);
                }
            }
        }

    }

    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = projectileSpawnPoint.position;
        Vector3 velocity = (aimPosition - transform.position).normalized * bulletSpeed;
        projectile.transform.LookAt(aimPosition);

        projectile.GetComponent<BooletScript>().destroyDelay = bulletLifetime;

        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        projRb.velocity = velocity;

        shotTimer = timeBetweenShots;

        anim.Play("shoot");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            BooletScript hitBooletScript = collision.collider.GetComponent<BooletScript>();

            if (!hitBooletScript.hasCollided)
            {
                hitBooletScript.hasCollided = true;
                if (currentHealth > 0)
                {
                    currentHealth -= 20;

                    //spawn shrapnel
                    for (int i = 0; i < Random.Range(10, 20); i++)
                    {
                        GameObject shrapnel = Instantiate(shrapnelPrefab);
                        shrapnel.transform.position = playerModel.position + Random.onUnitSphere;
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

    private void OnTriggerEnter(Collider collider)
    {
        if (other.tag == "KeyCard")
        {
            other.gameObject.SetActive(false);
            GameManager.instance.CollectKeyCard();
        }
        else if (other.tag == "Elevator")
        {
            if (GameManager.instance.IsElevatorAvailable())
            {
                GameManager.instance.HandleLevelComplete();
            }
        }
        else if (other.gameObject.tag == "HealthPickup")
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += maxHealth / 2;
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
                other.gameObject.SetActive(false);
            }
        }
    }
}
