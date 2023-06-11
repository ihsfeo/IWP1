using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected Status status;

    protected int Shield;
    protected int ShieldMax;

    protected int BaseDamage;

    protected List<StatusAilment> StatusAilmentsList = new List<StatusAilment>();
    protected ItemBase.TypeOfDamage ShieldType;
    protected List<ItemBase.TypeOfDamage> ShieldWeakness = new List<ItemBase.TypeOfDamage>();

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

    int HasStatusAilment(ItemBase.TypeOfDamage Element)
    {
        for (int i = 0; i < StatusAilmentsList.Count; i++)
        {
            if (StatusAilmentsList[i].TypeOfAilment == Element)
                return i;
        }

        return -1;
    }

    public void TakeDamage(ItemBase.TypeOfDamage Element, int damage)
    {
        int FinalDamage = damage;// * PDefences[(int)Element] / 100; // % Defence
        //FinalDamage -= FDefences[(int)Element]; // Flat Defence
        if (FinalDamage <= 0)
            FinalDamage = 1;

        status.Health -= FinalDamage;

        int AilmentIndex = HasStatusAilment(Element);
        if (AilmentIndex != -1)
        {
            StatusAilmentsList[AilmentIndex].IncreaseAdv(5);
            return;
        }

        StatusAilment statusAilment;
        switch (Element)
        {
            case ItemBase.TypeOfDamage.Physical:
                statusAilment = new PhysicalAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(0);
                StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Fire:
                statusAilment = new FireAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(0);
                StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Ice:
                statusAilment = new IceAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(0);
                StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Natural:
                statusAilment = new NaturalAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(0);
                StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Lightning:
                statusAilment = new LightningAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(0);
                StatusAilmentsList.Add(statusAilment);
                break;
            default:
                break;
        }
    }
}
