using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    public List<Sprite> monsterSprites; // 몬스터 스프라이트 리스트

    // 난이도별 몬스터 리스트 (2차원 리스트)
    public static List<Monster>[] monsterListByRank = new List<Monster>[10];

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
        for (int i = 0; i < monsterListByRank.Length; i++)
        {
            monsterListByRank[i] = new List<Monster>();
        }

        LoadMonstersFromFile("Assets/Data/MonsterData.txt");
    }

    //  몬스터 데이터 로드 (TXT)
    private void LoadMonstersFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"몬스터 데이터 파일을 찾을 수 없습니다: {filePath}");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

            string[] data = line.Split(';');
            if (data.Length != 10) continue;

            int rank = int.Parse(data[0]) - 1;
            string name = data[1];
            string description = data[2];
            string type = data[3];
            string element = data[4];

            Stat stats = new Stat
            {
                speed = Random.Range(int.Parse(data[5].Split(',')[0]), int.Parse(data[5].Split(',')[1])),
                physicalAttack = Random.Range(int.Parse(data[6].Split(',')[0]), int.Parse(data[6].Split(',')[1])),
                magicAttack = Random.Range(int.Parse(data[7].Split(',')[0]), int.Parse(data[7].Split(',')[1])),
                health = Random.Range(int.Parse(data[8].Split(',')[0]), int.Parse(data[8].Split(',')[1])),
            };

            int spriteIndex = int.Parse(data[9]);
            Sprite appearance = spriteIndex >= 0 && spriteIndex < monsterSprites.Count ? monsterSprites[spriteIndex] : null;

            Monster newMonster = new Monster(name, description, rank + 1, type, element, stats, appearance);
            monsterListByRank[rank].Add(newMonster);
        }

        Debug.Log("몬스터 데이터 로드 완료!");
    }
}