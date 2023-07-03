using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProficiency : MonoBehaviour
{
    public int Proficiency;
    public int ProficiencyOwned;
    public int ProficiencyExp;
    public int ProficiencyRequired;

    ItemBase.ItemRarity Rarity;

    public WeaponProficiency(ItemBase.ItemRarity rarity)
    {
        Rarity = rarity;
        ProficiencyRequired = GetProficiencyRequired();
    }

    public void IncreaseProficiency(int exp = 1)
    {
        if (Proficiency >= 10)
            return;

        ProficiencyExp += exp;
        if (ProficiencyExp >= ProficiencyRequired)
        {
            ProficiencyExp -= ProficiencyRequired;
            Proficiency++;
            if (Proficiency < 10)
                ProficiencyRequired = GetProficiencyRequired();
        }
    }

    public int GetProficiencyRequired()
    {
        return (Proficiency + 100) * (Proficiency + 1) * (Proficiency + 1) * (int)(Rarity + 1);
    }
}
