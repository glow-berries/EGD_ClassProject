using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float moveX;

    private float moveSpeed = 5f;

    [SerializeField] private float jumpForce = 10f;

    private int numJumps = 0;

    private Coroutine dashCooldownRoutine;

    private bool isTouchingGround = false;

    private bool isDashing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isDashing == true)
        {
            return;
        }

        rb.linearVelocityX = moveX * moveSpeed;
    }

    void OnMove(InputValue value)
    {
        moveX = value.Get<Vector2>().x;
    }

    void OnJump(InputValue inputValue)
    {
        if (numJumps == 0 && isTouchingGround == true)
        {

            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            numJumps++;
        }
        else if (numJumps == 1 && isTouchingGround == false)
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            numJumps++;
        }
    }

    void OnFire(InputValue inputValue)
    {
        if (dashCooldownRoutine == null)
        {
            dashCooldownRoutine = StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        float mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x - transform.position.x;

        if (mousePos < 0)
        {
            rb.AddForceX(-jumpForce, ForceMode2D.Impulse);
        }
        else if (mousePos > 0)
        {
            rb.AddForceX(jumpForce, ForceMode2D.Impulse);
        }
        else
        {
            int random = Random.Range(0, 2);

            switch (random)
            {
                case 0:
                    rb.AddForceX(-jumpForce, ForceMode2D.Impulse);
                    break;
                case 1:
                    rb.AddForceX(jumpForce, ForceMode2D.Impulse);
                    break;
            }
        }

        yield return new WaitForSeconds(1f);

        dashCooldownRoutine = null;
        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            numJumps = 0;
            isTouchingGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isTouchingGround = false;
        }
    }
}
