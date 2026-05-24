using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    public GameObject deathUi;
    public EntityStats playerStats;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        if(playerStats.stats.currentHealth <= 0)
        {
            deathUi.SetActive(true);
        }
    }
}
