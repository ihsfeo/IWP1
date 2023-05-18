using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected int Shield;
    protected int ShieldMax;

    protected int Health;
    protected int HealthMax;

    protected int BaseDamage;

    protected List<StatusAilment> StatusAilmentsList;
    protected WeaponBase.TypeOfDamage ShieldType;
    protected List<WeaponBase.TypeOfDamage> ShieldWeakness;

    public enum EnemyType
    {
        Normal = 0,
        Elite,
        Boss,
        Total
    }

    public enum EnemyMovementType
    {
        Normal = 0,
        Flying,
        Worm,
        Special,
        Total
    }

    public enum EnemyVariation
    {
        Basic,
        Total
    }

    protected EnemyType eEnemyType;
    protected EnemyMovementType eEnemyMovementType;
    protected EnemyVariation eEnemyVariation;
}
