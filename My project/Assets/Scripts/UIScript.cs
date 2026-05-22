using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public EntityStats playerStats;

    public Image healthBar;
    public Image staminaBar;

    void Update()
    {
        healthBar.fillAmount = playerStats.stats.currentHealth / playerStats.stats.maxHealth;
        staminaBar.fillAmount = playerStats.stats.Stamina / playerStats.stats.maxStamina;
    }
}
