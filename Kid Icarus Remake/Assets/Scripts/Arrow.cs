using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        TankEnemy e = hitInfo.GetComponent<TankEnemy>();
        RegularEnemy f = hitInfo.GetComponent<RegularEnemy>();
        if(e != null)
        {
            e.TakeDamage(damage);
        }
        Destroy(gameObject);
        if (f != null)
        {
            f.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
