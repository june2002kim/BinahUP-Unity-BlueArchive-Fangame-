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

    public int score = 0;
    public int windScore = 1;
    public int missileScore = 2;
    public int laserScore = 3;
    private int windTime;

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
                ObstacleManager.instance.missile = true;
            }
            if (score == laserScore)
            {
                ObstacleManager.instance.laser = true;
            }
        }
    }

    public void OnPlayerDead()
    {
        isGameover = true;
        gameoverUI.SetActive(true);

        int bestScore = PlayerPrefs.GetInt("BestScore");

        if(score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);

            bestrecordUI.SetActive(true);
        }

        scoreUI.SetActive(false);
        currentscoreText.text = "Score : " + score;
        bestscoreText.text = "( Best Score : " + bestScore + ")";
    }
}
