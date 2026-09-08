using UnityEngine;

/// <summary>
/// Purely visual. Reflects the door's collider state on its sprite:
/// - collider enabled (blocking / "kapal�")  -> sprite fully opaque, unchanged.
/// - collider disabled (passable / "a��k")   -> sprite semi-transparent.
///
/// This component has zero say over door state � it only listens to
/// DoorInputHandler.OnDoorOpenChanged and reacts. All state logic stays in
/// DoorInputHandler, keeping each script to a single responsibility.
/// </summary>
[RequireComponent(typeof(DoorInputHandler))]
public class DoorVisualFeedback : MonoBehaviour
{
    [SerializeField] private SpriteRenderer doorSpriteRenderer;
    [SerializeField, Range(0f, 1f)] private float openAlpha = 0.5f;
    [SerializeField, Range(0f, 1f)] private float closedAlpha = 0.1f;

    private DoorInputHandler doorInputHandler;
    private Collider2D doorCollider;

    private void Awake()
    {
        doorInputHandler = GetComponent<DoorInputHandler>();

        // Get the SpriteRenderer directly from this GameObject rather than
        // reading DoorInputHandler.DoorSpriteRenderer. Unity does NOT guarantee
        // Awake() order between different components on the same GameObject,
        // so if DoorInputHandler.Awake() hasn't run yet, its DoorSpriteRenderer
        // property could still be null here — leaving doorSpriteRenderer
        // permanently unassigned and silently breaking all visual updates.
        if (doorSpriteRenderer == null)
        {
            doorSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Fallback for the (unusual) case where the sprite lives on a
        // different object than DoorInputHandler.
        if (doorSpriteRenderer == null && doorInputHandler != null)
        {
            doorSpriteRenderer = doorInputHandler.DoorSpriteRenderer;
        }

        // Same reasoning as above, but for the collider: read it straight off
        // this GameObject instead of trusting doorInputHandler.IsOpen, since
        // that property is only correct once DoorInputHandler.Awake() has run —
        // and that's not guaranteed to have happened yet at this point.
        doorCollider = GetComponent<Collider2D>();
        if (doorCollider == null && doorInputHandler != null)
        {
            doorCollider = doorInputHandler.DoorCollider;
        }

        // Sync the sprite immediately here, straight from the collider's real
        // enabled state, with zero dependency on any other script's Awake
        // having already run.
        bool isOpenAtStart = doorCollider != null && !doorCollider.enabled;
        HandleDoorOpenChanged(isOpenAtStart);
    }

    private void OnEnable()
    {
        if (doorInputHandler == null) return;

        doorInputHandler.OnDoorOpenChanged += HandleDoorOpenChanged;
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