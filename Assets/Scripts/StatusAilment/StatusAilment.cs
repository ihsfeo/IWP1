using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusAilment : MonoBehaviour
{
    public WeaponBase.TypeOfDamage TypeOfAilment;
    public int Level;
    public float Adv;
    public float AdvNeeded;
    float TimeFromHit;

    float GetAdvNeeded(int level)
    {
        switch (TypeOfAilment)
        {
            case WeaponBase.TypeOfDamage.Physical:
                if (level == 1) return 25;
                else if (level == 2) return 75;
                else if (level == 3) return 125;
                break;
            case WeaponBase.TypeOfDamage.Fire:
                if (level == 1) return 25;
                else if (level == 2) return 75;
                else if (level == 3) return 125;
                break;
            case WeaponBase.TypeOfDamage.Ice:
                if (level == 1) return 25;
                else if (level == 2) return 100;
                else if (level == 3) return 300;
                break;
            case WeaponBase.TypeOfDamage.Natural:
                if (level == 1) return 25;
                else if (level == 2) return 100;
                else if (level == 3) return 250;
                break;
            case WeaponBase.TypeOfDamage.Lightning:
                if (level == 1) return 50;
                else if (level == 2) return 150;
                else if (level == 3) return 300;
                break;
            //case WeaponBase.TypeOfDamage.Dark:
            //case WeaponBase.TypeOfDamage.Light:
        }
        return 0;
    }
    
    public void IncreaseAdv(int amount)
    {
        if (Level == 3)
            return;
        Adv += amount;
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
            Level--;
        }
    }

    public virtual bool UpdateAilment()
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
