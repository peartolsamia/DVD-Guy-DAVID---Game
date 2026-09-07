using System;
using UnityEngine;

public class PlayerSkillPoints : MonoBehaviour
{
    public event Action OnSkillPointsChanged;
    public int Current { get; private set; }
    // Can access current skill points amount in other scripts with; Player.Instance.SkillPoints.Current

    private void Start()
    {
        ResetForNewRun();

        // TEMP DEBUG - REMOVE AFTER TEST
        //InvokeRepeating(nameof(LogCurrentSkillPoints), 1f, 1f);
    }


    /*
    // TEMP DEBUG - REMOVE AFTER TEST
    private void LogCurrentSkillPoints()
    {
        Debug.Log($"[PlayerSkillPoints] Current: {Current}");
    }
    */


    public bool CanSpend(int amount)
    {
        return amount > 0 && Current >= amount;
    }

    public void EarnSkillPoints(int amount)
    {
        if (amount <= 0) return;

        Current += amount;
        OnSkillPointsChanged?.Invoke();
    }

    public bool SpendSkillPoints(int amount)
    {
        if (!CanSpend(amount)) return false;

        Current -= amount;
        OnSkillPointsChanged?.Invoke();
        return true;
    }


    public void ResetForNewRun()
    {
        Current = 0;
        OnSkillPointsChanged?.Invoke();
    }

    // Deposit all in hand current skill points to storage at the end of the run
    public void DepositToStorage()
    {
        if (Current > 0 && PlayerMetaDataManager.Instance != null && PlayerMetaDataManager.Instance.Data != null)
        {
            PlayerMetaDataManager.Instance.Data.ChangeStorageSkillPoints(Current);
        }

        Current = 0;
        OnSkillPointsChanged?.Invoke();
    }
}