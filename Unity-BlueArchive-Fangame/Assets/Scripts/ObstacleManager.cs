using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script for Obstacle Management */

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager instance;

    public GameObject missilePrefab;        // Missile Prefab
    public int missileCount = 6;            // Count of missile can be created
    public GameObject explosionPrefab;      // Explosion effect Prefab
    public Laser laserPrefab;               // Laser Prefab's Laser script

    public float timeBetSpawnMin = 0.01f;   // Minimum spawn time of missiles
    public float timeBetSpawnMax = 1.50f;   // Maximum spawn time of missiles
    private float timeBetSpawn;             // Time interval of missile spawn
    public float timeBetSpawnMin_ = 2.5f;   // Minimum spawn time of laser
    public float timeBetSpawnMax_ = 3.5f;   // Maximum spawn time of laser
    private float timeBetSpawn_;            // Time interval of laser spawn

    public float xMin = -8f;                // Minimum X coordinate of obstacle spawn position
    public float xMax = 8f;                 // Maximum X coordinate of obstacle spawn position 
    public float yMin = 0f;                 // Minimum Y coordinate of obstacle spawn position
    public float yMax = 10f;                // Maximum Y coordinate of obstacle spawn position

    // Position for Laser
    private float xPos;
    private float xPos_;
    private float yPos;
    private float yPos_;
    private float lastSpawnX_;
    private float lastSpawnY_;

    private GameObject[] missiles;          // GameObject array for missiles
    private int missileIndex = 0;           // Missile index for control
    public GameObject[] explosions;         // GameObject array for explosions
    public int explosionIndex = 0;          // Explosion index for control
    private Laser lasers;                   // Laser script for lasers
    private bool firstLaser = true;         // boolean variable for instantiate laser for the first time

    private Vector2 poolPosition = new Vector2(-23, 0);             // Pool position for creating obstacles when game starts
    private float lastSpawnTime;                                    // Last spawn time for missiles                      
    private float lastSpawnTime_;                                   // Last spawn time for laser

    private Color32 c1 = new Color32(239, 62, 116, 200);            // Red color for laser alert
    private Color c2 = new Color32(255, 255, 255, 200);             // White color for laser (Laser's original color)

    private WaitForSeconds alertDelay = new WaitForSeconds(1.5f);   // Coroutine delay for alert
    private WaitForSeconds laserDelay = new WaitForSeconds(0.7f);   // Coroutine delay for laser

    [SerializeField] private float windSpeed = 10f;                 // Wind blowing speed for AddForce
    [SerializeField] private float missileSpeed = 5f;

    [SerializeField] public bool wind = false;                      // Wind trigger variable
    [SerializeField] public bool missile = false;                   // Missille trigger variable
    [SerializeField] public bool laser = false;                     // Laser trigger variable

    [SerializeField] public bool eastWind = false;                  // Wind blowing direction
    public GameObject eastSandstorm;                                // Sand gradient background for eastWind
    public GameObject westSandstorm;                                // Sand gradient background for westWind

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
            Debug.LogWarning("More than 2 ObstacleManager exist in Scene!");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
         Create instances with prefab.
        allocate array of missiles and explosions with prefab
        instantiate it to poolPosition
        reset lastSpawnTimes and timeBetSpawns
         */

        missiles = new GameObject[missileCount];
        explosions = new GameObject[missileCount];

        for(int i = 0; i < missileCount; i++)
        {
            missiles[i] = Instantiate(missilePrefab, poolPosition, Quaternion.identity);
            explosions[i] = Instantiate(explosionPrefab, poolPosition, Quaternion.identity);
        }

        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
        lastSpawnTime_ = 0f;
        timeBetSpawn_ = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         If game is not overed and obstacle trigger variables satisfy, call obstacle function. 
         */

        if (!GameManager.instance.isGameover)
        {
            if (wind)
            {
                windBlower();
            }

            if (missile)
            {
                missileLauncher();
            }

            if (laser)
            {
                if (firstLaser)
                {
                    // Instantiate laser from here because it plays sound on awake
                    lasers = Instantiate(laserPrefab, Vector3.zero, Quaternion.identity);
                    firstLaser = false;
                }
               
                StartCoroutine(laserShooter());
                //laser = false;
            }
        }
    }

    private void windBlower()
    {
        /*
         Wind blowing function by AddForce in Rigidbody.
         */

        // Because windBlower function is called in update, it doesn't stop even timeScale is 0. So it prevents force added when players reading dialogue.
        if(Time.timeScale != 0)
        {
            if (eastWind)
            {
                PlayerController.instance.playerRigidbody.AddForce(Vector2.left * windSpeed);
                westSandstorm.SetActive(false);
                eastSandstorm.SetActive(true);
            }
            else
            {
                PlayerController.instance.playerRigidbody.AddForce(Vector2.right * windSpeed);
                eastSandstorm.SetActive(false);
                westSandstorm.SetActive(true);
            }
        }
    }

    private void missileLauncher()
    {
        /*
         Missile launching function.
         */

        // If (current time) >= (last missile spawn time) + (random time between spawn) then (spawn missile)
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            // Set (current time) to (last missile spawn time)
            lastSpawnTime = Time.time;

            // Select random (time between spawn) between timeBetSpawnMin and timeBetSpawnMax
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            // If placed missile flew far away, replace it higher than player 
            if(missiles[missileIndex].transform.position.x < -10f || missiles[missileIndex].transform.position.x > 10f)
            {
                float lastSpawnY = PlayerController.instance.transform.position.y;  // players current Y coordinate
                float yPos = Random.Range(yMin, yMax);                              // Random Y position that missile will be placed

                if (Random.Range(0, 2) == 0)
                {
                    // Place to left side
                    missiles[missileIndex].transform.position = new Vector2(xMin, lastSpawnY + yPos);
                }
                else
                {
                    // Place to right side
                    missiles[missileIndex].transform.position = new Vector2(xMax, lastSpawnY + yPos);
                }

                missiles[missileIndex].SetActive(false);
                missiles[missileIndex].SetActive(true);

                // Change target to next GameObject in missile array
                missileIndex++;

                if (missileIndex >= missileCount)
                {
                    missileIndex = 0;
                }
            }
        }
    }

    IEnumerator laserShooter()
    {
        /*
         Coroutine for shooting laser
        I thought it would work by starting coroutine once and if there's infinite while loop in coroutine.
        But setting yeild with float value, game freezes.
        So modified it to start coroutine every time .
        I don't know why it's problem and it's solved
         */

        /*
        while (true)
        {
            if (GameManager.instance.isGameover)
            {
                yield break;
            }

            if (Time.time >= lastSpawnTime_ + timeBetSpawn_)
            {
                lastSpawnTime_ = Time.time;

                timeBetSpawn_ = Random.Range(timeBetSpawnMin_, timeBetSpawnMax_);

                float lastSpawnX_ = PlayerController.instance.transform.position.x;
                float lastSpawnY_ = PlayerController.instance.transform.position.y;
                float xPos = Random.Range(xMin, xMax);
                float yPos = Random.Range(yMin, yMax);
                float xPos_;
                float yPos_ = lastSpawnY_ + yPos * 2;

                if (xPos * (lastSpawnX_ - xPos) < 0)
                {
                    xPos_ = lastSpawnX_ - xPos;
                }
                else
                {
                    xPos_ = (lastSpawnX_ - xPos) * (-1f);
                }

                lasers.startPosition = new Vector3(xPos, lastSpawnY_ * -5, 0);
                lasers.endPosition = new Vector3(xPos_ * 10, yPos_ * 10, 0);

                lasers.lineRenderer.startColor = c1;
                lasers.lineRenderer.endColor = c1;

                lasers.gameObject.SetActive(false);
                lasers.gameObject.SetActive(true);

                yield return alertDelay;

                lasers.lineRenderer.startColor = c2;
                lasers.lineRenderer.endColor = c2;
                lasers.tag = "Dead";

                yield return laserDelay;

                lasers.tag = "Untagged";
                lasers.gameObject.SetActive(false);
            }
        }
        */

        if (Time.time >= lastSpawnTime_ + timeBetSpawn_)
        {
            lastSpawnTime_ = Time.time;

            timeBetSpawn_ = Random.Range(timeBetSpawnMin_, timeBetSpawnMax_);

            lastSpawnX_ = PlayerController.instance.transform.position.x;                                           // Players current X coordinate
            lastSpawnY_ = PlayerController.instance.transform.position.y;                                           // Players current Y coordinate
            xPos = Random.Range(xMin, xMax) * 10f;                                                                  // Random Laser's start X position
            yPos = Random.Range(yMin, yMax) * (-10f);                                                               // Random Laser's start Y position (negative)
            xPos_ = xPos * (-3f);                                                                                   // Laser's end X position (other side)
            yPos_ = (lastSpawnY_ + 2 - yPos) / (lastSpawnX_ - xPos) * (xPos_ - lastSpawnX_) + lastSpawnY_ + 2;      // Laser's end Y position (calculated line equation which passes by start position of laser and above player's position)

            // Set laser's positions
            lasers.startPosition = new Vector3(xPos, yPos, 0);
            lasers.endPosition = new Vector3(xPos_, yPos_, 0);

            lasers.lineRenderer.startColor = c1;
            lasers.lineRenderer.endColor = c1;

            lasers.gameObject.SetActive(false);
            lasers.gameObject.SetActive(true);

            yield return alertDelay;

            lasers.lineRenderer.startColor = c2;
            lasers.lineRenderer.endColor = c2;
            lasers.tag = "Dead";

            yield return laserDelay;

            // Change tag to "Untagged" to avoid touching laser alert makes player die
            lasers.tag = "Untagged";
            lasers.gameObject.SetActive(false);
        }
    }

}
