using UnityEngine;

public class EndController : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector3 originalPos;

    [SerializeField] private EndActivator endActivator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            this.rb = rb;
        }

        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.linearVelocityY = 0f;
        }
    }

    public void Reset()
    {
        rb.linearVelocity = Vector3.zero;
        rb.gravityScale = 0f;
        transform.position = originalPos;

        endActivator.StopReset();
    }
}
