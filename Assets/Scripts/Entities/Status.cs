using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public bool InWater = false;
    public bool InLava = false;
    public float MovementSpeed = 1;
    public int HealthMax;
    public int Health;
    public bool IsFrozen = false;
    public float KnockbackResistance = 0;

    private void Update()
    {
        if (Health < 0)
            Health = 0;
    }
}
