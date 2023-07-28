using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/* Script for Main Menu control */

public class MainMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public AudioMixer audioMixer;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && SceneManager.GetActiveScene().name == "Binah")
        {
            if (GameManager.instance.isTutorial)
            {
                GameManager.instance.isTutorial = false;
                pauseUI.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                resumeGame();
            }
        }
    }

    public void playGame()
    {
        /*
         When "Play" button pressed, load scene "Binah" and play the game.
         */
        SceneManager.LoadScene("Binah");
        Time.timeScale = 1;
    }

    public void quitGame()
    {
        /*
         When "Quit" button pressed, end application.
         */
        Application.Quit();
    }

    public void resumeGame()
    {
        /*
         When "Resume" button pressed, resume game.
         */
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void returnMenu()
    {
        /*
         When "Return to Mainscreen" button pressed, load scene "Menu".
         */
        SceneManager.LoadScene("Menu");
    }

    public void tutorial()
    {
        /*
         When "Tutorial" button pressed, show tutorial.
         */
        GameManager.instance.isTutorial = true;
    }

    public void resetScore()
    {
        /*
         When "ResetScore" button pressed, reset best score.
         */
        PlayerPrefs.SetInt("BestScore", 0);
    }

    public void back()
    {
        /*
         When "Return" button pressed in tutorial, back to paused page.
         */
        GameManager.instance.isTutorial = false;
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
