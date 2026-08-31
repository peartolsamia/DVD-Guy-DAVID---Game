using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 5f;
    private float speedMultiplier = 1f;
    private Rigidbody2D rb;

    public Vector2 Direction { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Direction = new Vector2(1f, 1f).normalized;
        UpdateVelocity();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        Direction = Vector2.Reflect(Direction, normal).normalized;
        UpdateVelocity();
    }

    public void SetDirection(Vector2 newDirection)
    {
        Direction = newDirection;
        UpdateVelocity();
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        if (rb != null)
        {
            rb.linearVelocity = Direction * (baseSpeed * speedMultiplier);
        }
    }
}