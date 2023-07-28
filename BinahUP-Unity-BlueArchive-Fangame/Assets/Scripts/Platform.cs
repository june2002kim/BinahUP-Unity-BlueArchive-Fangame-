using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script for changing damaged sprite and reset its stepped state of platforms */

public class Platform : MonoBehaviour
{
    private bool stepped = false;
    private bool damaged = false;

    public Sprite damageSprite;

    private SpriteRenderer spriterenderer;
    private WaitForSeconds blink = new WaitForSeconds(1f);  // Time of damaged platform disappears after player steps on it
    private float speed = 10f;

    private void Awake()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        /*
         Reset platform's state
         */

        stepped = false;
        damaged = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /* 
         Change platforms state when collision occurs 
         */

        // If player steps on platform (calculated by contacts.normal.y)
        if(collision.collider.tag == "Player" && collision.contacts[0].normal.y <= -1f)
        {
            if (!stepped)
            {
                // If it hasn't stepped after replaced, change its state and call AddScore function in GameManager
                stepped = true;
                GameManager.instance.AddScore(1);
            }
            else
            {
                if (damaged)
                {
                    // If player steps on damaged platform, start coroutine damagedPlatform
                    StartCoroutine(damagedPlatform());
                }
            }
            
        }

        // If platform collides with missile, change its state to damaged and sprite also
        if (collision.collider.tag == "Dead")
        {
            damaged = true;
            spriterenderer.sprite = damageSprite;
        }
    }

    IEnumerator damagedPlatform()
    {
        /*
         If player steps on damaged platform, moves that platform outside of screen after blink and puts it back where it placed.
        Not using setActive to avoid reseting each platforms state. (stepped / damaged)
         */

        yield return blink;
        transform.Translate(Vector3.down * speed);
        yield return blink;
        transform.Translate(Vector3.up * speed);
    }
}
