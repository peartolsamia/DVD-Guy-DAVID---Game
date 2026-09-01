using System;

[System.Serializable]
public class PlayerData
{
    // --- DEFAULTS ---
    public const float DefaultMaxHealth = 3f;
    public const float DefaultSpeed = 5f;

    // --- EVENTS ---
    public event Action OnHealthMaxChanged;
    public event Action OnSpeedDefaultChanged;
    public event Action OnSkillPointsChanged;






    // --- HEALTH ---
    public float health_Max = DefaultMaxHealth;

    // --- SPEED ---
    public float speed_Default = DefaultSpeed;

    // --- SKILL POINTS ---
    public int skillPoints_Storage;








    // --- DATA MANIPULATION ---

    // --- HEALTH ---
    public void UpgradeMaxHealth(float amount)
    {
        health_Max += amount;
        OnHealthMaxChanged?.Invoke();
    }

    // --- SPEED ---
    public void ChangeDefaultSpeed(float multiplier)
    {
        speed_Default *= multiplier;
        OnSpeedDefaultChanged?.Invoke();
    }

    // --- SKILL POINTS ---
    public void ChangeStorageSkillPoints(int amount)
    {
        skillPoints_Storage += amount;
        OnSkillPointsChanged?.Invoke();
    }
}