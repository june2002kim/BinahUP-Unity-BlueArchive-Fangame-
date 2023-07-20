using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager instance;

    public GameObject missilePrefab;
    public int missileCount = 6;
    public GameObject explosionPrefab;
    public Laser laserPrefab;

    public float timeBetSpawnMin = 0.01f;
    public float timeBetSpawnMax = 1.50f;
    private float timeBetSpawn;
    public float timeBetSpawnMin_ = 2f;
    public float timeBetSpawnMax_ = 3f;
    private float timeBetSpawn_;

    public float xMin = -8f;
    public float xMax = 8f;
    public float yMin = 0f;
    public float yMax = 10f;

    private GameObject[] missiles;
    private int missileIndex = 0;
    public int explosionIndex = 0;
    public GameObject[] explosions;
    private Laser lasers;

    private Vector2 poolPosition = new Vector2(-23, 0);
    private float lastSpawnTime;
    private float lastSpawnTime_;

    private Color32 c1 = new Color32(239,62,116, 200);
    private Color c2 = Color.yellow;

    private WaitForSeconds alertDelay = new WaitForSeconds(1f);
    private WaitForSeconds laserDelay = new WaitForSeconds(2f);

    [SerializeField] private float windSpeed = 10f;
    [SerializeField] private float missileSpeed = 5f;

    [SerializeField] public bool wind = false;
    [SerializeField] public bool missile = false;
    [SerializeField] public bool laser = false;

    [SerializeField] public bool eastWind = false;
    public GameObject eastSandstorm;
    public GameObject westSandstorm;

    private void Awake()
    {
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

        lasers = Instantiate(laserPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
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
                StartCoroutine(laserShooter());
                laser = false;
            }
        }
    }

    private void windBlower()
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

    private void missileLauncher()
    {
        if (Time.time >= lastSpawnTime + timeBetSpawn)
        {
            lastSpawnTime = Time.time;

            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);

            if(missiles[missileIndex].transform.position.x < -10f || missiles[missileIndex].transform.position.x > 10f)
            {
                float lastSpawnY = PlayerController.instance.transform.position.y;
                float yPos = Random.Range(yMin, yMax);

                if (Random.Range(0, 2) == 0)
                {
                    missiles[missileIndex].transform.position = new Vector2(xMin, lastSpawnY + yPos);
                }
                else
                {
                    missiles[missileIndex].transform.position = new Vector2(xMax, lastSpawnY + yPos);
                }

                missiles[missileIndex].SetActive(false);
                missiles[missileIndex].SetActive(true);

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
    }
}
