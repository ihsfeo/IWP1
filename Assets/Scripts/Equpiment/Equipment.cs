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
        ItemName = EquipmentValues.Name;
        Stats.Add(new CStats(EquipmentValues.PrimaryStat, EquipmentValues.PrimaryValue));
    }

    public List<CStats> GetStats()
    {
        return Stats;
    }
}
