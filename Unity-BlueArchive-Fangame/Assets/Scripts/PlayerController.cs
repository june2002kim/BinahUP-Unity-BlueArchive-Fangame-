using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 200f;
    public float speed = 2f;

    private float horizontal;
    private int jumpCount = 0;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private bool isDead = false;
    private bool isGrounded_ = false;

    private Rigidbody2D playerRigidbody;
    private Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDead)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            jumpCount++;

            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
        }
        else if(Input.GetButtonUp("Jump") && playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y * 0.5f);
        }

        Flip();
        animator.SetBool("Grounded", isGrounded);

        if (!isGrounded_ && Input.GetButtonDown("Jump"))
        {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            playerRigidbody.velocity = new Vector2(horizontal * speed, playerRigidbody.velocity.y);
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");

        playerRigidbody.velocity = Vector2.zero;
        isDead = true;

        GameManager.instance.OnPlayerDead();
    }

    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Dead" && !isDead)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded_ = true;
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded_ = false;
    }
}
