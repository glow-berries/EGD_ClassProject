using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float moveX;

    private float moveSpeed = 5f;

    [SerializeField] private TextMeshProUGUI coinsText;

    [SerializeField] private TextMeshProUGUI deathText;

    [SerializeField] private float jumpForce = 30f;

    [SerializeField] private float dashForce = 1f;

    private int numJumps = 0;

    private Coroutine dashCooldownRoutine;

    private bool isTouchingGround = false;

    private bool isDashing = false;

    private int coinsCollected = 0;

    private int numDeaths = 0;

    [SerializeField] private Animator animator;

    [SerializeField] private SpriteRenderer sprite;

    bool isMovingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public int getNumDeaths()
    {
        return numDeaths;
    }

    public void UpdateCoins()
    {
        coinsCollected++;
        coinsText.text = "Coins: " + coinsCollected.ToString();
    }

    public void Respawn(Vector2 newPosition)
    {
        rb.position = newPosition;
        rb.linearVelocity = Vector2.zero;
        numDeaths++;
        deathText.text = "Deaths: " + numDeaths.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isJumping", !isTouchingGround);
        animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocityX) > 0.01f);
        if (isMovingRight && moveX < 0)
        {
            isMovingRight = false;
            sprite.flipX = true;
        }
        else if (!isMovingRight && moveX > 0)
        {
            isMovingRight = true;
            sprite.flipX = false;
        }

        if (rb.linearVelocityY < -10f)
        {
            rb.linearVelocityY = -10f;
        }
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
            rb.linearVelocityY = 0;
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
            rb.AddForceX(-dashForce, ForceMode2D.Impulse);
        }
        else if (mousePos > 0)
        {
            rb.AddForceX(dashForce, ForceMode2D.Impulse);
        }
        else
        {
            int random = Random.Range(0, 2);

            switch (random)
            {
                case 0:
                    rb.AddForceX(-dashForce, ForceMode2D.Impulse);
                    break;
                case 1:
                    rb.AddForceX(dashForce, ForceMode2D.Impulse);
                    break;
            }
        }

        yield return new WaitForSeconds(0.25f);

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
