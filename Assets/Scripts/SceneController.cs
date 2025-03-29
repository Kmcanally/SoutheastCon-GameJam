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

    public void SetDifficultyEasy() {
        PlayerPrefs.SetString("Difficulty", "Easy");
    }

    public void SetDifficultyMedium() {
        PlayerPrefs.SetString("Difficulty", "Medium");
    }

    public void SetDifficultyHard() {
        PlayerPrefs.SetString("Difficulty", "Hard");
    }

    public void CloseSettings() {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        Debug.Log(PlayerPrefs.GetString("Difficulty"));
    }

    public void ViewCredits() {
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
