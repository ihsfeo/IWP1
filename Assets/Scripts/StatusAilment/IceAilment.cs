using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAilment : StatusAilment
{
    float TimeForDamage = 0;
    float FrozenTime = 0;

    public override float GetAdvNeeded(int level)
    {
        if (level == 1) return 25;
        else if (level == 2) return 100;
        else if (level == 3) return 300;
        return 0;
    }

    public override void IncreaseAdv(int amount)
    {
        TimeFromHit = 0;
        if (Level == 3)
            return;

        Adv += amount;
        if (Adv >= GetAdvNeeded(Level + 1))
        {
            Adv -= GetAdvNeeded(Level + 1);
            AdvNeeded = GetAdvNeeded(Level + 2);
            Level++;
            if (Level == 3)
                FrozenTime = 1;
        }
    }

    bool HasAilment(List<StatusAilment> statusAilments, ItemBase.TypeOfDamage Element, int MinimumLevel)
    {
        for (int i = 0; i < statusAilments.Count; i++)
        {
            if (statusAilments[i].TypeOfAilment == Element && statusAilments[i].Level >= MinimumLevel)
                return true;
        }

        return false;
    }

    int LevelOfAilment(List<StatusAilment> statusAilments, ItemBase.TypeOfDamage Element)
    {
        for (int i = 0; i < statusAilments.Count; i++)
        {
            if (statusAilments[i].TypeOfAilment == Element)
                return statusAilments[i].Level;
        }
        return 0;
    }

    public override bool UpdateAilment(List<StatusAilment> statusAilments, Status status)
    {
        int LevelOfFire = LevelOfAilment(statusAilments, ItemBase.TypeOfDamage.Fire);
        if (LevelOfFire > 0)
        {
            DecreaseAdv(LevelOfFire * Time.deltaTime);
        }

        if (TimeFromHit > 5)
        {
            if (Level < 3)
            {
                DecreaseAdv(Time.deltaTime);
                if (Adv <= 0)
                    return false;
            }
        }
        else
            TimeFromHit += Time.deltaTime;

        // When Frozen
        if (FrozenTime > 0)
        {
            status.IsFrozen = true;
            FrozenTime -= Time.deltaTime;
            // Breaking Out Of Frozen On Time
            if (FrozenTime <= 0)
            {
                DecreaseAdv(GetAdvNeeded(3) / 2);
                status.IsFrozen = false;
            }
        }

        if (Level < 3)
            status.MovementSpeed = 1 - 0.1f * Level;
        else
            status.MovementSpeed = 0;

        // Take Damage
        if (Level == 2 && HasAilment(statusAilments, ItemBase.TypeOfDamage.Fire, 2))
        {
            TimeForDamage += Time.deltaTime;
            if (TimeForDamage >= 1)
            {
                status.Health -= 1;
                TimeForDamage--;
            }
        }
        else
            TimeForDamage = 0;

        return true;
    }
}
