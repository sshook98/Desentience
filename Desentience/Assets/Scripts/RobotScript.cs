using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : MonoBehaviour
{

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

    private Rigidbody rb;
    private Animator anim;

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
                }
            }

        }
    }
}
