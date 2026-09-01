using System.IO;
using UnityEngine;

public static class DiskSaveSystem
{
    private static string savePath = Application.persistentDataPath + "/playerSave.json";



    public static void SavePlayer(PlayerData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log("Game saved: " + savePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Save failed at {savePath}: {e.Message}");
        }
    }




    public static PlayerData LoadPlayer()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                if (data != null)
                {
                    return data;
                }

                Debug.LogWarning("Save file was corrupted or empty, creating new data.");
                return new PlayerData();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Load failed at {savePath}: {e.Message}. Creating new data.");
        }

        Debug.Log("No saved data found, now creating new one.");
        return new PlayerData();
    }
}