using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalAilment : StatusAilment
{
    public override float GetAdvNeeded(int level)
    {
        if (level == 1) return 25;
        else if (level == 2) return 75;
        else if (level == 3) return 125;
        return 0;
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

        return true;
    }
}
