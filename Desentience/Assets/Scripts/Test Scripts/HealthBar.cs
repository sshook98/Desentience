using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthFillImage;
    private float lastHealthValue = -1;
    public Healthy healthComponent;

    private void Awake()
    {
        healthComponent = GetComponentInParent<Healthy>();
    }
    private void Update()
    {
        if (lastHealthValue != healthComponent.currentHealth)
        {
            healthFillImage.fillAmount = (healthComponent.currentHealth / healthComponent.maxHealth);
        }
        lastHealthValue = healthComponent.currentHealth;
        Debug.Log("lastHealthValue = " + lastHealthValue);
    }

}
