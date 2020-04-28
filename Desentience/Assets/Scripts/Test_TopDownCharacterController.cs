using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Healthy))]
public class Test_TopDownCharacterController : MonoBehaviour
{
    public float speed;
    public Transform playerModel;
    public Transform projectileSpawnPoint;
    public float fireRate;
    public float bulletSpeed;
    public float bulletLifetime;
    public int laserDamage = 1;
    public float leanIntensity;
    public float leanStep;
    
    private Rigidbody rb;
    private Vector3 aimPosition = Vector3.zero;
    private Animator anim;

    private float oldh;
    private float oldv;

    public ParticleSystem smoke_emitter;
    public ParticleSystem explosion_emitter;
    public Renderer head;
    public Material flashMaterialOff;
    public Material flashMaterialOn;
    public float flashTime = 1;

    private bool healthLow = false;
    private bool healthCritical = false;
    public Healthy healthComponent;

    private float flashCount;
    private bool flashState;
    private bool dying = false;
    // *** ***

    public GameObject shrapnelPrefab;
    public Material[] shrapnelMaterials;

    public AudioClip gunshot;

    public Item selectedItem;
    public Item equippedItem;
    public GameObject itemInstance;

    private void Equip(Item item)
    {
        if (item != equippedItem)
        {
            if (equippedItem != null)
            {
                Unequip();
            }
            equippedItem = item;
            itemInstance = Instantiate(selectedItem.itemPrefab);
            itemInstance.transform.parent = transform;
            ProjectileWeapon pw = itemInstance.GetComponent<ProjectileWeapon>();
            if (pw != null)
            {
                pw.projectileSpawnPoint = projectileSpawnPoint;
            }
        }
    }

    //TODO
    // Deactivate unequipped items that are still in inventory? To avoid creating / destroying unneccessarily
    private void Unequip()
    {
        Destroy(itemInstance);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        healthComponent = GetComponent<Healthy>();
        if (healthComponent == null)
        {
            Debug.Log("No Health script attached");
        }
        if (selectedItem != null)
        {
            Equip(selectedItem);
        }
        else
        {
            Debug.Log("No default weapon detected");
        }

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
        GameManager.Instance.player = gameObject;
    }

    private void FixedUpdate()
    {
        if (healthComponent.currentHealth <= 0 && !dying)
        {
            Material[] matArray = head.materials;
            matArray[0] = flashMaterialOn;
            head.materials = matArray;
            dying = true;
        }
        if (!dying)
        {
            //Need to change input to virtual fire1 button
            if (Input.GetMouseButton(0)) {
                Debug.Log("Trying to fire");
                UseAction();
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            rb.velocity = new Vector3(h, 0, v) * speed;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Quaternion currentRotation = playerModel.rotation;
            float newh = Mathf.Lerp(oldh, h, leanStep);
            float newv = Mathf.Lerp(oldv, v, leanStep);
            oldh = newh;
            oldv = newv;
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

            

            // *** Robot health code ***
            float healthRatio = healthComponent.currentHealth / healthComponent.maxHealth;

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
        }
        else
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
                    GameManager.Instance.PlayerDeath();
                    Destroy(gameObject);
                }
            }
        }

    }

    private void UseAction()
    {
        IActionable actionable = itemInstance.GetComponent<IActionable>() as IActionable;
        if (actionable != null)
        {
            if (actionable.Action())
            {
                anim.Play("shoot");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            SpawnShrapnel();
            BulletScript bs = collision.collider.GetComponent<BulletScript>();
            if (bs != null)
            {
                healthComponent.TakeDamage(bs.damage);
            }
        }
    }

    private void SpawnShrapnel()
    {
        for (int i = 0; i < Random.Range(10, 20); i++)
        {
            GameObject shrapnel = Instantiate(shrapnelPrefab);
            shrapnel.transform.SetParent(transform);
            shrapnel.transform.position = playerModel.position + Random.onUnitSphere;
            shrapnel.transform.localScale = Vector3.Scale(new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)), shrapnelPrefab.transform.localScale) * 2;
            Vector3 velocity = Random.insideUnitSphere * 5;
            Rigidbody projRb = shrapnel.GetComponent<Rigidbody>();
            projRb.velocity = velocity;
            shrapnel.GetComponent<Renderer>().material = shrapnelMaterials[Random.Range(0, shrapnelMaterials.Length)];
            shrapnel.GetComponent<ShrapnelScript>().destroyDelay = Random.Range(1.0f, 3.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KeyCard")
        {
            other.gameObject.SetActive(false);
            GameManager.Instance.CollectKeyCard();
        }
        else if (other.tag == "Elevator")
        {
            if (GameManager.Instance.IsElevatorAvailable())
            {
                GameManager.Instance.HandleLevelComplete();
            }
        }
        else if (other.tag == "HealthPickup")
        {
            if (healthComponent.currentHealth < healthComponent.maxHealth)
            {
                healthComponent.currentHealth += healthComponent.maxHealth / 2;
                if (healthComponent.currentHealth > healthComponent.maxHealth)
                {
                    healthComponent.currentHealth = healthComponent.maxHealth;
                }
                other.gameObject.SetActive(false);
            }
        }
        else if (other.tag == "Laserbeam")
        {
            healthComponent.TakeDamage(laserDamage);
        }
        else if (other.tag == "Explosion")
        {
            ExplosionScript es = other.GetComponent<ExplosionScript>();
            if (es != null)
            {
                healthComponent.TakeDamage(es.damage);
            }
        }
        else
        {
            Debug.Log("Unexpected trigger entry with " + other.name);
        }
    }
}
