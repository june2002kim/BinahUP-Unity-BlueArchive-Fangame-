using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float xPos;
    private bool isFacingRight = true;

    Vector2 rightPool = new Vector2(10f, 0f);
    Vector2 leftPool = new Vector2(-10f, 0f);

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
    }
}
