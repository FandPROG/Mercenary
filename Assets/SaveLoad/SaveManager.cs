using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string savePath; // ✅ 경로 저장 변수

    private void Awake()
    {
        savePath = Path.Combine(Application.dataPath, "Data", "SaveData"); // ✅ Awake()에서 초기화
    }

    public static void SaveGameData()
    {
        SaveRequiredData();
        SaveMercenaryData();
        Debug.Log("게임 데이터 저장 완료!");
    }

    private static void SaveRequiredData()
    {
        if (string.IsNullOrEmpty(savePath))
        {
            savePath = Application.persistentDataPath; // ✅ 안전장치 추가
        }

        string requiredDataPath = Path.Combine(savePath, "RequiredSavedata.json");

        RequiredData data = new RequiredData
        {
            day = LoadManager.Day,
            gold = LoadManager.Gold,
            difficulty = LoadManager.Difficulty,
            maxCapacity = MercenaryManager.maxCapacity
        };

        File.WriteAllText(requiredDataPath, JsonUtility.ToJson(data, true));
    }

    private static void SaveMercenaryData()
    {
        if (string.IsNullOrEmpty(savePath))
        {
            savePath = Application.persistentDataPath; // ✅ 안전장치 추가
        }

        string mercenaryDataPath = Path.Combine(savePath, "MercenarySavedata.json");

        List<MercenarySaveData> mercenaryDataList = new List<MercenarySaveData>();
        foreach (var merc in LoadManager.mercenaryList)
        {
            MercenarySaveData data = new MercenarySaveData
            {
                name = merc.name,
                lore = merc.Lore,
                fame = merc.fame,
                starRating = merc.starRating,
                job = merc.job,
                element = merc.element,
                baseStats = merc.baseStats.ToJSON(),
                equipmentStats = merc.equipmentStats.ToJSON(),
                buffStats = merc.buffStats.ToJSON(),
                weaponID = merc.equippedWeapon != null ? merc.equippedWeapon.id : -1,
                armorID = merc.equippedArmor != null ? merc.equippedArmor.id : -1,
                bootsID = merc.equippedBoots != null ? merc.equippedBoots.id : -1,
                trust = merc.trust,
                morale = merc.morale,
                energy = merc.energy,
                HP = merc.HP,
                maxMana = merc.maxMana,
                currentMana = merc.currentMana,
                spriteIndex = LoadManager.Instance.GetSpriteIndex(merc.appearance),
                skillIDs = merc.skills.ConvertAll(skill => skill.id)
            };

            mercenaryDataList.Add(data);
        }

        MercenarySaveWrapper saveWrapper = new MercenarySaveWrapper { mercenaries = mercenaryDataList };
        File.WriteAllText(mercenaryDataPath, JsonUtility.ToJson(saveWrapper, true));
    }
}

// 📌 JSON으로 저장할 현재 보유 용병 데이터 클래스
[System.Serializable]
public class MercenarySaveData
{
    public string name;
    public string lore;
    public int fame;
    public int starRating;
    public string job;
    public string element;
    public string baseStats;
    public string equipmentStats;
    public string buffStats;
    public int weaponID;
    public int armorID;
    public int bootsID;
    public int trust;
    public int morale;
    public int energy;
    public int HP;
    public int maxMana;
    public int currentMana;
    public int spriteIndex;
    public List<int> skillIDs; // 스킬 ID 리스트 저장
}

// 📌 JSON으로 여러 보유 용병을 저장할 래퍼 클래스
[System.Serializable]
public class MercenarySaveWrapper
{
    public List<MercenarySaveData> mercenaries;
}
