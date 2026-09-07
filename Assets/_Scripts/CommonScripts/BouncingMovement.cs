using UnityEngine;

/// <summary>
/// Generic "DVD-style" bouncing movement: moves in a direction at a given speed and
/// reflects off collisions. Has no dependency on Player or PlayerMetaDataManager, so
/// it can be attached to the player, enemies, or projectiles alike.
///
/// Player-specific concerns (syncing base speed from PlayerMetaDataManager, registering
/// bounces with PlayerBounceRewardHandler) live in PlayerMovementSync, not here.
///
/// Enemy/projectile-specific behaviors (interval stop-start, slow-down-and-despawn, etc.)
/// should be their own small components that call Pause()/Resume()/SetSpeedMultiplier()
/// on this component or listen to OnBounce — no changes to this file needed as new
/// enemy/projectile types are added.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BouncingMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private Vector2 defaultDirection = Vector2.one;

    private float speedMultiplier = 1f;
    private bool isPaused = false;
    private Rigidbody2D rb;

    public Vector2 Direction { get; private set; }
    public float BaseSpeed => baseSpeed;
    public float SpeedMultiplier => speedMultiplier;
    public bool IsPaused => isPaused;

    /// <summary>Raised whenever direction changes due to a bounce (collision or screen edge).</summary>
    public event System.Action<Vector2> OnBounce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set here (not Start) so spawners can call SetDirection() right after
        // Instantiate() without it being overwritten.
        if (Direction == Vector2.zero)
        {
            Direction = defaultDirection.normalized;
        }
    }

    private void Start()
    {
        UpdateVelocity();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        Bounce(Vector2.Reflect(Direction, normal).normalized);
    }

    /// <summary>Changes direction as a bounce (fires OnBounce). Use this for screen-edge
    /// bounces, wall bounces, etc.</summary>
    public void Bounce(Vector2 newDirection)
    {
        Direction = newDirection.normalized;
        UpdateVelocity();
        OnBounce?.Invoke(Direction);
    }

    /// <summary>Sets direction without treating it as a bounce (no OnBounce event).
    /// Use this for initial spawn direction.</summary>
    public void SetDirection(Vector2 newDirection)
    {
        Direction = newDirection.normalized;
        UpdateVelocity();
    }

    public void SetBaseSpeed(float newBaseSpeed)
    {
        baseSpeed = newBaseSpeed;
        UpdateVelocity();
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
        UpdateVelocity();
    }

    public void Pause()
    {
        isPaused = true;
        UpdateVelocity();
    }

    public void Resume()
    {
        isPaused = false;
        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        if (rb == null) return;
        rb.linearVelocity = isPaused ? Vector2.zero : Direction * (baseSpeed * speedMultiplier);
    }
}