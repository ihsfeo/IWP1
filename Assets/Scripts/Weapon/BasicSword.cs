using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : WeaponBase
{
    // Start is called before the first frame update
    void Awake()
    {
        eTypeOfDamage = TypeOfDamage.Physical;
        BaseDamage = 10;
        Proficiency = 0;
        ProficiencyExp = 0;
        ProficiencyOwned = 0;
        ProficiencyRequired = 100; // Temp Number
        eWeaponType = WeaponType.Swords;
        Level = 1;
        // Tree
        // BonusStats
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
