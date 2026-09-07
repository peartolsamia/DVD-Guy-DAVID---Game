using UnityEngine;

/// <summary>
/// Bounces this object off the screen edges. Works with any object that has a
/// BouncingMovement component — player, enemies, or projectiles — no dependency
/// on the Player singleton.
/// </summary>
[RequireComponent(typeof(BouncingMovement))]
public class ScreenBoundsHandler : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 localExtents;
    private BouncingMovement movement;

    public bool IsActive { get; set; } = true;

    // Screen bounds only change when the camera moves (room transitions), not every frame.
    // Cached statically so every bouncing object (player, enemies, projectiles) shares
    // one calculation instead of each doing its own ViewportToWorldPoint every frame.
    private static Vector3 cachedMinBounds;
    private static Vector3 cachedMaxBounds;
    private static Vector3 lastCameraPosition;
    private static bool boundsCacheValid = false;

    // Static fields survive scene reloads if "Domain Reload" is disabled in Player Settings.
    // Reset the cache on load so a stale bounds value from a previous scene can't leak in.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetStaticCache()
    {
        boundsCacheValid = false;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        movement = GetComponent<BouncingMovement>();

        // Cache local size instead of world bounds to prevent scaling/position miscalculations on room transitions
        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            localExtents = spriteRenderer.sprite.bounds.extents;
        }
        else if (TryGetComponent<Collider2D>(out var col))
        {
            localExtents = col.bounds.extents;
        }
    }

    private void LateUpdate()
    {
        if (!IsActive || movement == null) return;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }

        UpdateBoundsCacheIfNeeded();

        Vector3 pos = transform.position;

        // Multiply local extents with lossyScale for accurate bounds calculation
        Vector2 objectBounds = new Vector2(localExtents.x * transform.lossyScale.x, localExtents.y * transform.lossyScale.y);

        Vector2 dir = movement.Direction;
        bool bounced = false;

        if ((pos.x - objectBounds.x <= cachedMinBounds.x && dir.x < 0) || (pos.x + objectBounds.x >= cachedMaxBounds.x && dir.x > 0))
        {
            dir.x *= -1;
            bounced = true;
        }

        if ((pos.y - objectBounds.y <= cachedMinBounds.y && dir.y < 0) || (pos.y + objectBounds.y >= cachedMaxBounds.y && dir.y > 0))
        {
            dir.y *= -1;
            bounced = true;
        }

        if (bounced)
        {
            movement.Bounce(dir);
        }
    }

    private void UpdateBoundsCacheIfNeeded()
    {
        if (boundsCacheValid && mainCamera.transform.position == lastCameraPosition)
        {
            return;
        }

        cachedMinBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        cachedMaxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));
        lastCameraPosition = mainCamera.transform.position;
        boundsCacheValid = true;
    }
}