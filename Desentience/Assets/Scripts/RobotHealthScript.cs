using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHealthScript : MonoBehaviour
{

    public float maxHealth = 100;
    public float currentHealth = 100;

    public ParticleSystem smoke_emitter;
    public Renderer head;
    public Material flashMaterialOff;
    public Material flashMaterialOn;
    public float flashTime = 1;

    private bool healthLow = false;
    private bool healthCritical = false;
    private float flashCount;
    private bool flashState;

    void Start()
    {
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

    void Update()
    {
        float healthRatio = currentHealth / maxHealth;

        if (!healthLow && healthRatio < 0.5f) healthLow = true;
        if (healthLow && healthRatio >= 0.5f) healthLow = false;
        if (!healthCritical && healthRatio < 0.25f) healthCritical = true;
        if (healthCritical && healthRatio >= 0.25f) healthCritical = false;

        if (healthLow && smoke_emitter.isStopped) smoke_emitter.Play();
        if (!healthLow && smoke_emitter.isPlaying) smoke_emitter.Stop();
        
        if (healthCritical && flashState == false) { 
            if (flashCount >= flashTime)
            {
                flashState = true;
                flashCount = 0;

                Material[] matArray = head.materials;
                matArray[0] = flashMaterialOn;
                head.materials = matArray;
            } else
            {
                flashCount += Time.deltaTime;
            }
        } else if (healthCritical && flashState == true)
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

    }
}
