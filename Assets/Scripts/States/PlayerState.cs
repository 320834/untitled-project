using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject deathScreen;
    public GameObject endGame;
    public Text killedText;

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.instance.player.GetComponent<PlayerStats>().isAlive())
        {
            //Despawn All enemies here
            Vector3 newPos = new Vector3(555, -353, -361);
            PlayerManager.instance.player.GetComponent<Transform>().position = newPos;

            PlayerManager.instance.player.GetComponent<PlayerInteract>().death = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            gameUI.SetActive(false);
            deathScreen.SetActive(true);

            int killed = GameState.zombiesKilled;
            killedText.text = "You killed " + killed.ToString() + " zombies";
        }
    }

    public void endMenu()
    {
        Vector3 newPos = new Vector3(555, -353, -361);
        PlayerManager.instance.player.GetComponent<Transform>().position = newPos;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameUI.SetActive(false);
        endGame.SetActive(true);
        


    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
