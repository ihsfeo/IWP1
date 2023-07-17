using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    // States
    public bool InWater = false;
    public bool InLava = false;
    public bool IsFrozen = false;
    public bool IsShocked = false;

    // Stats
    public int Health;
    private List<CStats> cStats = new List<CStats>();
    public List<StatusAilment> StatusAilmentsList = new List<StatusAilment>();

    // Additional Stats
    public float MovementSpeed = 1;
    public float KnockbackResistance = 0; // Based on Hit location and hitter location, resistance is based on division

    public void InitStats()
    {
        for (int i = 0; i < (int)CStats.Stats.Total; i++)
        {
            CStats temp = new CStats((CStats.Stats)i, 0);
            cStats.Add(temp);
        }
    }

    public void IncludeStatsOf(ItemBase item)
    {
        if (!item)
            return;
        
        List<CStats> stats;

        switch (item.ItemType)
        {
            case ItemBase.TypeOfItems.Helmet: 
            case ItemBase.TypeOfItems.Chestplate:
            case ItemBase.TypeOfItems.Legging:
            case ItemBase.TypeOfItems.Shoe:
                stats = item.GetComponent<Equipment>().GetStats();
                break;
            case ItemBase.TypeOfItems.Weapon:
            default: return;
        }

        for (int i = 0; i < stats.Count; i++)
        {
            cStats[(int)stats[i].Type].Value += stats[i].Value;
            //switch (stats[i].Type)
            //{
            //    case CStats.Stats.AttackFlat:
            //    case CStats.Stats.AttackPercentage:
            //    case CStats.Stats.MaxHealth:
            //    case CStats.Stats.IncreasedAfflictions:
            //    case CStats.Stats.DefenceFlat:
            //    case CStats.Stats.DefencePercentage:
            //    case CStats.Stats.FireDefencePercentage:
            //    case CStats.Stats.IceDefencePercentage:
            //    case CStats.Stats.NaturalDefencePercentage:
            //    case CStats.Stats.LightningDefencePercentage:
            //    case CStats.Stats.PhysicalDefencePercentage:
            //    case CStats.Stats.DarkDefencePercentage:
            //    case CStats.Stats.LightDefencePercentage:
            //    case CStats.Stats.ResistancePercentage:
            //    case CStats.Stats.FireResistancePercentage:
            //    case CStats.Stats.IceResistancePercentage:
            //    case CStats.Stats.NaturalResistancePercentage:
            //    case CStats.Stats.LightningResistancePercentage:
            //    case CStats.Stats.PhysicalResistancePercentage:
            //    case CStats.Stats.DarkResistancePercentage:
            //    case CStats.Stats.LightResistancePercentage:
            //    default: return;
            //}
        }
    }

    public float GetStat(CStats.Stats type)
    {
        for (int i = 0; i < cStats.Count; i++)
        {
            if (cStats[i].Type == type)
                return cStats[i].Value;
        }
        return -1;
    }

    public void SetMaxHealth(int num)
    {
        for (int i = 0; i < cStats.Count; i++)
        {
            if (cStats[i].Type == CStats.Stats.MaxHealth)
                cStats[i].Value = num;
        }
    }
}
