using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : MonoBehaviour
{

    public float maxHealth = 100;
    public float currentHealth = 100;

    public ParticleSystem smoke_emitter;
    public GameObject explosionPrefab;
    public int explosionDamage = 40;
    public Renderer head;
    public Material flashMaterialOff;
    public Material flashMaterialOn;
    public float flashTime = 1;

    private bool healthLow = false;
    private bool healthCritical = false;
    private float flashCount;
    private bool flashState;
    public bool dying = false;

    private Rigidbody rb;
    private Animator anim;

    public GameObject shrapnelPrefab;
    public Material[] shrapnelMaterials;
    public Transform robotModel;

    private bool exploded = false;
    private bool explosionGrace = false;

    public int incomingBulletDamage = 20;
    public int incomingLaserDamage = 4;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (smoke_emitter == null)
        {
            Debug.LogError("No smoke emitter connected");
        } else
        {
            smoke_emitter.Stop();
        }


        if (head == null)
        {
            Debug.LogError("No head renderer connected");
        } else
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

    void Update()
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
        } else
        {
            anim.SetBool("dying", true);
            rb.velocity = Vector3.zero;
            if (gameObject.GetComponentInParent<TestAI>())
            {
                gameObject.GetComponentInParent<TestAI>().wanderRadius = 0f;
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                if (exploded == false && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    GameObject explosion = Instantiate(explosionPrefab);
                    explosion.transform.position = gameObject.transform.position;
                    explosion.GetComponent<ExplosionScript>().damage = explosionDamage;
                    exploded = true;

                }
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (damage > 0)
        {
            //spawn shrapnel
            for (int i = 0; i < Random.Range(10, 20); i++)
            {
                GameObject shrapnel = Instantiate(shrapnelPrefab);
                shrapnel.transform.position = robotModel.position + Random.onUnitSphere;
                shrapnel.transform.localScale = Vector3.Scale(new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)), shrapnelPrefab.transform.localScale) * 2;
                Vector3 velocity = Random.insideUnitSphere * 5;
                Rigidbody projRb = shrapnel.GetComponent<Rigidbody>();
                projRb.velocity = velocity;
                shrapnel.GetComponent<Renderer>().material = shrapnelMaterials[Random.Range(0, shrapnelMaterials.Length)];
                shrapnel.GetComponent<ShrapnelScript>().destroyDelay = Random.Range(1.0f, 3.0f);
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            BooletScript hitBooletScript = collision.collider.GetComponent<BooletScript>();

            if (!hitBooletScript.hasCollided && hitBooletScript.shooter != gameObject.transform.parent.gameObject)
            {
                hitBooletScript.hasCollided = true;
                if (currentHealth > 0)
                {
                    TakeDamage(incomingBulletDamage);
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Explosion" && explosionGrace == false)
        {
            explosionGrace = true;
            StartCoroutine(ExplosionCoroutine());

            TakeDamage(other.GetComponent<ExplosionScript>().damage);
        }
        else if (other.tag == "Laserbeam")
        {
            TakeDamage(incomingLaserDamage);
        }
    }

    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(1);

        explosionGrace = false;
    }
}
