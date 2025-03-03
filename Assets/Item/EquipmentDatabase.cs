using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EquipmentDatabase : MonoBehaviour
{
    public static EquipmentDatabase Instance;
    public List<Sprite> equipmentIcons; // 장비 아이콘 리스트
    private static Dictionary<int, Equipment> equipmentDict = new Dictionary<int, Equipment>();

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

        LoadEquipmentData();
    }

    private void LoadEquipmentData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "EquipmentData.json");
        if (!File.Exists(filePath))
        {
            Debug.LogError("EquipmentData.json 파일을 찾을 수 없습니다!");
            return;
        }

        string json = File.ReadAllText(filePath);
        EquipmentDataWrapper equipmentDataWrapper = JsonUtility.FromJson<EquipmentDataWrapper>(json);

        foreach (EquipmentData data in equipmentDataWrapper.equipments)
        {
            Equipment equipment = new Equipment(
                data.id,
                data.name,
                data.description,
                (EquipmentType)System.Enum.Parse(typeof(EquipmentType), data.equipmentType),
                data.starRating,
                Stat.FromJSON(data.bonusStats),
                GetSpriteByID(data.iconIndex)
            );

            equipmentDict[data.id] = equipment;
        }
        Debug.Log($"장비 데이터 로드 완료! 총 {equipmentDict.Count}개");
    }

    public static Equipment GetEquipmentByID(int id)
    {
        return equipmentDict.ContainsKey(id) ? equipmentDict[id] : null;
    }

    private Sprite GetSpriteByID(int id)
    {
        return id >= 0 && id < equipmentIcons.Count ? equipmentIcons[id] : null;
    }
}

// JSON 데이터를 담는 클래스
[System.Serializable]
public class EquipmentDataWrapper
{
    public List<EquipmentData> equipments;
}

[System.Serializable]
public class EquipmentData
{
    public int id;
    public string name;
    public string description;
    public string equipmentType;
    public int starRating;
    public string bonusStats; // Stat을 JSON 문자열로 저장
    public int iconIndex;
}
