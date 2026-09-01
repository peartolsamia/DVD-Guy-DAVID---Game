using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public bool IsDead { get; private set; }

    private void Start()
    {
        RefreshMaxHealth();
        CurrentHealth = MaxHealth;
    }

    private void OnEnable()
    {
        if (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            PlayerMetaDataManager.Instance.Data.OnHealthMaxChanged += RefreshMaxHealth;
        }
    }

    private void OnDisable()
    {
        if (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            PlayerMetaDataManager.Instance.Data.OnHealthMaxChanged -= RefreshMaxHealth;
        }
    }

    public void RefreshMaxHealth()
    {
        if (PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            MaxHealth = PlayerMetaDataManager.Instance.Data.health_Max;
        }
        else
        {
            MaxHealth = PlayerData.DefaultMaxHealth;
        }

        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);
    }

    private void Die()
    {
        IsDead = true;

        Debug.Log("Player died. Returning to base...");

        // Animation
        // Open Game Over Menu
        // Load Base Scene
    }

    public void Revive()
    {
        IsDead = false;
        CurrentHealth = MaxHealth;
    }
}