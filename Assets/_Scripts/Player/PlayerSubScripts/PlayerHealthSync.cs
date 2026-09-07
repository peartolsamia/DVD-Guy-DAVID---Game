using UnityEngine;

/// <summary>
/// Wires the player's PlayerMetaDataManager max-health value into a plain Health
/// component. Add this alongside Health on the player object only — enemies just
/// use Health by itself and never need this script.
/// </summary>
[RequireComponent(typeof(Health))]
public class PlayerHealthSync : MonoBehaviour
{
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Start()
    {
        ApplyMaxHealth(fullHeal: true);
    }

    private void OnEnable()
    {
        if (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            PlayerMetaDataManager.Instance.Data.OnHealthMaxChanged += OnHealthMaxChanged;
        }
    }

    private void OnDisable()
    {
        if (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            PlayerMetaDataManager.Instance.Data.OnHealthMaxChanged -= OnHealthMaxChanged;
        }
    }

    private void OnHealthMaxChanged()
    {
        ApplyMaxHealth(fullHeal: false);
    }

    private void ApplyMaxHealth(bool fullHeal)
    {
        float newMax = (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
            ? PlayerMetaDataManager.Instance.Data.health_Max
            : PlayerData.DefaultMaxHealth;

        health.SetMaxHealth(newMax, fullHeal);
    }
}