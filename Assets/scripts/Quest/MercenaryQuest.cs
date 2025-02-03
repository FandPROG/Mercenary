using System;

public class MercenaryQuest
{
    public enum QuestRank { 우드, 쿠퍼, 아이언, 실버, 골드, 백금, 다이아몬드, 루비, 블랙 }
    public enum QuestType { 전투, 파견, 조건 }

    public QuestRank rank;      // 퀘스트 등급
    public string questName;    // 퀘스트 이름
    public QuestType type;      // 퀘스트 유형
    public string description;  // 퀘스트 내용
    public int rewardGold;      // 보상 골드 (없으면 0)
    public string rewardArmor;  // 보상 갑옷 (없으면 "None")
    public string rewardWeapon; // 보상 무기 (없으면 "None")
    public bool isCompleted;    // 클리어 여부

    public MercenaryQuest(QuestRank rank, string questName, QuestType type, string description, int rewardGold, string rewardArmor, string rewardWeapon)
    {
        this.rank = rank;
        this.questName = questName;
        this.type = type;
        this.description = description;
        this.rewardGold = rewardGold;
        this.rewardArmor = rewardArmor;
        this.rewardWeapon = rewardWeapon;
        this.isCompleted = false;
    }
}