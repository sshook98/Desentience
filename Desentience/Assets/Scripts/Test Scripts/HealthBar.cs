using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Healthy))]
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private float lastHealthValue = -1;
    public Healthy healthComponent;

    private void Awake()
    {
        healthComponent = GetComponent<Healthy>();
    }
    private void Update()
    {
        if (lastHealthValue != healthComponent.currentHealth)
        {
            slider.value = (healthComponent.currentHealth / healthComponent.maxHealth);
        }
        lastHealthValue = healthComponent.currentHealth;
    }

}
