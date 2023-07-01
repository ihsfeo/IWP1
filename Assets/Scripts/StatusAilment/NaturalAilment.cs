using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalAilment : StatusAilment
{
    float PoisonDamageTime = 0;
    int PoisonSpeed = 0;

    void UpdatePoisonSpeed()
    {
        if (Level == 1)
            PoisonSpeed = 6;
        else if (Level == 2)
            PoisonSpeed = 3;
        else if (Level == 3)
            PoisonSpeed = 1;
    }

    public override float GetAdvNeeded(int level)
    {
        if (level == 1) return 25;
        else if (level == 2) return 100;
        else if (level == 3) return 250;
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

    public override bool UpdateAilment(Status status)
    {
        if (TimeFromHit > 5)
        {
            if (Level == 0)
            {
                DecreaseAdv(Time.deltaTime);
                if (Adv <= 0)
                    return false;
            }
        }
        else
            TimeFromHit += Time.deltaTime;

        switch (Level)
        {
            case 1: 
            case 2:
            case 3:
                UpdatePoisonSpeed();
                PoisonDamageTime -= Time.deltaTime / PoisonSpeed; break;
            default: break;
        }

        while (PoisonDamageTime <= 0)
        {
            PoisonDamageTime += Time.deltaTime / PoisonSpeed;
            status.Health -= 1;
            if (status.Health < 0)
                status.Health = 0;
        }

        return true;
    }
}
