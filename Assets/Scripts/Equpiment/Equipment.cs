using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : ItemBase
{
    public EquipmentSO EquipmentValues;

    List<CStats> Stats = new List<CStats>();

    private void Awake()
    {
        Level = 1;
        itemID = EquipmentValues.itemID;
        Rarity = EquipmentValues.itemRarity;
        ItemName = EquipmentValues.Name;
        for (int i = 0; i < EquipmentValues.Stats.Count; i++)
            Stats.Add(new CStats(EquipmentValues.Stats[i], EquipmentValues.StatValues[i]));
    }

    public List<CStats> GetStats()
    {
        return Stats;
    }
}
