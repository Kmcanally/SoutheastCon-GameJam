using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Button playButton;
    public GameObject settings;

    public void StartGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenSettings() {

    }

    public void CloseSettings() {
        
    }

    public void ViewCredits() {
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame() {
        Application.Quit();
    }
}
