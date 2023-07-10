using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected Status status;

    protected int Shield;
    protected int ShieldMax;

    protected int BaseDamage;

    protected int Level;

    protected ItemBase.TypeOfDamage ShieldType;
    protected List<ItemBase.TypeOfDamage> ShieldWeakness = new List<ItemBase.TypeOfDamage>();
    public enum EnemyState
    {
        Idle,
        Attacking,
        Dead,
        Chase
    }
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
    protected EnemyState eEnemyState;


    int HasStatusAilment(ItemBase.TypeOfDamage Element)
    {
        for (int i = 0; i < status.StatusAilmentsList.Count; i++)
        {
            if (status.StatusAilmentsList[i].TypeOfAilment == Element)
                return i;
        }

        return -1;
    }

    public int TakeDamage(ItemBase.TypeOfDamage Element, int damage, bool AdditionalHit = false)
    {
        // Damage Calculation
        {
            if (Element == ItemBase.TypeOfDamage.Fire)
            {
                int index;
                if ((index = HasStatusAilment(ItemBase.TypeOfDamage.Fire)) != -1)
                {
                    damage = (int)((float)damage * (1f + (float)status.StatusAilmentsList[index].Level / 10));
                }
            }

            int FinalDamage = damage * (1 - (int)status.GetStat(CStats.Stats.FireDefencePercentage + 2 * (int)Element) / 100); // % Defence
            FinalDamage -= (int)status.GetStat(CStats.Stats.FireDefenceFlat + 2 * (int)Element); // Flat Defence
            if (FinalDamage <= 0)
                FinalDamage = 1;

            status.Health -= FinalDamage;
            if (status.Health <= 0)
                return ((int)eEnemyType + 1) * ((int)eEnemyVariation + 1) * Level;
        }

        // Frozen
        if (status.IsFrozen)
        {
            status.IsFrozen = false;
            TakeDamage(ItemBase.TypeOfDamage.Ice, damage * 2);
        }

        if (Element == ItemBase.TypeOfDamage.Lightning && !AdditionalHit)
        {
            int index;
            if ((index = HasStatusAilment(ItemBase.TypeOfDamage.Lightning)) != -1)
            {
                switch (status.StatusAilmentsList[index].Level)
                {
                    case 1: 
                        if (Random.Range(0, 10) <= 1)
                        {
                            // Chain
                        }
                        break;
                    case 2:
                        if (Random.Range(0, 5) <= 1)
                        {
                            // Chain
                        }
                        break;
                    case 3:
                        // Chain
                        break;
                    default: break;
                }
            }
        }

        int AilmentIndex = HasStatusAilment(Element);
        if (AilmentIndex != -1)
        {
            status.StatusAilmentsList[AilmentIndex].IncreaseAdv(5);
        }
        else
        {
            StatusAilment statusAilment;
            switch (Element)
            {
                case ItemBase.TypeOfDamage.Physical:
                    statusAilment = new PhysicalAilment();
                    statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                    statusAilment.Adv = 5;
                    statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(0);
                    status.StatusAilmentsList.Add(statusAilment);
                    break;
                case ItemBase.TypeOfDamage.Fire:
                    statusAilment = new FireAilment();
                    statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                    statusAilment.Adv = 5;
                    statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(0);
                    status.StatusAilmentsList.Add(statusAilment);
                    break;
                case ItemBase.TypeOfDamage.Ice:
                    statusAilment = new IceAilment();
                    statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                    statusAilment.Adv = 5;
                    statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(0);
                    status.StatusAilmentsList.Add(statusAilment);
                    break;
                case ItemBase.TypeOfDamage.Natural:
                    statusAilment = new NaturalAilment();
                    statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                    statusAilment.Adv = 5;
                    statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(0);
                    status.StatusAilmentsList.Add(statusAilment);
                    break;
                case ItemBase.TypeOfDamage.Lightning:
                    statusAilment = new LightningAilment();
                    statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                    statusAilment.Adv = 5;
                    statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(0);
                    status.StatusAilmentsList.Add(statusAilment);
                    break;
                default:
                    break;
            }
        }

        return 1;
    }

    public virtual void Reset()
    {
        status.Health = (int)status.GetStat(CStats.Stats.MaxHealth);
        eEnemyState = EnemyState.Idle;
    }
}
