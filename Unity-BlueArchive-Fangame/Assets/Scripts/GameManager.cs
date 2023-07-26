using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Script for Game Management */

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameover = false;
    public bool isTutorial = false;

    public Text scoreText;              // Display score while playing game
    public Text currentscoreText;       // Display current score when game is over
    public Text bestscoreText;          // Display best score when game is over
    public GameObject gameoverUI;
    public GameObject scoreUI;          // Score UI that show score while playing game
    public GameObject bestrecordUI;
    public GameObject dialogueUI;
    public GameObject pauseUI;

    public int score = 0;               // Current score
    private int bestScore;              // Best score

    // Obstacle triggering score
    public int windScore = 10;
    public int missileScore = 15;
    public int laserScore = 25;
    private int windTime;

    // Audio Clips
    private AudioSource gameAudio;
    public AudioClip winClip;
    public AudioClip loseClip;

    // Tutorial dialogues for obstacles
    public DialogueTrigger dialogueSand;
    public DialogueTrigger dialogueMissile;
    public DialogueTrigger dialogueLaser;

    // Binah sprites for notifing obstacles after tutorial
    public GameObject windAlert;
    public GameObject missileAlert;
    public GameObject laserAlert;

    public bool resetScore = false;

    private void Awake()
    {
        /*
         Singleton
         */

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than 2 GameManager exist in Scene!");
            Destroy(gameObject);
        }

        // Get player's best score by PlayerPrefs to consider to show tutorials
        bestScore = PlayerPrefs.GetInt("BestScore");

        // Reset player's best score for test
        if (resetScore)
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }

        gameAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         When game is over, Can restart by pushing "Jump" button again.
        Turn on Menu panel by pusing "Cancel" button
         */

        if(isGameover && Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
        }
    }

    public void AddScore(int newScore)
    {
        /*
         If player steps on platform, add score.
        and considering current score, add obstacles by ObstacleManager's trigger variables
         */

        if (!isGameover)
        {
            score += newScore;
            scoreText.text = "Score : " + score;

            if (score == windScore)
            {
                if(bestScore < windScore)
                {
                    // If player didn't experience wind obstacle, show tutorial dialogue
                    dialogueUI.SetActive(true);
                    dialogueSand.TriggerDialogue();
                }

                windAlert.SetActive(true);

                ObstacleManager.instance.wind = true;
                windTime = Random.Range(0, 10);
            }
            // Change wind's direction randomly
            if (score >= windScore)
            {
                if (score % 10 == windTime)
                {
                    ObstacleManager.instance.eastWind = true;
                    windTime = Random.Range(0, 10);
                }
                else
                {
                    ObstacleManager.instance.eastWind = false;
                }
            }
            if (score == missileScore)
            {
                if (bestScore < missileScore)
                {
                    // If player didn't experience missile obstacle, show tutorial dialogue
                    dialogueUI.SetActive(true);
                    dialogueMissile.TriggerDialogue();
                }

                missileAlert.SetActive(true);

                ObstacleManager.instance.missile = true;
            }
            if (score == laserScore)
            {
                if (bestScore < laserScore)
                {
                    // If player didn't experience laser obstacle, show tutorial dialogue
                    dialogueUI.SetActive(true);
                    dialogueLaser.TriggerDialogue();
                }

                laserAlert.SetActive(true);

                ObstacleManager.instance.laser = true;
            }
        }
    }

    public void OnPlayerDead()
    {
        /*
         When player is dead, stop BackGroundMusic
        and update BestScore depending on score
        and show game's result UI and play audioClip depending on result
         */

        isGameover = true;
        gameoverUI.SetActive(true);

        gameAudio.Stop();

        // Get player's best score by PlayerPrefs again considering reset score button has pushed
        bestScore = PlayerPrefs.GetInt("BestScore");

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);

            bestrecordUI.SetActive(true);
            gameAudio.clip = winClip;
        }
        else
        {
            gameAudio.clip = loseClip;
        }

        // Turn off Left above's scoreUI
        scoreUI.SetActive(false);
        currentscoreText.text = "Score : " + score;
        bestscoreText.text = "( Best Score : " + bestScore + ")";

        gameAudio.loop = false;
        gameAudio.Play();
    }
}
