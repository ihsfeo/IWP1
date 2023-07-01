using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAilment : StatusAilment
{
    float OverChargedTime = 0;
    float TillShock = 0;
    float ShockTime = 0;

    public override float GetAdvNeeded(int level)
    {
        if (level == 1) return 50;
        else if (level == 2) return 150;
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
            {
                OverChargedTime = 6;
                // Overlay OverCharged Sprite
            }
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
        if (OverChargedTime > 0)
        {
            OverChargedTime -= Time.deltaTime;
            TillShock -= Time.deltaTime;
            if (TillShock <= 0)
            {
                TillShock = 2;
                ShockTime = 0.2f;
            }
            if (OverChargedTime <= 0)
            {
                DecreaseAdv(GetAdvNeeded(3) / 2);
            }    
        }
        else if (TimeFromHit > 5)
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

        if (ShockTime > 0)
        {
            ShockTime -= Time.deltaTime;
            if (ShockTime <= 0)
                status.IsShocked = false;
        }

        return true;
    }
}
