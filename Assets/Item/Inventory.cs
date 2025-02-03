using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private static Dictionary<int, int> ownedItems = new Dictionary<int, int>(); // {아이템 ID, 개수}

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        savePath = Path.Combine(Application.persistentDataPath, "InventoryData.json");
        LoadInventory();
    }

    public void AddItem(int itemId, int amount)
    {
        if (ownedItems.ContainsKey(itemId))
        {
            ownedItems[itemId] += amount;
        }
        else
        {
            ownedItems[itemId] = amount;
        }
        SaveInventory();
    }

    public void RemoveItem(int itemId, int amount)
    {
        if (ownedItems.ContainsKey(itemId))
        {
            ownedItems[itemId] -= amount;
            if (ownedItems[itemId] <= 0) ownedItems.Remove(itemId);
        }
        SaveInventory();
    }

    public int GetItemCount(int itemId)
    {
        return ownedItems.ContainsKey(itemId) ? ownedItems[itemId] : 0;
    }

    private void SaveInventory()
    {
        string json = JsonUtility.ToJson(new InventoryDataWrapper(ownedItems));
        File.WriteAllText(savePath, json);
    }

    private void LoadInventory()
    {
        if (!File.Exists(savePath)) return;
        string json = File.ReadAllText(savePath);
        InventoryDataWrapper data = JsonUtility.FromJson<InventoryDataWrapper>(json);
        ownedItems = data.ToDictionary();
    }
}

[System.Serializable]
public class InventoryDataWrapper
{
    public List<int> itemIds;
    public List<int> itemCounts;

    public InventoryDataWrapper(Dictionary<int, int> items)
    {
        itemIds = new List<int>(items.Keys);
        itemCounts = new List<int>(items.Values);
    }

    public Dictionary<int, int> ToDictionary()
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();
        for (int i = 0; i < itemIds.Count; i++)
        {
            dict[itemIds[i]] = itemCounts[i];
        }
        return dict;
    }
}
