using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // --- SAVE / LOAD ---
    public void SaveData(PlayerData data)
    {
        if (data != null)
        {
            DiskSaveSystem.SavePlayer(data);
        }
    }

    public PlayerData LoadData()
    {
        return DiskSaveSystem.LoadPlayer();
    }
}