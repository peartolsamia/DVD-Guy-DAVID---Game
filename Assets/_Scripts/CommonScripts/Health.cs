using UnityEngine;

/// <summary>
/// Generic health component. Attach this to the player, enemies, destructibles, etc.
/// It has no dependency on any external system (like PlayerMetaDataManager) — anything
/// that needs to drive max health from outside (stats, upgrades, difficulty scaling)
/// should call SetMaxHealth() from its own script. See PlayerHealthSync for an example
/// of wiring this up to PlayerMetaDataManager for the player.
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsDead { get; private set; }
    public float HealthPercent => MaxHealth > 0f ? CurrentHealth / MaxHealth : 0f;

    public event System.Action<float, float> OnDamaged;   // (damage, currentHealth)
    public event System.Action<float, float> OnHealed;    // (amount, currentHealth)
    public event System.Action OnDeath;
    public event System.Action OnRevived;
    public event System.Action<float> OnMaxHealthChanged; // (newMaxHealth)

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    /// <summary>
    /// Changes max health at runtime (e.g. level-up, difficulty scaling, upgrades).
    /// By default keeps current health proportionally clamped; pass fullHeal to top it off.
    /// </summary>
    public void SetMaxHealth(float newMaxHealth, bool fullHeal = false)
    {
        maxHealth = Mathf.Max(0f, newMaxHealth);
        CurrentHealth = fullHeal ? maxHealth : Mathf.Clamp(CurrentHealth, 0f, maxHealth);
        OnMaxHealthChanged?.Invoke(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0f) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        OnDamaged?.Invoke(damage, CurrentHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f) return;

        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        OnHealed?.Invoke(amount, CurrentHealth);
    }

    private void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    public void Revive(bool fullHeal = true)
    {
        IsDead = false;
        CurrentHealth = fullHeal ? maxHealth : CurrentHealth;
        OnRevived?.Invoke();
    }
}