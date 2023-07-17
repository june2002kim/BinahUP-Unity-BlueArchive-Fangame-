using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject shortplatformPrefab;
    public GameObject platformPrefab;
    public GameObject longplatformPrefab;
    public int count = 15;

    public float timeBetSpawnMin = 0.01f;
    public float timeBetSpawnMax = 1.15f;
    private float timeBetSpawn;

    public float xMin = -3f;
    public float xMax = 3f;
    public float yMin = 0.95f;
    public float yMax = 1.2f;
    private float lastSpawnX = 0.64f;
    private float lastSpawnY = 0f;

    private GameObject[] platforms;
    private int currentIndex = 0;

    private Vector2 poolPosition = new Vector2(-25, 0);
    private float lastSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        platforms = new GameObject[count];

        for(int i = 0; i < count; i++)
        {
            if(Random.Range(0,3) == 0)
            {
                platforms[i] = Instantiate(shortplatformPrefab, poolPosition, Quaternion.identity);
            }
            else if (Random.Range(0, 3) == 1)
            {
                platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.identity);
            }
            else
            {
                platforms[i] = Instantiate(longplatformPrefab, poolPosition, Quaternion.identity);
            }
        }

        lastSpawnX = 0.64f;
        lastSpawnY = 0f;
        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameover)
        {
            return;
        }

        if(Time.time >= lastSpawnTime + timeBetSpawn)
        {
            lastSpawnTime = Time.time;

            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            float xPos = Random.Range(xMin, xMax);
            float yPos = Random.Range(yMin, yMax);

            if(-0.5f <= xPos && xPos <= 0.5f)
            {
                if (xPos != 0f)
                {
                    xPos *= 3f;
                }
                else
                {
                    int j = Random.Range(0, 1);
                    if(j == 0)
                    {
                        xPos = -1.5f;
                    }
                    else
                    {
                        xPos = 1.5f;
                    }
                }
            }

            if(lastSpawnX + xPos < -3.8f || lastSpawnX + xPos > 3.8f)
            {
                xPos *= -1f;
            }

            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(true);

            platforms[currentIndex].transform.position = new Vector2(lastSpawnX + xPos, lastSpawnY + yPos);
            currentIndex++;
            lastSpawnX += xPos;
            lastSpawnY += yPos;

            if(currentIndex >= count)
            {
                currentIndex = 0;
            }
        }
    }
}
