using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthy : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth = 100;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void HealDamage(float damage)
    {
        currentHealth += damage;
    }
}
