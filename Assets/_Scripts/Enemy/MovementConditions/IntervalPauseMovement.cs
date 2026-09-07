using UnityEngine;

/// <summary>
/// EXAMPLE / TEMPLATE — not wired into any prefab yet.
///
/// Shows the pattern for enemy-specific movement behaviors: a small standalone
/// component that only calls the public API of BouncingMovement (Pause/Resume/
/// SetSpeedMultiplier). Add a script like this to whichever enemy prefab needs the
/// behavior, no changes to BouncingMovement required. Different enemy types can mix
/// and match different combinations of these components.
///
/// This one makes the object alternate between moving and stopping at fixed intervals.
/// </summary>
[RequireComponent(typeof(BouncingMovement))]
public class IntervalPauseMovement : MonoBehaviour
{
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float pauseDuration = 1f;

    private BouncingMovement movement;
    private float timer;
    private bool isPaused;

    private void Awake()
    {
        movement = GetComponent<BouncingMovement>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (!isPaused && timer >= moveDuration)
        {
            movement.Pause();
            isPaused = true;
            timer = 0f;
        }
        else if (isPaused && timer >= pauseDuration)
        {
            movement.Resume();
            isPaused = false;
            timer = 0f;
        }
    }
}