using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item.asset", menuName = "Item/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string Name;
    public string Description;

    public int BaseDamage;

    public ItemBase.ItemID itemID;
    public ItemBase.ItemRarity itemRarity;
    public ItemBase.TypeOfDamage typeOfDamage;
    public ItemBase.WeaponType weaponType;
    public List<CStats.Stats> Stats = new List<CStats.Stats>();
    public List<float> StatValues = new List<float>();
}
