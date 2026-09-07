using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central manager for handling door input interactions.
/// Listens to directional inputs from InputReader and toggles the corresponding door
/// only if it is matching the input direction and currently visible within the main camera bounds.
///
/// NOTE: door open/closed state is always changed through DoorInputHandler.ToggleDoor()/
/// SetDoorOpen(). This manager never touches a Collider2D directly, so it can't race
/// against the room-transition pairing logic in DoorPairTriggerController2D.
/// </summary>
public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance { get; private set; }

    private readonly List<DoorInputHandler> allDoors = new List<DoorInputHandler>();
    private Camera mainCamera;
    private bool isSubscribed = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        CacheMainCamera();
    }

    private void Start()
    {
        TrySubscribe();
    }

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void OnDisable()
    {
        TryUnsubscribe();
    }

    private void TrySubscribe()
    {
        if (!isSubscribed && InputReader.Instance != null)
        {
            InputReader.Instance.OnDoorDirectionPressed += HandleDoorDirection;
            isSubscribed = true;
#if UNITY_EDITOR
            Debug.Log("[DoorManager] Successfully Subscribed to InputReader event.");
#endif
        }
    }

    private void TryUnsubscribe()
    {
        if (isSubscribed && InputReader.Instance != null)
        {
            InputReader.Instance.OnDoorDirectionPressed -= HandleDoorDirection;
            isSubscribed = false;
        }
    }

    public void RegisterDoor(DoorInputHandler door)
    {
        if (!allDoors.Contains(door))
        {
            allDoors.Add(door);
        }
    }

    public void UnregisterDoor(DoorInputHandler door)
    {
        if (allDoors.Contains(door))
        {
            allDoors.Remove(door);
        }
    }

    private void HandleDoorDirection(Vector2 direction)
    {
        DoorType targetType = GetDoorTypeFromDirection(direction);
        if (targetType == DoorType.None) return;

        CacheMainCamera();

        // Reset camera matrix and sync physics transforms before running frustum test
        mainCamera.ResetWorldToCameraMatrix();
        Physics2D.SyncTransforms();

        Plane[] cameraPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        for (int i = 0; i < allDoors.Count; i++)
        {
            var door = allDoors[i];

            if (door != null && door.DoorType == targetType)
            {
                if (IsDoorVisible(door, cameraPlanes))
                {
                    door.ToggleDoor();
                    break;
                }
            }
        }
    }

    private bool IsDoorVisible(DoorInputHandler door, Plane[] cameraPlanes)
    {
        Collider2D doorCollider = door.DoorCollider;
        if (doorCollider != null)
        {
            Bounds bounds = doorCollider.bounds;
            bounds.extents = new Vector3(bounds.extents.x, bounds.extents.y, 100f);

            if (GeometryUtility.TestPlanesAABB(cameraPlanes, bounds))
                return true;
        }

        SpriteRenderer renderer = door.DoorSpriteRenderer;
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            bounds.extents = new Vector3(bounds.extents.x, bounds.extents.y, 100f);

            // FIX: previously this tested the original renderer.bounds instead of the
            // widened 'bounds' above, so the z-extent widening had no effect at all.
            if (GeometryUtility.TestPlanesAABB(cameraPlanes, bounds))
                return true;
        }

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(door.transform.position);
        return viewportPos.x >= 0f && viewportPos.x <= 1f && viewportPos.y >= 0f && viewportPos.y <= 1f;
    }

    private DoorType GetDoorTypeFromDirection(Vector2 direction)
    {
        if (direction.y > 0.5f) return DoorType.Top;
        if (direction.y < -0.5f) return DoorType.Bottom;
        if (direction.x < -0.5f) return DoorType.Left;
        if (direction.x > 0.5f) return DoorType.Right;
        return DoorType.None;
    }

    private void CacheMainCamera()
    {
        if (mainCamera == null || !mainCamera.isActiveAndEnabled)
        {
            mainCamera = Camera.main;
        }
    }
}