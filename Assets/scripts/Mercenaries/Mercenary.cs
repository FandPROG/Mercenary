using UnityEngine;
using System.Collections.Generic;

public class Mercenary
{
    public string name;
    public string Lore; // 설명(용병에 대한 간단한 스토리)
    public int fame; // 명성도
    public int starRating; // 등급 (1~10성)
    public string job; // 직업
    public string element; // 속성 (불, 물, 자연 등)

    // 기본, 장비, 버프 스탯
    public Stat baseStats;
    public Stat equipmentStats;
    public Stat buffStats;

    // 장착한 장비
    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public Boots equippedBoots;

    // 기타 스탯
    public int trust; // 신뢰도
    public int morale; // 사기
    public int energy; // 에너지 (최대값 필요)
    public int HP; // 현재 HP상태
    public int maxMana; // 최대 마나
    public int currentMana; // 현재 마나

    // 용병 외형 (Sprite 추가)
    public Sprite appearance;

    // 보유 스킬 리스트 (최대 5개)
    public List<BaseSkill> skills = new List<BaseSkill>();

    // 생성자
    public Mercenary(string name, string Lore, int fame, int star, string job, string element, Stat baseStats, Sprite appearance, int maxMana)
    {
        this.name = name;
        this.Lore = Lore;
        this.fame = fame;
        this.starRating = Mathf.Clamp(star, 1, 10);
        this.job = job;
        this.element = element;
        this.baseStats = baseStats;
        this.equipmentStats = new Stat();
        this.buffStats = new Stat();
        this.equippedWeapon = null;
        this.equippedArmor = null;
        this.equippedBoots = null;
        this.trust = 50;
        this.morale = 50;
        this.energy = 100;
        this.HP = this.equipmentStats.health;
        this.appearance = appearance;
        this.maxMana = maxMana;
        this.currentMana = maxMana; // 시작 시 최대 마나로 설정
    }

    // 최종 스탯 계산 (기본 + 장비 + 버프)
    public Stat GetTotalStats()
    {
        Stat totalStats = baseStats + equipmentStats + buffStats;

        if (equippedWeapon != null)
            totalStats += equippedWeapon.bonusStats;
        if (equippedArmor != null)
            totalStats += equippedArmor.bonusStats;
        if (equippedBoots != null)
            totalStats += equippedBoots.bonusStats;

        return totalStats;
    }

    // 장비 장착
    public void EquipWeapon(Weapon weapon)
    {
        this.equippedWeapon = weapon;
    }

    public void EquipArmor(Armor armor)
    {
        this.equippedArmor = armor;
    }

    public void EquipBoots(Boots boots)
    {
        this.equippedBoots = boots;
    }

    // 스킬 추가 (최대 5개)
    public bool LearnSkill(int skillID)
    {
        if (skills.Count >= 5)
        {
            Debug.Log($"{name}의 보유 스킬이 최대(5개)입니다!");
            return false;
        }

        BaseSkill skill = SkillDatabase.GetSkillByID(skillID);
        if (skill == null)
        {
            Debug.LogError($"스킬 ID {skillID}를 찾을 수 없습니다!");
            return false;
        }

        skills.Add(skill);
        Debug.Log($"{name}이(가) {skill.name} 스킬을 배웠습니다!");
        return true;
    }

    // 스킬 사용 (마나 소모)
    public bool UseSkill(BaseSkill skill)
    {
        if (!skills.Contains(skill))
        {
            Debug.Log($"{name}은(는) {skill.name} 스킬을 가지고 있지 않습니다!");
            return false;
        }
        if (currentMana < skill.manaCost)
        {
            Debug.Log($"{name}의 마나가 부족합니다! (현재 마나: {currentMana} / 필요 마나: {skill.manaCost})");
            return false;
        }

        currentMana -= skill.manaCost; // 마나 차감
        Debug.Log($"{name}이(가) {skill.name} 스킬을 사용했습니다! (남은 마나: {currentMana})");
        return true;
    }

    // 마나 회복
    public void RecoverMana(int amount)
    {
        currentMana = Mathf.Min(maxMana, currentMana + amount);
        Debug.Log($"{name}의 마나가 {amount} 회복되었습니다! (현재 마나: {currentMana}/{maxMana})");
    }
}
