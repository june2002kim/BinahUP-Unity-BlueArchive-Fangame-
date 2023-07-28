using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script for spawning platforms */

public class PlatformSpawner : MonoBehaviour
{
    public GameObject shortplatformPrefab;  // Short platform Prefab
    public GameObject platformPrefab;       // platform Prefab
    public GameObject longplatformPrefab;   // Long platform Prefab
    public int count = 300;                 // Count of platforms

    public float timeBetSpawnMin = 0.01f;   // Minimum spawn time of platform
    public float timeBetSpawnMax = 1.15f;   // Maximum spawn time of platform
    private float timeBetSpawn;             // Time interval of platform spawn

    public float xMin = -2.7f;              // Minimum X coordinate of platform spawn position
    public float xMax = 2.7f;               // Maximum X coordinate of platform spawn position
    public float yMin = 0.95f;              // Minimum Y coordinate of platform spawn position
    public float yMax = 1.2f;               // Maximum Y coordinate of platform spawn position
    private float lastSpawnX = 0.64f;       // Platform's last spawned X position
    private float lastSpawnY = 0f;          // Platform's last spawned Y position

    private GameObject[] platforms;         // GameObject of platform's array
    private int currentIndex = 0;           // Index for platforms control

    private Vector2 poolPosition = new Vector2(-25, 0);     // PoolPosition for spawning platforms before it placed
    private float lastSpawnTime;                            // Last spawn time for platform

    // Start is called before the first frame update
    void Start()
    {
        /*
         Instantiate platforms randomly to poolPosition
         */

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
        /*
         If game is not overed, replace platform above last placed platform
         */


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

            // If new platform's positin X is too close to last placed platform, move little bit 
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

            // If new platform's positin X is too close to left or right side, move to other direction
            if (lastSpawnX + xPos < -3.8f || lastSpawnX + xPos > 3.8f)
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
