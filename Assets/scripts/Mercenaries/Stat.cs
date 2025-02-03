using UnityEngine;

[System.Serializable]
public struct Stat
{
    public int speed;
    public int physicalAttack;
    public int magicAttack;
    public float criticalChance;
    public float criticalDamage;
    public float criticalResistance;
    public int health;
    public int magicResistance;
    public int physicalResistance;
    public float effectResistance;
    public float effectAccuracy;

    // 📌 스탯 합산 연산자 오버로딩 (기초 + 장비 + 버프)
    public static Stat operator +(Stat a, Stat b)
    {
        return new Stat
        {
            speed = a.speed + b.speed,
            physicalAttack = a.physicalAttack + b.physicalAttack,
            magicAttack = a.magicAttack + b.magicAttack,
            criticalChance = a.criticalChance + b.criticalChance,
            criticalDamage = a.criticalDamage + b.criticalDamage,
            criticalResistance = a.criticalResistance + b.criticalResistance,
            health = a.health + b.health,
            magicResistance = a.magicResistance + b.magicResistance,
            physicalResistance = a.physicalResistance + b.physicalResistance,
            effectResistance = a.effectResistance + b.effectResistance,
            effectAccuracy = a.effectAccuracy + b.effectAccuracy
        };
    }
    // 📌 0이 아닌 스탯만 필터링하여 문자열로 변환
    public string ToFilteredString()
    {
        string result = "";
        if (speed != 0) result += $"속도: {speed}, ";
        if (physicalAttack != 0) result += $"물리 공격력: {physicalAttack}, ";
        if (magicAttack != 0) result += $"마법 공격력: {magicAttack}, ";
        if (criticalChance != 0) result += $"치명타 확률: {criticalChance * 100}%, ";
        if (criticalDamage != 0) result += $"치명타 데미지: {criticalDamage * 100}%, ";
        if (criticalResistance != 0) result += $"치명타 저항: {criticalResistance * 100}%, ";
        if (health != 0) result += $"체력: {health}, ";
        if (magicResistance != 0) result += $"마법 저항: {magicResistance}, ";
        if (physicalResistance != 0) result += $"물리 저항: {physicalResistance}, ";
        if (effectResistance != 0) result += $"효과 저항: {effectResistance * 100}%, ";
        if (effectAccuracy != 0) result += $"효과 적중: {effectAccuracy * 100}%, ";

        return result.TrimEnd(',', ' '); // 마지막 콤마 제거
    }

    // 📌 Stat 객체를 JSON 문자열로 변환
    public string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }

    // 📌 JSON 문자열에서 Stat 객체로 변환
    public static Stat FromJSON(string json)
    {
        return JsonUtility.FromJson<Stat>(json);
    }
}
