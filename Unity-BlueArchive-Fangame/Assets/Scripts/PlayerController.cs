using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script for Player Controlling */

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float jumpForce = 200f;      // Player's jump force for AddForce
    public float speed = 2f;            // Player's move speed

    private float horizontal;           // Get horizontal axis input
    private int jumpCount = 0;
    private bool isFacingRight = true;
    private bool isGrounded = false;
    private bool isDead = false;
    private bool isMoving = false;

    public Rigidbody2D playerRigidbody;
    private Animator animator;
    private AudioSource playerAudio;

    public GameObject groundCheck;
    public GameObject warning;

    public AudioClip deathClip;

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
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        /*
         Control player's vertical movements by rigidbody's AddForce
         */

        if (isDead)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump") && jumpCount < 2 && Time.timeScale != 0)
        {
            jumpCount++;

            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            playerAudio.Play();
        }
        else if(Input.GetButtonUp("Jump") && playerRigidbody.velocity.y > 0)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y * 0.5f);
        }

        // Call Flip function
        Flip();

        animator.SetBool("Grounded", isGrounded);

        // Use Raycast to consider player is on ground or not
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.transform.position, Vector2.down, 0.1f);
        //Debug.DrawRay(groundCheck.transform.position, Vector2.down * 0.1f, Color.red);

        if(hit.collider != null)
        {
            if (hit.collider.tag == "Ground")
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }

        // If player is close to deadzone, SetActive warning sprite
        if(transform.position.y < -4)
        {
            warning.SetActive(true);
        }
        else
        {
            warning.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        /*
         Control players horizontal movements by rigidbody's velocity
         */

        if (!isDead)
        {
            playerRigidbody.velocity = new Vector2(horizontal * speed, playerRigidbody.velocity.y);
            if (horizontal != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            animator.SetBool("Moving", isMoving);
        }
    }

    private void Die()
    {
        /*
         Control player's dead statement.
        play deathClip audio and change player's velocity to zero
        and call GameManager's OnPlayerDead function
         */

        animator.SetTrigger("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerRigidbody.velocity = Vector2.zero;
        isDead = true;

        GameManager.instance.OnPlayerDead();
    }

    private void Flip()
    {
        /*
         Flips player's sprite to 'right' direction
         */

        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            // Flip function calls in Update, so let it doesn't change direction when game is paused by timeScale
            if (Time.timeScale != 0)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
         If player collides with trigger which has "Dead" tag(laser), kill player
         */

        if(collision.tag=="Dead" && !isDead)
        {
            Die();
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         If player collides with smaller than 45 degree ground, reset jump count
        If player collides with object which has "Dead" tag(missile), kill player
        Not considering 'isGrounded' boolean by OnCollisionEnter2D and OnCollisionExit2D is player keeps taking apart with platform because it scrolls down.
         */

        if(collision.contacts[0].normal.y > 0.7f)
        {
            jumpCount = 0;
        }
        if(collision.gameObject.tag == "Dead" && !isDead)
        {
            Die();
        }
    }
}
