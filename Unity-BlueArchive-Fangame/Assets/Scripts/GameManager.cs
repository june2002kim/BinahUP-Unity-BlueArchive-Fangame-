using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameover = false;
    public Text scoreText;
    public Text currentscoreText;
    public Text bestscoreText;
    public GameObject gameoverUI;
    public GameObject scoreUI;
    public GameObject bestrecordUI;
    public GameObject dialogueUI;

    public int score = 0;
    public int windScore = 10;
    public int missileScore = 15;
    public int laserScore = 25;
    private int windTime;

    private int bestScore;
    private AudioSource gameAudio;
    public AudioClip winClip;
    public AudioClip loseClip;

    public DialogueTrigger dialogueSand;
    public DialogueTrigger dialogueMissile;
    public DialogueTrigger dialogueLaser;

    public GameObject windAlert;
    public GameObject missileAlert;
    public GameObject laserAlert;

    public bool resetScore = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than 2 GameManager exist in Scene!");
            Destroy(gameObject);
        }

        bestScore = PlayerPrefs.GetInt("BestScore");

        if (resetScore)
        {
            PlayerPrefs.SetInt("BestScore", 0);
        }

        gameAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameover && Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            score += newScore;
            scoreText.text = "Score : " + score;

            if (score == windScore)
            {
                if(bestScore < windScore)
                {
                    dialogueUI.SetActive(true);
                    dialogueSand.TriggerDialogue();
                }

                windAlert.SetActive(true);

                ObstacleManager.instance.wind = true;
                windTime = Random.Range(0, 10);
            }
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
        isGameover = true;
        gameoverUI.SetActive(true);

        gameAudio.Stop();

        //int bestScore = PlayerPrefs.GetInt("BestScore");

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

        scoreUI.SetActive(false);
        currentscoreText.text = "Score : " + score;
        bestscoreText.text = "( Best Score : " + bestScore + ")";

        gameAudio.loop = false;
        gameAudio.Play();
    }
}
