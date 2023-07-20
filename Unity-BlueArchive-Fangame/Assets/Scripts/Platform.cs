using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private bool stepped = false;
    private bool damaged = false;

    public Sprite damageSprite;

    private SpriteRenderer spriterenderer;
    private WaitForSeconds blink = new WaitForSeconds(1f);
    private float speed = 10f;

    private void Awake()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        stepped = false;
        damaged = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player" && collision.contacts[0].normal.y <= -1f)
        {
            if (!stepped)
            {
                stepped = true;
                GameManager.instance.AddScore(1);
            }
            else
            {
                if (damaged)
                {
                    StartCoroutine(damagedPlatform());
                }
            }
            
        }

        if (collision.collider.tag == "Dead")
        {
            damaged = true;
            spriterenderer.sprite = damageSprite;
        }
    }

    IEnumerator damagedPlatform()
    {
        yield return blink;
        transform.Translate(Vector3.down * speed);
        yield return blink;
        transform.Translate(Vector3.up * speed);
    }
}
