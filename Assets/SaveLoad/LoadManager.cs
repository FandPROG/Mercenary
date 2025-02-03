using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;
    public static int Day = 1;
    public static int Gold = 100;
    public static int Difficulty = 1;
    public static List<Mercenary> mercenaryList = new List<Mercenary>();

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

        savePath = Path.Combine(Application.dataPath, "Data", "SaveData");

        LoadRequiredData();
        LoadMercenaryData();
    }

    private void LoadRequiredData()
    {
        string requiredDataPath = Path.Combine(savePath, "RequiredSavedata.json");
        if (!File.Exists(requiredDataPath)) return;

        string json = File.ReadAllText(requiredDataPath);
        RequiredData data = JsonUtility.FromJson<RequiredData>(json);

        Day = data.day;
        Gold = data.gold;
        Difficulty = data.difficulty;
        MercenaryManager.maxCapacity = data.maxCapacity; // 📌 maxCapacity 불러오기

        Debug.Log($"기본 데이터 로드 완료: Day {Day}, Gold {Gold}, Difficulty {Difficulty}, maxCapacity {MercenaryManager.maxCapacity}");
    }

    private void LoadMercenaryData()
    {
        string mercenaryDataPath = Path.Combine(savePath, "MercenarySavedata.json");
        if (!File.Exists(mercenaryDataPath))
        {
            Debug.LogError($"❌ 용병 데이터 파일이 존재하지 않습니다: {mercenaryDataPath}");
            return;
        }

        string json = File.ReadAllText(mercenaryDataPath);
        Debug.Log($"📜 불러온 JSON 데이터: {json}"); // JSON 데이터 출력

        MercenarySaveWrapper mercenaryDataWrapper = JsonUtility.FromJson<MercenarySaveWrapper>(json);
        if (mercenaryDataWrapper == null || mercenaryDataWrapper.mercenaries == null)
        {
            Debug.LogError("❌ mercenaryDataWrapper 또는 mercenaryDataWrapper.mercenaries가 null입니다.");
            return;
        }

        mercenaryList.Clear(); // 기존 리스트 초기화

        foreach (MercenarySaveData data in mercenaryDataWrapper.mercenaries)
        {
            Debug.Log($"✅ 불러온 용병: {data.name}, 스탯(체력): {data.baseStats}");

            Stat baseStats = Stat.FromJSON(data.baseStats);
            Stat equipmentStats = Stat.FromJSON(data.equipmentStats);
            Stat buffStats = Stat.FromJSON(data.buffStats);

            Sprite appearance = GetSpriteByIndex(data.spriteIndex);

            Mercenary merc = new Mercenary(data.name, data.lore, data.fame, data.starRating, data.job, data.element, baseStats, appearance, data.maxMana)
            {
                equipmentStats = equipmentStats,
                buffStats = buffStats,
                trust = data.trust,
                morale = data.morale,
                energy = data.energy,
                HP = data.HP,
                currentMana = data.currentMana,
                equippedWeapon = LoadWeaponByID(data.weaponID),
                equippedArmor = LoadArmorByID(data.armorID),
                equippedBoots = LoadBootsByID(data.bootsID)
            };

            // 📌 스킬 불러오기
            if (data.skillIDs != null)
            {
                foreach (int skillID in data.skillIDs)
                {
                    BaseSkill skill = SkillDatabase.GetSkillByID(skillID);
                    if (skill != null)
                    {
                        merc.skills.Add(skill);
                    }
                    else
                    {
                        Debug.LogWarning($"⚠️ 스킬 ID {skillID}를 찾을 수 없습니다.");
                    }
                }
            }

            mercenaryList.Add(merc);
        }

        Debug.Log($"✅ 용병 데이터 로드 완료! 보유 용병 수: {mercenaryList.Count}");
    }



    public int GetSpriteIndex(Sprite sprite)
    {
        if (sprite == null) return -1;
        return MercenaryManager.Instance.mercenarySprites.IndexOf(sprite);
    }

    public Sprite GetSpriteByIndex(int index)
    {
        if (index >= 0 && index < MercenaryManager.Instance.mercenarySprites.Count)
        {
            return MercenaryManager.Instance.mercenarySprites[index];
        }
        return null;
    }

    private Weapon LoadWeaponByID(int id)
    {
        Equipment equipment = EquipmentDatabase.GetEquipmentByID(id);
        return equipment is Weapon ? (Weapon)equipment : null;
    }

    private Armor LoadArmorByID(int id)
    {
        Equipment equipment = EquipmentDatabase.GetEquipmentByID(id);
        return equipment is Armor ? (Armor)equipment : null;
    }

    private Boots LoadBootsByID(int id)
    {
        Equipment equipment = EquipmentDatabase.GetEquipmentByID(id);
        return equipment is Boots ? (Boots)equipment : null;
    }
}

// 📌 JSON으로 저장된 기본 데이터 클래스 (여기에 추가)
[System.Serializable]
public class RequiredData
{
    public int day;
    public int gold;
    public int difficulty;
    public int maxCapacity;
}

