using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Text livesText;
    public float speed;
    public float jumpForce;
    public float freezeSpeed;
    public Text winText;
    private float moveInput;
    private bool facingRight = true;

    private Rigidbody2D rb;

    private bool touchingEnemy;
    public Transform enemyCheck;
    public LayerMask whatIsEnemy;
    private bool touchingFlyingEnemy;
    public Transform flyingEnemyCheck;
    public LayerMask whatIsFlyingEnemy;
    public float checkHurtRadius;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public bool canMove;

    public float timeHurt = 0.1f;
    bool isHurt;
    float hurtTimer;
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    private int livesValue = 3;
    private int extraJumps;
    public int extraJumpsValue;
    public bool gameOver = false;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
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

        if (gameOver == false)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
        else if (gameOver == true)
        {
            rb.velocity = new Vector2(moveInput * 0, rb.velocity.y);
        }

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

        touchingEnemy = Physics2D.OverlapCircle(enemyCheck.position, checkHurtRadius, whatIsEnemy);
        touchingFlyingEnemy = Physics2D.OverlapCircle(flyingEnemyCheck.position, checkHurtRadius, whatIsFlyingEnemy);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (touchingEnemy == true)
        {
            if (isInvincible)
                return;
            isHurt = true;
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        if (touchingFlyingEnemy == true)
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
            if (livesValue <= 0)
            {
                anim.SetTrigger("isDying");
                gameOver = true;
                //This is the text for the lose screen
                winText.text = "You lose!";
            }
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Win area")
        {
            gameOver = true;
            //This is the text for the win screen
            winText.text = "You Win!";
        }
    }
}