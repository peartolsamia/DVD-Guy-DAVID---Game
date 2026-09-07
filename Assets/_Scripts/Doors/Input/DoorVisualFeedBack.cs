using UnityEngine;

/// <summary>
/// Purely visual. Reflects the door's collider state on its sprite:
/// - collider enabled (blocking / "kapalý")  -> sprite fully opaque, unchanged.
/// - collider disabled (passable / "açýk")   -> sprite semi-transparent.
///
/// This component has zero say over door state — it only listens to
/// DoorInputHandler.OnDoorOpenChanged and reacts. All state logic stays in
/// DoorInputHandler, keeping each script to a single responsibility.
/// </summary>
[RequireComponent(typeof(DoorInputHandler))]
public class DoorVisualFeedback : MonoBehaviour
{
    [SerializeField] private SpriteRenderer doorSpriteRenderer;
    [SerializeField, Range(0f, 1f)] private float openAlpha = 0.1f;

    private DoorInputHandler doorInputHandler;
    private float closedAlpha = 1f;

    private void Awake()
    {
        doorInputHandler = GetComponent<DoorInputHandler>();

        if (doorSpriteRenderer == null)
        {
            doorSpriteRenderer = doorInputHandler.DoorSpriteRenderer;
        }

        if (doorSpriteRenderer != null)
        {
            // Remember whatever alpha the sprite was authored with, instead of assuming 1.
            closedAlpha = doorSpriteRenderer.color.a;
        }
    }

    private void OnEnable()
    {
        if (doorInputHandler == null) return;

        doorInputHandler.OnDoorOpenChanged += HandleDoorOpenChanged;

        // Sync visuals immediately with whatever state the door is already in
        // (e.g. after a scene load, before any toggle happens).
        HandleDoorOpenChanged(doorInputHandler.IsOpen);
    }

    private void OnDisable()
    {
        if (doorInputHandler != null)
        {
            doorInputHandler.OnDoorOpenChanged -= HandleDoorOpenChanged;
        }
    }

    private void HandleDoorOpenChanged(bool isOpen)
    {
        if (doorSpriteRenderer == null) return;

        Color color = doorSpriteRenderer.color;
        color.a = isOpen ? openAlpha : closedAlpha;
        doorSpriteRenderer.color = color;
    }
}