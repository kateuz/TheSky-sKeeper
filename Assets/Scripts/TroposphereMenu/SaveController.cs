using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class SaveController : MonoBehaviour
{

    private string saveLocation;
    private InventoryController inventoryController;
    void Start()
    {
        saveLocation= Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();

        LoadGame();
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Killian");
        if (player == null)
        {
            Debug.LogWarning("Player not found in scene!");
            return;
        }

        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();
        if (confiner == null || confiner.m_BoundingShape2D == null)
        {
            Debug.LogWarning("CinemachineConfiner or its collider not found!");
            return;
        }

        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Killian").transform.position,
            //mapBoundary = FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems()
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
        Debug.Log("Save successful at: " + saveLocation);

    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject.FindGameObjectWithTag("Killian").transform.position = saveData.playerPosition;

            //FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
        }
        else
        {
            SaveGame();
        }
    }
}
