using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Panels
    public GameObject settingsPanel;
    public GameObject statUpgradePanel;

    // Load scene
    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Quit game
    public void quitGame()
    {
        Application.Quit();
    }

    // =========================
    // SETTINGS PANEL
    // =========================

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // =========================
    // STAT UPGRADE PANEL
    // =========================

    public void OpenStatUpgrade()
    {
        statUpgradePanel.SetActive(true);
    }

    public void CloseStatUpgrade()
    {
        statUpgradePanel.SetActive(false);
    }
}