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

        savePath = Path.Combine(Application.dataPath, "Data", "SaveData", "InventoryData.json");
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

    public Dictionary<int, int> GetAllOwnedItems()
    {
        return new Dictionary<int, int>(ownedItems); // 보유 아이템 목록을 복사하여 반환
    }

    private void LoadInventory()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogError($"❌ InventoryData.json 파일을 찾을 수 없습니다! 경로: {savePath}");
            return;
        }

        string json = File.ReadAllText(savePath);
        Debug.Log($"📜 불러온 JSON 데이터: {json}");

        InventoryDataWrapper data = JsonUtility.FromJson<InventoryDataWrapper>(json);
        if (data == null || data.itemIds == null || data.itemCounts == null)
        {
            Debug.LogError("❌ InventoryData.json이 비어 있거나 잘못된 형식입니다.");
            return;
        }

        ownedItems = data.ToDictionary();

        Debug.Log($"✅ 인벤토리 데이터 로드 완료! 총 {ownedItems.Count}개 아이템 보유");
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
