using UnityEngine;

public enum SkillType { Buff = 0, Debuff = 1, PhysicalAttack = 2, MagicAttack = 3 }

[System.Serializable]
public class BaseSkill
{
    public int id; // 스킬 고유 ID
    public string name; // 스킬 이름
    public string description; // 스킬 설명
    public int manaCost; // 스킬에 사용되는 마나
    public int targetCount; // 스킬 범위 (대상 수)
    public int effectIndex; // 스킬 이펙트 IDX (이펙트 프리팹 연결 예정)
    public Sprite icon; // 스킬 아이콘

    public BaseSkill(int id, string name, string description, int manaCost, int targetCount, int effectIndex, Sprite icon)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.manaCost = manaCost;
        this.targetCount = targetCount;
        this.effectIndex = effectIndex;
        this.icon = icon;
    }
}

// 📌 **버프 스킬 클래스 (스탯 증가, 지속 턴 추가)**
[System.Serializable]
public class BuffSkill : BaseSkill
{
    public Stat bonusStats; // 증가하는 스탯
    public int duration; // 📌 지속되는 턴 수 추가

    public BuffSkill(int id, string name, string description, int manaCost, int targetCount, int effectIndex, Sprite icon, Stat bonusStats, int duration)
        : base(id, name, description, manaCost, targetCount, effectIndex, icon)
    {
        this.bonusStats = bonusStats;
        this.duration = duration;
    }
}

// 📌 **디버프 스킬 클래스 (스탯 감소, 지속 턴 추가)**
[System.Serializable]
public class DebuffSkill : BaseSkill
{
    public Stat reducedStats; // 감소하는 스탯
    public int duration; // 📌 지속되는 턴 수 추가

    public DebuffSkill(int id, string name, string description, int manaCost, int targetCount, int effectIndex, Sprite icon, Stat reducedStats, int duration)
        : base(id, name, description, manaCost, targetCount, effectIndex, icon)
    {
        this.reducedStats = reducedStats;
        this.duration = duration;
    }
}

// 📌 **물리 공격 스킬 클래스**
[System.Serializable]
public class PhysicalAttackSkill : BaseSkill
{
    public float physicalAttackMultiplier; // 물리 공격 계수 (실수형)

    public PhysicalAttackSkill(int id, string name, string description, int manaCost, int targetCount, int effectIndex, Sprite icon, float physicalAttackMultiplier)
        : base(id, name, description, manaCost, targetCount, effectIndex, icon)
    {
        this.physicalAttackMultiplier = physicalAttackMultiplier;
    }
}

// 📌 **마법 공격 스킬 클래스**
[System.Serializable]
public class MagicAttackSkill : BaseSkill
{
    public float magicAttackMultiplier; // 마법 공격 계수 (실수형)

    public MagicAttackSkill(int id, string name, string description, int manaCost, int targetCount, int effectIndex, Sprite icon, float magicAttackMultiplier)
        : base(id, name, description, manaCost, targetCount, effectIndex, icon)
    {
        this.magicAttackMultiplier = magicAttackMultiplier;
    }
}
