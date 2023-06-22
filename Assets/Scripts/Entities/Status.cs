using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    // States
    public bool InWater = false;
    public bool InLava = false;
    public bool IsFrozen = false;

    // Stats
    public int HealthMax;
    public int Health;
    public List<int> PResistances = new List<int>();
    public List<int> FResistances = new List<int>();
    public List<int> PDefences = new List<int>();
    public List<int> FDefences = new List<int>();
    public List<StatusAilment> StatusAilmentsList = new List<StatusAilment>();

    // Additional Stats
    public float MovementSpeed = 1;
    public float KnockbackResistance = 0;
}
