using UnityEngine;

/// <summary>
/// Gradually reduces the speed of an object (bullet or enemy) over time
/// and destroys (or disables) it when the speed hits zero.
/// Utilizes the SetSpeedMultiplier method of BouncingMovement.
/// </summary>
[RequireComponent(typeof(BouncingMovement))]
public class SlowdownAndDespawn : MonoBehaviour
{
    [Header("Slowdown Settings")]
    [Tooltip("Total duration (in seconds) over which the object slows down before despawning.")]
    [SerializeField] private float lifetime = 3f;

    [Tooltip("If true, the GameObject is destroyed when time expires; if false, it is set inactive.")]
    [SerializeField] private bool destroyOnFinish = true;

    [Header("Optional Curve")]
    [Tooltip("Optional custom curve to define the deceleration style. Falls back to linear if left empty.")]
    [SerializeField] private AnimationCurve customDecelerationCurve = AnimationCurve.Linear(0, 1, 1, 0);

    private BouncingMovement movement;
    private float elapsedTime;

    private void Awake()
    {
        movement = GetComponent<BouncingMovement>();
    }

    private void Update()
    {
        if (elapsedTime < lifetime)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / lifetime);

            // Use AnimationCurve if assigned and valid, otherwise fallback to linear interpolation
            float multiplier = (customDecelerationCurve != null && customDecelerationCurve.length > 0)
                ? customDecelerationCurve.Evaluate(normalizedTime)
                : Mathf.Lerp(1f, 0f, normalizedTime);

            movement.SetSpeedMultiplier(multiplier);

            if (elapsedTime >= lifetime)
            {
                OnFinish();
            }
        }
    }

    private void OnFinish()
    {
        if (destroyOnFinish)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Resets the timer and restores movement speed multiplier to 1.
    /// Call this when recycling the object via Object Pooling.
    /// </summary>
    public void ResetSlowdown()
    {
        elapsedTime = 0f;
        if (movement != null)
        {
            movement.SetSpeedMultiplier(1f);
        }
    }
}