using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager instance;

    public GameObject missilePrefab;
    public int missileCount = 6;
    private LineRenderer lineRenderer;
    public Laser laserPrefab;

    public float timeBetSpawnMin = 0.01f;
    public float timeBetSpawnMax = 2.00f;
    private float timeBetSpawn;
    public float timeBetSpawnMin_ = 2f;
    public float timeBetSpawnMax_ = 3f;
    private float timeBetSpawn_;

    public float xMin = -10f;
    public float xMax = 10f;
    public float yMin = 0f;
    public float yMax = 10f;

    private GameObject[] missiles;
    private int missileIndex = 0;
    private Laser lasers;

    private Vector2 poolPosition = new Vector2(-23, 0);
    private float lastSpawnTime;
    private float lastSpawnTime_;

    [SerializeField] private float windSpeed = 13f;
    [SerializeField] private float missileSpeed = 5f;

    [SerializeField] public bool wind = false;
    [SerializeField] public bool missile = false;
    [SerializeField] public bool laser = false;

    [SerializeField] public bool eastWind = false;

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

        lineRenderer = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        missiles = new GameObject[missileCount];

        for(int i = 0; i < missileCount; i++)
        {
            missiles[i] = Instantiate(missilePrefab, poolPosition, Quaternion.identity);
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
                StartCoroutine("laserShooter");
            }
        }
    }

    private void windBlower()
    {
        if (eastWind)
        {
            PlayerController.instance.playerRigidbody.AddForce(Vector2.left * windSpeed);
        }
        else
        {
            PlayerController.instance.playerRigidbody.AddForce(Vector2.right * windSpeed);
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

    public int cnt = 0;

    IEnumerator laserShooter()
    {
        if (Time.time >= lastSpawnTime_ + timeBetSpawn_)
        {
            lastSpawnTime_ = Time.time;

            timeBetSpawn_ = Random.Range(timeBetSpawnMin_, timeBetSpawnMax_);

            float lastSpawnX_ = PlayerController.instance.transform.position.x;
            float lastSpawnY_ = PlayerController.instance.transform.position.y;
            float xPos = Random.Range(xMin, xMax);
            float yPos = Random.Range(yMin, yMax);
            float xPos_;
            float yPos_ = lastSpawnY_ + yPos;

            if (xPos * (lastSpawnX_ - xPos) < 0)
            {
                xPos_ = lastSpawnX_ - xPos;
            }
            else
            {
                xPos_ = (lastSpawnX_ - xPos) * (-1f);
            }

            lasers.startPosition = new Vector3(xPos, -5, 0);
            lasers.endPosition = new Vector3(xPos_ * 10, yPos_ * 10, 0);

            lasers.gameObject.SetActive(false);
            lasers.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(0.5f);
            
            lasers.gameObject.SetActive(false);
        }
    }
}
