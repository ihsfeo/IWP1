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
        FireDefenceFlat,
        FireDefencePercentage,
        IceDefenceFlat,
        IceDefencePercentage,
        NaturalDefenceFlat,
        NaturalDefencePercentage,
        LightningDefenceFlat,
        LightningDefencePercentage,
        PhysicalDefenceFlat,
        PhysicalDefencePercentage,
        DarkDefenceFlat,
        DarkDefencePercentage,
        LightDefenceFlat,
        LightDefencePercentage,

        ResistanceFlat,
        ResistancePercentage,
        FireResistanceFlat,
        FireResistancePercentage,
        IceResistanceFlat,
        IceResistancePercentage,
        NaturalResistanceFlat,
        NaturalResistancePercentage,
        LightningResistanceFlat,
        LightningResistancePercentage,
        PhysicalResistanceFlat,
        PhysicalResistancePercentage,
        DarkResistanceFlat,
        DarkResistancePercentage,
        LightResistanceFlat,
        LightResistancePercentage,

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
        Fire = 0,
        Ice,
        Natural,
        Lightning,
        Physical,
        Dark,
        Light,
        TotalTypes
    }

    public enum ItemRarity
    {
        Common = 0,
        Uncommon,
        Rare,
        total
    }

    public enum TypeOfItems
    {
        Helmet,
        Chestplate,
        Legging,
        Shoe,
        Weapon,
        Consumable,
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
    public ItemRarity Rarity;
    public int MaxCount;
    public int Count;
    public TypeOfItems ItemType;

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
