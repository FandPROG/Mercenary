using UnityEngine;

public class Boots : Equipment
{
    public Boots(int id, string name, string description, int rank, Stat bonusStats, Sprite icon)
        : base(id, name, description, EquipmentType.Boots, rank, bonusStats, icon) { }
}
