using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusAilment : MonoBehaviour
{
    public ItemBase.TypeOfDamage TypeOfAilment;
    public int Level;
    public float Adv;
    public float AdvNeeded;
    public float TimeFromHit;

    public virtual float GetAdvNeeded(int level)
    {
        return 0;
    }
    
    public virtual void IncreaseAdv(int amount)
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

    // For Use of Items etc, Big one time use things
    public void DecreaseAdv(float amount)
    {
        Adv -= amount;
        while (Adv < 0)
        {
            if (Level == 0)
                return; 
            Adv += GetAdvNeeded(Level);
            AdvNeeded = GetAdvNeeded(Level);
            Level--;
        }
    }

    public virtual bool UpdateAilment(Status status)
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
