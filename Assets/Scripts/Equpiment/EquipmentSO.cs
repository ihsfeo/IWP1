using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item.asset", menuName = "Item/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public string Name;
    public string Description;

    public ItemBase.ItemID itemID;
    public ItemBase.ItemRarity itemRarity;
    public List<CStats.Stats> Stats = new List<CStats.Stats>();
    public List<float> StatValues = new List<float>();
}
