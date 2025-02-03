using UnityEngine;

public class Weapon : Equipment
{
    public Weapon(int id, string name, string description, int rank, Stat bonusStats, Sprite icon)
        : base(id, name, description, EquipmentType.Weapon, rank, bonusStats, icon) { }
}
