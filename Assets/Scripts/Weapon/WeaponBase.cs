using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
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

        Total
    }

    protected TypeOfDamage eTypeOfDamage;
    protected int BaseDamage;

    protected int Proficiency;
    protected int ProficiencyOwned;
    protected int ProficiencyExp;
    protected int ProficiencyRequired;
    // Tree

    public enum WeaponType
    {
        Swords = 0,
        Bow,
        Hammer,
        TotalTypes
    }

    protected WeaponType eWeaponType;

    protected int Level;

    // BonusStats

    protected List<GameObject> HitList = new List<GameObject>();

    public int GetDamage()
    {
        int returnDamage;

        returnDamage = BaseDamage;

        return returnDamage;
    }

    public void ResetHits()
    {
        HitList.Clear();
    }

    protected bool HitEntity(GameObject target)
    {
        if (!HitList.Contains(target))
        {
            HitList.Add(target);
            return true;
        }
        return false;
    }
}
