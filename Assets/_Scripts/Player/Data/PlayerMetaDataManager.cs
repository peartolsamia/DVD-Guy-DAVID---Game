// ----------- OTHER SCRIPTS REACHES AND UPDATES PLAYER DATA VIA THIS MANAGER ----------
// -------------------------------------------------------------------------------------


using UnityEngine;


[DefaultExecutionOrder(-100)]
public class PlayerMetaDataManager : MonoBehaviour
{
    public static PlayerMetaDataManager Instance { get; private set; }

    public PlayerData Data { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            LoadMetaData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMetaData()
    {
        if (SaveManager.Instance != null)
        {
            Data = SaveManager.Instance.LoadData();
        }
        else
        {
            Data = DiskSaveSystem.LoadPlayer();
        }
    }

    public void SaveMetaData()
    {
        if (SaveManager.Instance != null && Data != null)
        {
            SaveManager.Instance.SaveData(Data);
        }
    }

    private void OnApplicationQuit() => SaveMetaData();
    private void OnApplicationPause(bool pause) { if (pause) SaveMetaData(); }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}