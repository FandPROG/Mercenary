using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MercenaryManager : MonoBehaviour
{
    public static MercenaryManager Instance;
    public static int maxCapacity = 10; // 기본 최대 용병 수
    public List<Sprite> mercenarySprites; // 용병 외형 리스트

    // 등급별 용병 데이터 (2차원 리스트)
    public static List<Mercenary>[] mercenaryListByRank = new List<Mercenary>[10];
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 📌 2차원 리스트 초기화 (모든 등급을 비어 있는 리스트로 초기화)
        for (int i = 0; i < mercenaryListByRank.Length; i++)
        {
            mercenaryListByRank[i] = new List<Mercenary>();
        }

        LoadMercenariesFromJson();
    }

    // 📌 JSON 파일에서 용병 데이터 불러오기
    private void LoadMercenariesFromJson()
    {
        string filePath = Path.Combine(Application.dataPath, "Data", "MercenaryData.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("❌ MercenaryData.json 파일을 찾을 수 없습니다!");
            return;
        }

        string json = File.ReadAllText(filePath);
        MercenaryDataWrapper mercenaryDataWrapper = JsonUtility.FromJson<MercenaryDataWrapper>(json);

        foreach (MercenaryData data in mercenaryDataWrapper.mercenaries)
        {
            if (data.rank < 1 || data.rank > 10)
            {
                Debug.LogWarning($"⚠️ 잘못된 용병 등급: {data.rank}");
                continue;
            }

            int rankIndex = data.rank - 1; // JSON은 1부터 시작, 리스트는 0부터 시작

            Stat baseStats = Stat.FromJSON(data.statData); // 📌 JSON에서 Stat 변환

            int spriteIndex = data.spriteIndex;
            Sprite appearance = (spriteIndex >= 0 && spriteIndex < mercenarySprites.Count)
                ? mercenarySprites[spriteIndex]
                : Resources.Load<Sprite>("DefaultSprite"); // 기본 스프라이트 적용

            int maxMana = 100; // 📌 용병마다 최대 마나 설정

            Mercenary newMercenary = new Mercenary(data.name, data.lore, data.fame, data.rank, data.job, data.element, baseStats, appearance, maxMana)
            {
                currentMana = maxMana,
                skills = new List<BaseSkill>() // 📌 스킬을 비워둠
            };

            // 📌 스킬 불러오기 (추가된 부분)
            if (data.skillIDs != null)
            {
                foreach (int skillID in data.skillIDs)
                {
                    BaseSkill skill = SkillDatabase.GetSkillByID(skillID);
                    if (skill != null)
                    {
                        newMercenary.skills.Add(skill);
                    }
                    else
                    {
                        Debug.LogWarning($"⚠️ 스킬 ID {skillID}를 찾을 수 없습니다.");
                    }
                }
            }

            mercenaryListByRank[rankIndex].Add(newMercenary);
        }
        Debug.Log($"✅ 용병 데이터 로드 완료! 총 {mercenaryDataWrapper.mercenaries.Count}개");
    }


    // 📌 용병 고용 (등급 범위 내에서 랜덤 선택)
    public Mercenary HireMercenary(int minRank, int maxRank)
    {
        int randomRank = Random.Range(minRank, maxRank + 1);

        if (randomRank < 0 || randomRank >= mercenaryListByRank.Length || mercenaryListByRank[randomRank].Count == 0)
        {
            Debug.LogError($"등급 {randomRank + 1}의 용병 데이터가 없습니다! 고용 불가능.");
            return null;
        }

        Mercenary selectedMercenary = mercenaryListByRank[randomRank][Random.Range(0, mercenaryListByRank[randomRank].Count)];

        if (selectedMercenary == null)
        {
            Debug.LogError($"랜덤 선택된 용병이 null입니다! 고용 실패.");
            return null;
        }

        selectedMercenary.skills = new List<BaseSkill>(); // 📌 고용 시 스킬 목록 비우기
        LoadManager.mercenaryList.Add(selectedMercenary);

        Debug.Log($"새 용병 고용: {selectedMercenary.name} ({selectedMercenary.starRating}성, {selectedMercenary.job}, {selectedMercenary.element})");
        return selectedMercenary;
    }

    // 📌 길거리 고용 (1~3성)
    public Mercenary HireStreetMercenary()
    {
        return HireMercenary(0, 2);
    }

    // 📌 용병 길드 방문 (2~5성)
    public Mercenary HireGuildMercenary()
    {
        return HireMercenary(1, 4);
    }

    // 📌 기사 영입 (3~7성)
    public Mercenary HireKnightMercenary()
    {
        return HireMercenary(2, 6);
    }
}

// 📌 JSON 데이터를 담는 클래스
[System.Serializable]
public class MercenaryDataWrapper
{
    public List<MercenaryData> mercenaries;
}

[System.Serializable]
public class MercenaryData
{
    public int rank;
    public string name;
    public string lore;
    public int fame;
    public string job;
    public string element;
    public string statData; // 📌 Stat을 JSON 문자열로 저장
    public int spriteIndex;
    public List<int> skillIDs; // 📌 용병의 스킬 ID 리스트 추가
}

