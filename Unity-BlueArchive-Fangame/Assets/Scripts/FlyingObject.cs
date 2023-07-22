using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    private AudioSource missileAudio;

    [SerializeField] private float speed = 5f;
    private float xPos;
    private bool isFacingRight = true;

    Vector2 rightPool = new Vector2(10f, 0f);
    Vector2 leftPool = new Vector2(-10f, 0f);

    private void Start()
    {
        missileAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        xPos = transform.position.x;
        if(isFacingRight && xPos > 0)
        {
            isFacingRight = false;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
        else if(!isFacingRight && xPos < 0)
        {
            isFacingRight = true;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isGameover && xPos < 0)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else if(!GameManager.instance.isGameover && xPos > 0)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if (collision.collider.tag != "Player")
        {
            if (isFacingRight)
            {
                transform.position = rightPool;
            }
            else
            {
                transform.position = leftPool;
            }
        }
        */

        if (isFacingRight)
        {
            transform.position = rightPool;
        }
        else
        {
            transform.position = leftPool;
        }

        missileAudio.Play();

        ObstacleManager.instance.explosions[ObstacleManager.instance.explosionIndex].transform.position = collision.contacts[0].point;
        ObstacleManager.instance.explosions[ObstacleManager.instance.explosionIndex].SetActive(false);
        ObstacleManager.instance.explosions[ObstacleManager.instance.explosionIndex].SetActive(true);
        ObstacleManager.instance.explosionIndex++;

        if (ObstacleManager.instance.explosionIndex >= ObstacleManager.instance.missileCount)
        {
            ObstacleManager.instance.explosionIndex = 0;
        }
    }
}
