using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase Instance;
    public List<Sprite> skillIcons; // 스킬 아이콘 리스트 추가
    private static Dictionary<int, BaseSkill> skillDict = new Dictionary<int, BaseSkill>();

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

        LoadSkillData();
    }

    private void LoadSkillData()
    {
        string filePath = Path.Combine(Application.dataPath, "Data", "SkillData.json");
        if (!File.Exists(filePath))
        {
            Debug.LogError(" SkillData.json 파일을 찾을 수 없습니다!");
            return;
        }

        string json = File.ReadAllText(filePath);
        SkillDataWrapper skillDataWrapper = JsonUtility.FromJson<SkillDataWrapper>(json);

        foreach (SkillData data in skillDataWrapper.skills)
        {
            Sprite icon = GetSpriteByIndex(data.iconIndex); // 아이콘 불러오기
            BaseSkill skill = CreateSkillFromData(data, icon);
            if (skill != null)
            {
                skillDict[data.id] = skill;
            }
        }
        Debug.Log($"스킬 데이터 로드 완료! 총 {skillDict.Count}개");
    }

    public static BaseSkill GetSkillByID(int id)
    {
        return skillDict.ContainsKey(id) ? skillDict[id] : null;
    }

    private Sprite GetSpriteByIndex(int index)
    {
        return (index >= 0 && index < skillIcons.Count) ? skillIcons[index] : null;
    }

    private BaseSkill CreateSkillFromData(SkillData data, Sprite icon)
    {
        SkillType skillType = (SkillType)data.skillType; //숫자로 저장된 skillType을 변환

        switch (skillType)
        {
            case SkillType.Buff:
                return new BuffSkill(data.id, data.name, data.description, data.manaCost, data.targetCount, data.effectIndex, icon, Stat.FromJSON(data.statData), data.duration);
            case SkillType.Debuff:
                return new DebuffSkill(data.id, data.name, data.description, data.manaCost, data.targetCount, data.effectIndex, icon, Stat.FromJSON(data.statData), data.duration);
            case SkillType.PhysicalAttack:
                return new PhysicalAttackSkill(data.id, data.name, data.description, data.manaCost, data.targetCount, data.effectIndex, icon, data.attackMultiplier);
            case SkillType.MagicAttack:
                return new MagicAttackSkill(data.id, data.name, data.description, data.manaCost, data.targetCount, data.effectIndex, icon, data.attackMultiplier);
            case SkillType.Heal: // HP 회복 스킬 추가
                return new HealSkill(data.id, data.name, data.description, data.manaCost, data.targetCount, data.effectIndex, icon, data.healAmount);
            case SkillType.ManaHeal: // 마나 회복 스킬 추가
                return new ManaHealSkill(data.id, data.name, data.description, data.manaCost, data.targetCount, data.effectIndex, icon, data.manaRestoreAmount);
            default:
                Debug.LogError($"⚠️ 알 수 없는 스킬 타입: {data.skillType}");
                return null;
        }
    }
}

// JSON에서 불러올 스킬 데이터 클래스
[System.Serializable]
public class SkillDataWrapper
{
    public List<SkillData> skills;
}

[System.Serializable]
public class SkillData
{
    public int id;
    public string name;
    public string description;
    public int manaCost;
    public int targetCount;
    public int effectIndex;
    public int skillType; // 숫자로 저장된 스킬 타입 (0~5)
    public string statData; // Buff/Debuff에 사용됨
    public float attackMultiplier; // 물리/마법 공격 스킬에 사용됨
    public int duration; // 버프/디버프의 지속 턴 추가
    public int iconIndex; // 스킬 아이콘의 인덱스 추가
    public int healAmount; // HP 회복 스킬에 사용
    public int manaRestoreAmount; // 마나 회복 스킬에 사용
}
