using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy1 : EnemyBase
{
    private void Awake()
    {
        ShieldMax = 100;
        Shield = ShieldMax;
        ShieldType = WeaponBase.TypeOfDamage.Physical;
        // ShieldWeakness ?

        eEnemyType = EnemyType.Normal;
        eEnemyVariation = EnemyVariation.Basic;
        eEnemyMovementType = EnemyMovementType.Normal;

        HealthMax = 100;
        Health = HealthMax;
        BaseDamage = 5;
    }

    private void Update()
    {
        
    }
}
