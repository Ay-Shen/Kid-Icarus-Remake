using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularEnemy : MonoBehaviour
{
    public float walkSpeed;
    public int health = 10;
    public Transform healthDrop;
    public GameObject healthPrefab;

    [HideInInspector]
    public bool mustPatrol;
    private bool mustTurn;

    public Rigidbody2D rb;
    public Transform groundCheckPos;
    public LayerMask groundLayer;
    public Collider2D bodyCollider;
    public float airSpeed;
    public float lineOfSight;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        mustPatrol = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Instantiate(healthPrefab, healthDrop.position, healthDrop.rotation);
            Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSight)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, airSpeed * Time.deltaTime);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Patrol()
    {
        if (mustTurn || bodyCollider.IsTouchingLayers(groundLayer))
        {
            Flip();
        }

        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustPatrol = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
    }

}