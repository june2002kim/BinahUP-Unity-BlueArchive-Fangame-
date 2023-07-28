using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script for missiles fly through */

public class FlyingObject : MonoBehaviour
{
    private AudioSource missileAudio;

    [SerializeField] private float speed = 5f;
    private float xPos;
    private bool isFacingRight = true;          // For flipping missile sprite

    Vector2 rightPool = new Vector2(10f, 0f);
    Vector2 leftPool = new Vector2(-10f, 0f);

    private void Start()
    {
        missileAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        /*
         When its Enabled, check missiles position and flip sprite to 'right' direction
        if xPos is negative, missile should fly to right side
        if xPos is positive, missile should fly to left side
         */

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
        /*
         Makes missile fly through the screen by Translate
         */

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
         If missile collides with objects(player / platform / other missile), move it to its destination
         */

        if (isFacingRight)
        {
            transform.position = rightPool;
        }
        else
        {
            transform.position = leftPool;
        }

        // Play explosion sound
        missileAudio.Play();

        // Place ObstacleManager's explosion GameObject to where collision occurred
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
