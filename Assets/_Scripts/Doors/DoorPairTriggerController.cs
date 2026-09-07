using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the room-transition door pair: when the player passes through this
/// door, it opens (so the player can walk through) and, after a short delay,
/// the paired door in the connected room closes behind them.
///
/// Collider state is delegated entirely to DoorInputHandler.SetDoorOpen(),
/// so this never races against DoorManager's input-driven door toggling.
/// </summary>
public class DoorPairTriggerController2D : MonoBehaviour
{
    [Header("Return door in connected room")]
    [SerializeField] private DoorPairTriggerController2D pairedObject;

    [Header("Shared door-state handler (source of truth for this door's collider)")]
    [SerializeField] private DoorInputHandler doorInputHandler;

    [Header("Delay before the paired door closes behind the player")]
    [SerializeField] private float closeDelay = 0.4f;

    private void Awake()
    {
        if (doorInputHandler == null)
        {
            doorInputHandler = GetComponent<DoorInputHandler>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HandleTriggerSequence());
        }
    }

    private IEnumerator HandleTriggerSequence()
    {
        if (doorInputHandler != null)
        {
            doorInputHandler.SetDoorOpen(true);
        }

        yield return new WaitForSeconds(closeDelay);

        if (pairedObject != null && pairedObject.doorInputHandler != null)
        {
            pairedObject.doorInputHandler.SetDoorOpen(false);
        }
    }
}