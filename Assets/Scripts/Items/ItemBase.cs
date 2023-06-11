using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStats
{
    public enum Stats
    {
        AttackFlat,
        AttackPercentage,

        MaxHealth,

        IncreasedAfflictions,

        DefenceFlat,
        DefencePercentage,
        FireDefencePercentage,
        IceDefencePercentage,
        NaturalDefencePercentage,
        LightningDefencePercentage,
        PhysicalDefencePercentage,
        DarkDefencePercentage,
        LightDefencePercentage,

        ResistancePercentage,
        FireResistancePercentage,
        IceResistancePercentage,
        NaturalResistancePercentage,
        LightningResistancePercentage,
        PhysicalResistancePercentage,
        DarkResistancePercentage,
        LightResistancePercentage,

        None,

        Total
    }

    public Stats Type;
    public float Value;

    public CStats(Stats statType, float value)
    {
        Type = statType;
        Value = value;
    }
}

public class ItemBase : MonoBehaviour
{
    public enum TypeOfDamage
    {
        Physical = 0,
        Fire,
        Ice,
        Lightning,
        Natural,
        Dark,
        Light,
        TotalTypes
    }

    public enum TypeOfItems
    {
        Consumable = 0,
        NotConsumable,
        Total
    }

    public enum ItemID
    {
        Sword = 0,
        Helmet,
        Apple,
        Total
    }

    public enum WeaponType
    {
        Sword = 0,
        Spear,
        Bow,
        Total
    }

    // Item Stuff
    protected string ItemName;
    public ItemID itemID;
    public int MaxCount;
    public int Count;

    // Weapon Stuff
    protected int BaseDamage;
    protected WeaponType eWeaponType;
    protected int Level;

    // Equipment Stuff

    public int GetDamage()
    {
        int returnDamage;

        returnDamage = BaseDamage;

        return returnDamage;
    }

    public virtual void ResetHits()
    {
        
    }

    public virtual bool HitEntity(GameObject target)
    {
        return false;
    }
}
