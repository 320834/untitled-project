using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject controlPanel;

    public void PlayGame()
    {
        //SpawnState.removeAllSpawners();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SpawnState.removeAllSpawners();
    }

    public void Quit()
    {
        //Remember to remove when making actual game
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void goToControls()
    {
        mainMenuPanel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void goToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        controlPanel.SetActive(false);
    }

    void start()
    {
        UnityEngine.Debug.Log("Start for menu");
    }

    void update()
    {
        Cursor.visible = true;
    }
}
