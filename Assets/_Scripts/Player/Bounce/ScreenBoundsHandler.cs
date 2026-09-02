using UnityEngine;

public class ScreenBoundsHandler : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 objectBounds;
    private Player player;

    public bool IsActive { get; set; } = true;

    private void Start()
    {
        mainCamera = Camera.main;


        player = Player.Instance;


        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            objectBounds = spriteRenderer.bounds.extents;
        }
        else if (TryGetComponent<Collider2D>(out var col))
        {
            objectBounds = col.bounds.extents;
        }
    }

    private void LateUpdate()
    {
        if (!IsActive || player == null || player.Movement == null) return;


        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }

        Vector3 pos = transform.position;
        Vector3 minBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 maxBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));


        Vector2 dir = player.Movement.Direction;
        bool bounced = false;

        if ((pos.x - objectBounds.x <= minBounds.x && dir.x < 0) || (pos.x + objectBounds.x >= maxBounds.x && dir.x > 0))
        {
            dir.x *= -1;
            bounced = true;
            player.BounceReward?.RegisterBounce();
        }

        if ((pos.y - objectBounds.y <= minBounds.y && dir.y < 0) || (pos.y + objectBounds.y >= maxBounds.y && dir.y > 0))
        {
            dir.y *= -1;
            bounced = true;
            player.BounceReward?.RegisterBounce();
        }

        if (bounced)
        {
            player.Movement.SetDirection(dir);
        }
    }
}