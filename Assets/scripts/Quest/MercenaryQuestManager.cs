using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MercenaryQuestManager : MonoBehaviour
{
    public static MercenaryQuestManager Instance;
    
    // 진행 중인 퀘스트 리스트
    public static List<MercenaryQuest> activeQuests = new List<MercenaryQuest>();

    // 난이도별 퀘스트 리스트
    public static List<MercenaryQuest>[] mercenaryQuestList = new List<MercenaryQuest>[9];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 2차원 리스트 초기화
        for (int i = 0; i < mercenaryQuestList.Length; i++)
        {
            mercenaryQuestList[i] = new List<MercenaryQuest>();
        }

        LoadQuestsFromFile("Assets/Data/MercenaryQuests.txt");
    }

    // 텍스트 파일에서 퀘스트 데이터 로드
    private void LoadQuestsFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"퀘스트 파일을 찾을 수 없습니다: {filePath}");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue; // 빈 줄 또는 주석(#) 제외

            string[] data = line.Split(';');
            if (data.Length != 7) continue; // 데이터 개수가 맞지 않으면 무시

            MercenaryQuest.QuestRank rank = (MercenaryQuest.QuestRank)System.Enum.Parse(typeof(MercenaryQuest.QuestRank), data[0]);
            string name = data[1];
            MercenaryQuest.QuestType type = (MercenaryQuest.QuestType)System.Enum.Parse(typeof(MercenaryQuest.QuestType), data[2]);
            string description = data[3];
            int rewardGold = int.Parse(data[4]);
            string rewardArmor = data[5];
            string rewardWeapon = data[6];

            mercenaryQuestList[(int)rank].Add(new MercenaryQuest(rank, name, type, description, rewardGold, rewardArmor, rewardWeapon));
        }
        Debug.Log("퀘스트 데이터 로드 완료!");
    }

    // 현재 용병단 등급에 맞는 퀘스트 5개 반환
    public List<MercenaryQuest> GetAvailableQuests(int mercenaryRank)
    {
        if (mercenaryRank < 0 || mercenaryRank >= mercenaryQuestList.Length)
            return new List<MercenaryQuest>();

        List<MercenaryQuest> quests = mercenaryQuestList[mercenaryRank];
        int count = Mathf.Min(quests.Count, 5);
        return quests.GetRange(0, count);
    }
}
