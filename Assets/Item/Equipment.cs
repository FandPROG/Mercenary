using UnityEngine;

public enum EquipmentType { Weapon, Armor, Boots }

[System.Serializable]
public class Equipment
{
    public int id; // 장비 고유 ID
    public string name; // 장비 이름
    public string description; // 장비 설명
    public EquipmentType equipmentType; // 장비 유형 (무기, 갑옷, 신발)
    public int starRating; // 등급 (1~10성)
    public Stat bonusStats; // 추가 스탯
    public Sprite icon; // 장비 아이콘

    public Equipment(int id, string name, string description, EquipmentType equipmentType, int starRating, Stat bonusStats, Sprite icon)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.equipmentType = equipmentType;
        this.starRating = starRating;
        this.bonusStats = bonusStats;
        this.icon = icon;
    }

    // 📌 JSON 변환 메서드 추가
    public string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public static Equipment FromJSON(string json)
    {
        return JsonUtility.FromJson<Equipment>(json);
    }
}
