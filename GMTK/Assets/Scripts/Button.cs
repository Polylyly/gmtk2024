using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public Canvas pauseCanvas, settingsCanvas;

    private void Start()
    {
        settingsCanvas.enabled = false;
    }

    public void play()
    {
        SceneManager.LoadScene("Level");
    }

    public void Settings()
    {
        pauseCanvas.enabled = false;
        settingsCanvas.enabled = true;
    }

    public void Exit()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Main Menu");
    }
}
