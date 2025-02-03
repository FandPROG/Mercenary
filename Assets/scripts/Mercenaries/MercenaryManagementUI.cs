using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MercenaryManagementUI : MonoBehaviour
{
    public GameObject mercenarySlotPrefab; // 용병 패널 프리팹
    public Transform contentPanel; // Scroll View의 Content
    public GameObject detailPanel; // 용병 상세 정보 패널
    public Image detailImage; // 용병 스프라이트
    public TextMeshProUGUI detailStatsText_name; // 용병 상세 스탯들들
    public TextMeshProUGUI detailStatsText_lore;
    public TextMeshProUGUI detailStatsText_star;
    public TextMeshProUGUI detailStatsText_EJETM;
    public TextMeshProUGUI detailStatsText;
    public Button equipWeaponButton, equipArmorButton, dismissButton, closeDetailButton;

    public GameObject weaponPanel, armorPanel; // 무기/갑옷 장착 패널
    public Button backButton, upgradeCapacityButton;
    private Mercenary selectedMercenary;

    // 📌 스킬 UI 패널 관련 변수 추가
public GameObject skillPanel; // 스킬 패널
public Image skillIcon;
public TextMeshProUGUI skillName;
public TextMeshProUGUI skillDescription;
public TextMeshProUGUI skillManaCost;
public TextMeshProUGUI skillTargetCount;
public TextMeshProUGUI skillType;
public TextMeshProUGUI skillExtraInfo;
public Button skillPrevButton, skillNextButton, closeSkillButton;
public Button skillButton; // 스킬 패널을 여는 버튼

private int currentSkillIndex = 0; // 현재 페이지 인덱스
private List<BaseSkill> currentSkills; // 현재 용병의 보유 스킬 리스트


    private void Start()
    {
        backButton.onClick.AddListener(ReturnToGameScene);
        upgradeCapacityButton.onClick.AddListener(UpgradeCapacity);
        closeDetailButton.onClick.AddListener(CloseDetailPanel);

        equipWeaponButton.onClick.AddListener(OpenWeaponPanel);
        equipArmorButton.onClick.AddListener(OpenArmorPanel);
        dismissButton.onClick.AddListener(DismissMercenary);
        // 📌 스킬 버튼 클릭 시 `SkillPanel`을 열도록 설정
        skillButton.onClick.AddListener(OpenSkillPanel);
        closeSkillButton.onClick.AddListener(CloseSkillPanel);
        skillPrevButton.onClick.AddListener(ShowPreviousSkill);
        skillNextButton.onClick.AddListener(ShowNextSkill);

        skillPanel.SetActive(false); // 시작 시 스킬 패널 비활성화


        weaponPanel.SetActive(false);
        armorPanel.SetActive(false);
        detailPanel.SetActive(false);

        DisplayMercenaryList();
    }

    private void DisplayMercenaryList()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < MercenaryManager.maxCapacity; i++)
        {
            GameObject slot = Instantiate(mercenarySlotPrefab, contentPanel);
            Image mercenaryImage = slot.transform.Find("MercenaryImage").GetComponent<Image>();
            TextMeshProUGUI mercenaryInfoText = slot.transform.Find("MercenaryInfoText").GetComponent<TextMeshProUGUI>();
            Button detailButton = slot.transform.Find("DetailButton").GetComponent<Button>();

            if (i < LoadManager.mercenaryList.Count)
            {
                Mercenary merc = LoadManager.mercenaryList[i];
                mercenaryImage.sprite = merc.appearance;
                mercenaryInfoText.text = $"{merc.name} ({merc.starRating}성)\n{merc.job} / {merc.element}";
                detailButton.onClick.AddListener(() => ShowDetail(merc));
            }
            else
            {
                mercenaryImage.enabled = false;
                mercenaryInfoText.text = "Empty";
                detailButton.interactable = false;
            }
        }
    }

    private void ShowDetail(Mercenary mercenary)
    {
        selectedMercenary = mercenary;
        detailPanel.SetActive(true);
        detailImage.sprite = mercenary.appearance;
        detailStatsText_name.text = $"Name : {mercenary.name}";
        detailStatsText_lore.text = $"{mercenary.Lore}";
        detailStatsText_star.text = $"{mercenary.starRating}★";
        detailStatsText_EJETM.text = $"{mercenary.job}({mercenary.element})   명성 : {mercenary.fame}\n" +
                               $"에너지 : {mercenary.energy} 신뢰도 : {mercenary.trust} 사기 : {mercenary.morale}";
        detailStatsText.text = $"체력: {mercenary.baseStats.health} 속도: {mercenary.baseStats.speed}\n물리atk: {mercenary.baseStats.physicalAttack} 마법atk: {mercenary.baseStats.magicAttack}\n" +
                               $"마법 저항: {mercenary.baseStats.magicResistance} 물리 저항: {mercenary.baseStats.physicalResistance}\n" +
                               $"치명타 확률: {mercenary.baseStats.criticalChance} 치명타 저항: {mercenary.baseStats.criticalResistance}\n" +
                               $"효과 적중: {mercenary.baseStats.magicAttack} 효과 저항: {mercenary.baseStats.health}";

        dismissButton.interactable = LoadManager.mercenaryList.Count > 1; // 최소 1명 유지
        currentSkills = mercenary.skills;
        currentSkillIndex = 0;

        // 📌 스킬 버튼 활성화 여부 결정 (스킬이 없는 경우 비활성화)
        skillButton.interactable = (currentSkills != null && currentSkills.Count > 0);
    }
    private void CloseDetailPanel()
    {
        detailPanel.SetActive(false);
    }

    private void OpenWeaponPanel()
    {
        weaponPanel.SetActive(true);
    }

    private void OpenArmorPanel()
    {
        armorPanel.SetActive(true);
    }

    private void DismissMercenary()
    {
        if (selectedMercenary != null && LoadManager.mercenaryList.Count > 1)
        {
            LoadManager.mercenaryList.Remove(selectedMercenary);
            CloseDetailPanel();
            DisplayMercenaryList();
        }
    }

    private void ReturnToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void UpgradeCapacity()
    {
        MercenaryManager.maxCapacity += 1;
        DisplayMercenaryList();
    }

    private void OpenSkillPanel()
    {
        if (currentSkills == null || currentSkills.Count == 0)
        {
            Debug.LogWarning("⚠️ 이 용병은 스킬이 없습니다.");
            return;
        }

        skillPanel.SetActive(true);
        ShowSkill(currentSkillIndex);
    }

    private void CloseSkillPanel()
    {
        skillPanel.SetActive(false);
    }
    private void ShowSkill(int index)
    {
        if (currentSkills == null || currentSkills.Count == 0) return;

        BaseSkill skill = currentSkills[index];

        skillIcon.sprite = skill.icon;
        skillName.text = skill.name;
        skillDescription.text = skill.description;
        skillManaCost.text = $"마나 비용: {skill.manaCost}";
        skillTargetCount.text = $"타겟 수: {skill.targetCount}";
        skillType.text = $"스킬 유형: {skill.GetType().Name}";

        // 📌 스킬 유형별 추가 정보 설정 (0이 아닌 값만 표시)
        if (skill is BuffSkill buffSkill)
        {
            string statInfo = buffSkill.bonusStats.ToFilteredString();
            skillExtraInfo.text = $"💡 버프 스킬\n지속 시간: {buffSkill.duration}턴\n" + (string.IsNullOrEmpty(statInfo) ? "증가 스탯 없음" : $"증가 스탯: {statInfo}");
        }
        else if (skill is DebuffSkill debuffSkill)
        {
            string statInfo = debuffSkill.reducedStats.ToFilteredString();
            skillExtraInfo.text = $"💀 디버프 스킬\n지속 시간: {debuffSkill.duration}턴\n" + (string.IsNullOrEmpty(statInfo) ? "감소 스탯 없음" : $"감소 스탯: {statInfo}");
        }
        else if (skill is PhysicalAttackSkill physicalAttackSkill)
        {
            skillExtraInfo.text = $"⚔️ 물리 공격\n공격 배율: {physicalAttackSkill.physicalAttackMultiplier}배";
        }
        else if (skill is MagicAttackSkill magicAttackSkill)
        {
            skillExtraInfo.text = $"🔮 마법 공격\n공격 배율: {magicAttackSkill.magicAttackMultiplier}배";
        }
        else
        {
            skillExtraInfo.text = "❓ 알 수 없는 스킬 유형";
        }

        // 📌 이전/다음 버튼 활성화 여부 설정
        skillPrevButton.interactable = (index > 0);
        skillNextButton.interactable = (index < currentSkills.Count - 1);
    }


    private void ShowPreviousSkill()
    {
        if (currentSkillIndex > 0)
        {
            currentSkillIndex--;
            ShowSkill(currentSkillIndex);
        }
    }

    private void ShowNextSkill()
    {
        if (currentSkillIndex < currentSkills.Count - 1)
        {
            currentSkillIndex++;
            ShowSkill(currentSkillIndex);
        }
    }
}
