using UnityEngine;

public enum ItemType { Battle, Consumable, Material }

[System.Serializable]
public class BaseItem
{
    public int id; // 아이템 고유 ID
    public string name; // 아이템 이름
    public string description; // 아이템 설명
    public int quantity; // 보유 개수
    public Sprite icon; // 아이템 아이콘

    public BaseItem(int id, string name, string description, int quantity, Sprite icon)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.quantity = quantity;
        this.icon = icon;
    }

    // 📌 JSON 변환 메서드 추가
    public virtual string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public static BaseItem FromJSON(string json)
    {
        return JsonUtility.FromJson<BaseItem>(json);
    }
}

// 📌 전투 아이템 (전투 중 사용, 일정 턴 동안 스탯 증가)
[System.Serializable]
public class BattleItem : BaseItem
{
    public Stat bonusStats; // 전투 시 증가하는 스탯
    public int duration; // 지속되는 턴 수

    public BattleItem(int id, string name, string description, int quantity, Sprite icon, Stat bonusStats, int duration)
        : base(id, name, description, quantity, icon)
    {
        this.bonusStats = bonusStats;
        this.duration = duration;
    }

    public override string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public static BattleItem FromJSON(string json)
    {
        return JsonUtility.FromJson<BattleItem>(json);
    }
}

// 📌 소비 아이템 (전투 외 특정 용병의 스탯을 회복하거나 증가)
[System.Serializable]
public class ConsumableItem : BaseItem
{
    public Stat increaseStats; // 증가하는 스탯

    public ConsumableItem(int id, string name, string description, int quantity, Sprite icon, Stat increaseStats)
        : base(id, name, description, quantity, icon)
    {
        this.increaseStats = increaseStats;
    }

    public override string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public static ConsumableItem FromJSON(string json)
    {
        return JsonUtility.FromJson<ConsumableItem>(json);
    }
}

// 📌 재료 아이템 (퀘스트, 장비 강화, 용병 훈련 재료)
[System.Serializable]
public class MaterialItem : BaseItem
{
    public MaterialItem(int id, string name, string description, int quantity, Sprite icon)
        : base(id, name, description, quantity, icon) { }

    public override string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public static MaterialItem FromJSON(string json)
    {
        return JsonUtility.FromJson<MaterialItem>(json);
    }
}
