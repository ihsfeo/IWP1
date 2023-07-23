using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : ItemBase
{
    public EquipmentSO EquipmentValues;

    public override void Init()
    {
        Level = 1;
        itemID = EquipmentValues.itemID;
        Rarity = EquipmentValues.itemRarity;
        ItemName = EquipmentValues.Name;
        Description = EquipmentValues.Description;
        for (int i = 0; i < EquipmentValues.Stats.Count; i++)
            cStats.Add(new CStats(EquipmentValues.Stats[i], EquipmentValues.StatValues[i]));
    }

    public List<CStats> GetStats()
    {
        return cStats;
    }
}
