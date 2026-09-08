using System;
using UnityEngine;

/// <summary>
/// Attached to individual door objects.
/// This is the SINGLE SOURCE OF TRUTH for whether a door is open or closed.
/// Registers/unregisters the door instance with the central DoorManager.
///
/// IMPORTANT: no other script should ever write to doorCollider.enabled directly.
/// Always go through SetDoorOpen() / ToggleDoor() here, so the room-transition
/// pairing logic (DoorPairTriggerController2D) and the player-input logic
/// (DoorManager) can never race over the same collider state.
///
/// This component only owns STATE. It knows nothing about how that state is
/// visualized (sprite alpha, animation, etc.) — it just announces changes via
/// OnDoorOpenChanged so other components (e.g. DoorVisualFeedback) can react.
/// </summary>
public class DoorInputHandler : MonoBehaviour
{
    [SerializeField] private DoorType doorType;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private SpriteRenderer doorSpriteRenderer;

    public DoorType DoorType => doorType;
    public Collider2D DoorCollider => doorCollider;
    public SpriteRenderer DoorSpriteRenderer => doorSpriteRenderer;

    /// <summary>Fired whenever the door's open/closed state changes, with the new state.</summary>
    public event Action<bool> OnDoorOpenChanged;

    /// <summary>True = door is open (collider disabled, player can pass through).</summary>
    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (doorCollider == null)
        {
            doorCollider = GetComponent<Collider2D>();
        }

        if (doorSpriteRenderer == null)
        {
            doorSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Seed IsOpen from whatever state the collider was left in (e.g. set in the editor),
        // so the source of truth starts consistent with the actual scene state.
        IsOpen = doorCollider != null && !doorCollider.enabled;
    }

    private void Start()
    {
        // Registration happens in Start, not Awake: Unity guarantees every object's
        // Awake() has already run before any object's Start() runs, so DoorManager.Instance
        // is guaranteed to be set here. Registering in Awake() instead is a race condition —
        // if this door's Awake() happens to run before DoorManager's Awake(), Instance would
        // still be null, RegisterDoor would silently be skipped (due to ?.), and this door
        // would be permanently invisible to input with no error ever being thrown.
        DoorManager.Instance?.RegisterDoor(this);
    }

    private void OnDestroy()
    {
        DoorManager.Instance?.UnregisterDoor(this);
    }

    /// <summary>
    /// The ONLY method allowed to write to doorCollider.enabled.
    /// Both room-transition logic and input logic must call this instead of
    /// touching the collider themselves.
    /// </summary>
    public void SetDoorOpen(bool open)
    {
        IsOpen = open;

        if (doorCollider != null)
        {
            doorCollider.enabled = !open;
        }

        OnDoorOpenChanged?.Invoke(open);
    }

    public void ToggleDoor()
    {
        SetDoorOpen(!IsOpen);
    }
}