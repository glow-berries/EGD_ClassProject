using System;
using UnityEngine;

public class BallBouncer : MonoBehaviour
{
    private Rigidbody2D rb;

    // [SerializeField] private float bounceForce = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            // rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
