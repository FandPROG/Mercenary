using UnityEngine;

public class Monster
{
    public string name;  // 몬스터 이름
    public string description; // 몬스터 설명
    public int rank;  // 몬스터 난이도 (1~10)
    public string type; // 몬스터 타입 (일반 / 보스)
    public string element; // 속성 (불, 물, 자연, 번개 등)
    
    public Stat stats;  // 몬스터 기본 스탯
    public Sprite appearance;  // 몬스터 외형

    // 생성자
    public Monster(string name, string description, int rank, string type, string element, Stat stats, Sprite appearance)
    {
        this.name = name;
        this.description = description;
        this.rank = Mathf.Clamp(rank, 1, 10);
        this.type = type;
        this.element = element;
        this.stats = stats;
        this.appearance = appearance;
    }
}
