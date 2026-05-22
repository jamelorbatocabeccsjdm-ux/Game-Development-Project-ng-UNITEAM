using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stats stats;

    public void TakeDamage(float damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void ConsumeStamina(float amount)
    {
        stats.Stamina -= amount;
        if (stats.Stamina < 0) stats.Stamina = 0;
    }

    void Update()
    {
        RegenerateHealth(stats.healthRegenRate );
        RegenerateStamina(stats.staminaRegenRate);
    }

    void RegenerateStamina(float amount)
    {
        stats.Stamina += amount * Time.deltaTime;
        if (stats.Stamina > stats.maxStamina) stats.Stamina = stats.maxStamina;
    }
    void RegenerateHealth(float amount)
    {
        stats.currentHealth += amount * Time.deltaTime;
        if (stats.currentHealth > stats.maxHealth) stats.currentHealth = stats.maxHealth;
    }
    
}

[System.Serializable]
public class Stats
{
    public float maxHealth;
    public float currentHealth;
    public float healthRegenRate;
    public float attackPower;
    public float Stamina;
    public float maxStamina;
    public float staminaRegenRate;
}
