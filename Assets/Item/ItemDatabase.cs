using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;
    public List<Sprite> itemIcons; // 아이템 아이콘 리스트
    private static Dictionary<int, BaseItem> itemDict = new Dictionary<int, BaseItem>();

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

        LoadItemData();
    }

    private void LoadItemData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "ItemData.json");
        if (!File.Exists(filePath))
        {
            Debug.LogError("ItemData.json 파일을 찾을 수 없습니다!");
            return;
        }

        string json = File.ReadAllText(filePath);
        ItemDataWrapper itemDataWrapper = JsonUtility.FromJson<ItemDataWrapper>(json);

        foreach (ItemData data in itemDataWrapper.items)
        {
            BaseItem item = CreateItemFromData(data);
            if (item != null)
            {
                item.icon = GetSpriteByID(data.iconIndex);
                itemDict[item.id] = item;
            }
        }
        Debug.Log($"✅ 아이템 데이터 로드 완료! 총 {itemDict.Count}개");
    }

    public static BaseItem GetItemByID(int id)
    {
        return itemDict.ContainsKey(id) ? itemDict[id] : null;
    }

    private Sprite GetSpriteByID(int id)
    {
        return id >= 0 && id < itemIcons.Count ? itemIcons[id] : null;
    }

    private static BaseItem CreateItemFromData(ItemData data)
    {
        ItemType itemType = (ItemType)data.itemType;

        switch (itemType)
        {
            case ItemType.Battle:
                return new BattleItem(data.id, data.name, data.description, null, Stat.FromJSON(data.bonusStats), data.duration);
            case ItemType.Consumable:
                return new ConsumableItem(data.id, data.name, data.description, null, Stat.FromJSON(data.increaseStats));
            case ItemType.Material:
                return new MaterialItem(data.id, data.name, data.description, null);
            default:
                Debug.LogError($"⚠️ 알 수 없는 아이템 타입: {data.itemType}");
                return null;
        }
    }
}

[System.Serializable]
public class ItemDataWrapper
{
    public List<ItemData> items;
}

[System.Serializable]
public class ItemData
{
    public int id;
    public string name;
    public string description;
    public int itemType;
    public string bonusStats; 
    public string increaseStats;
    public int iconIndex;
    public int duration;
}
