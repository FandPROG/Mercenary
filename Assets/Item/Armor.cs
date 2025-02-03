using UnityEngine;

public class Armor : Equipment
{
    public Armor(int id, string name, string description, int rank, Stat bonusStats, Sprite icon)
        : base(id, name, description, EquipmentType.Armor, rank, bonusStats, icon) { }
}
