using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Button playButton;
    public GameObject mainMenuPanel, settingsPanel;

    public void StartGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenSettings() {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings() {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ViewCredits() {
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
