using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item.asset", menuName = "Equipment/Item")]
public class EquipmentSO : ScriptableObject
{
    public string Name;

    public CStats.Stats PrimaryStat;
    public float PrimaryValue;
}
