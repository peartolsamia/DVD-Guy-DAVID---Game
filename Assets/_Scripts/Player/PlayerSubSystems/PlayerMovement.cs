using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float baseSpeed = PlayerData.DefaultSpeed;
    private float speedMultiplier = 1f;
    private Rigidbody2D rb;

    public Vector2 Direction { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        RefreshBaseSpeed();

        Direction = new Vector2(1f, 1f).normalized;
        UpdateVelocity();
    }

    private void OnEnable()
    {
        if (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            PlayerMetaDataManager.Instance.Data.OnSpeedDefaultChanged += RefreshBaseSpeed;
        }
    }

    private void OnDisable()
    {
        if (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            PlayerMetaDataManager.Instance.Data.OnSpeedDefaultChanged -= RefreshBaseSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        Direction = Vector2.Reflect(Direction, normal).normalized;
        UpdateVelocity();
    }

    public void RefreshBaseSpeed()
    {
        if (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            baseSpeed = PlayerMetaDataManager.Instance.Data.speed_Default;
        }
        else
        {
            baseSpeed = PlayerData.DefaultSpeed;
        }

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