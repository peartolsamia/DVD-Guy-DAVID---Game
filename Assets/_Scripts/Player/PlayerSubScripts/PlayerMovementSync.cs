using UnityEngine;

/// <summary>
/// Wires player-specific concerns into a plain BouncingMovement component:
///  - syncs base speed from PlayerMetaDataManager
///  - registers bounces with PlayerBounceRewardHandler
/// Add this alongside BouncingMovement on the player object only — enemies and
/// projectiles just use BouncingMovement by itself and never need this script.
/// </summary>
[RequireComponent(typeof(BouncingMovement))]
public class PlayerMovementSync : MonoBehaviour
{
    private BouncingMovement movement;

    private void Awake()
    {
        movement = GetComponent<BouncingMovement>();
    }

    private void Start()
    {
        RefreshBaseSpeed();
    }

    private void OnEnable()
    {
        if (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            PlayerMetaDataManager.Instance.Data.OnSpeedDefaultChanged += RefreshBaseSpeed;
        }

        movement.OnBounce += HandleBounce;
    }

    private void OnDisable()
    {
        if (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            PlayerMetaDataManager.Instance.Data.OnSpeedDefaultChanged -= RefreshBaseSpeed;
        }

        movement.OnBounce -= HandleBounce;
    }

    private void RefreshBaseSpeed()
    {
        float newSpeed = (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
            ? PlayerMetaDataManager.Instance.Data.speed_Default
            : PlayerData.DefaultSpeed;

        movement.SetBaseSpeed(newSpeed);
    }

    private void HandleBounce(Vector2 newDirection)
    {
        Player.Instance?.BounceReward?.RegisterBounce();
    }
}