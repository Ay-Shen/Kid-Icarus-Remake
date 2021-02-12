using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text livesText;
    public float speed;
    public float jumpForce;
    private float moveInput;
    private bool facingRight = true;

    private Rigidbody2D rb;

    private bool touchingEnemy;
    public Transform enemyCheck;
    public LayerMask whatIsEnemy;
    public float checkHurtRadius;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public float timeHurt = 0.1f;
    bool isHurt;
    float hurtTimer;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    private int livesValue = 3;
    private int extraJumps;
    public int extraJumpsValue;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
        livesText.text = "Lives: " + livesValue.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

        touchingEnemy = Physics2D.OverlapCircle(enemyCheck.position, checkHurtRadius, whatIsEnemy);
    }

    void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    void Update()
    {
        if (touchingEnemy == true)
        {
            if (isInvincible)
                return;
            isHurt = true;
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        if (isHurt)
        {
            livesValue -= 1;
            livesText.text = "Lives: " + livesValue.ToString();
            hurtTimer -= Time.deltaTime;
            if (hurtTimer < 0)
                isHurt = false;
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
        {
            anim.SetTrigger("takeOff");
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if (moveInput == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Pickup")
        {
            livesValue += 1;
            livesText.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }
    }
}