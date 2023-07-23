using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescription : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] GameObject StatObj;
    [SerializeField] TextMeshProUGUI Description;

    List<TextMeshProUGUI> Stats = new List<TextMeshProUGUI>();

    public void Init()
    {
        Stats.Add(StatObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
    }

    public void Show(ItemBase item)
    {
        try
        {
            Name.text = item.ItemName + " (" + new string[] { "Common", "Uncommon", "Rare" }[(int)item.Rarity] + ")";
            Description.text = item.Description;
            for (int i = 0; i < Stats.Count; i++)
            {
                if (i >= 1)
                {
                    Destroy(Stats[i].gameObject);
                    i--;
                }
                else Stats[i].text = "";
            }

            if (item.ItemType == ItemBase.TypeOfItems.Consumable || item.ItemType == ItemBase.TypeOfItems.NotConsumable)
            {
                // Either Heal+ something, or dont
            }
            else
            {
                for (int i = 0; i < item.cStats.Count; i++)
                {
                    if (Stats.Count == i)
                        Stats.Add(Instantiate(Stats[0], StatObj.transform));

                    Stats[i].text = "";
                    switch (item.cStats[i].Type)
                    {
                        case CStats.Stats.AttackFlat: Stats[i].text += "Attack +" + item.cStats[i].Value; break;
                        case CStats.Stats.AttackPercentage: Stats[i].text += "Attack +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.MaxHealth: Stats[i].text += "Max Health +" + item.cStats[i].Value; break;
                        case CStats.Stats.IncreasedAfflictions: Stats[i].text += "Afflict +" + item.cStats[i].Value; break;
                        case CStats.Stats.DefenceFlat: Stats[i].text += "Defence +" + item.cStats[i].Value; break;
                        case CStats.Stats.DefencePercentage: Stats[i].text += "Defence +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.FireDefenceFlat: Stats[i].text += "Fire Defence +" + item.cStats[i].Value; break;
                        case CStats.Stats.FireDefencePercentage: Stats[i].text += "Fire Defence +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.IceDefenceFlat: Stats[i].text += "Ice Defence +" + item.cStats[i].Value; break;
                        case CStats.Stats.IceDefencePercentage: Stats[i].text += "Ice Defence +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.NaturalDefenceFlat: Stats[i].text += "Natural Defence +" + item.cStats[i].Value; break;
                        case CStats.Stats.NaturalDefencePercentage: Stats[i].text += "Natural Defence +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.LightningDefenceFlat: Stats[i].text += "Lightning Defence +" + item.cStats[i].Value; break;
                        case CStats.Stats.LightningDefencePercentage: Stats[i].text += "Lightning Defence +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.PhysicalDefenceFlat: Stats[i].text += "Physical Defence +" + item.cStats[i].Value; break;
                        case CStats.Stats.PhysicalDefencePercentage: Stats[i].text += "Physical Defence +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.DarkDefenceFlat: Stats[i].text += "Dark Defence +" + item.cStats[i].Value; break;
                        case CStats.Stats.DarkDefencePercentage: Stats[i].text += "Dark Defence +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.LightDefenceFlat: Stats[i].text += "Light Defence +" + item.cStats[i].Value; break;
                        case CStats.Stats.LightDefencePercentage: Stats[i].text += "Light Defence +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.ResistanceFlat: Stats[i].text += "Resistance +" + item.cStats[i].Value; break;
                        case CStats.Stats.ResistancePercentage: Stats[i].text += "Resistance +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.FireResistanceFlat: Stats[i].text += "Fire Resistance +" + item.cStats[i].Value; break;
                        case CStats.Stats.FireResistancePercentage: Stats[i].text += "Fire Resistance +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.IceResistanceFlat: Stats[i].text += "Ice Resistance +" + item.cStats[i].Value; break;
                        case CStats.Stats.IceResistancePercentage: Stats[i].text += "Ice Resistance +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.NaturalResistanceFlat: Stats[i].text += "Natural Resistance +" + item.cStats[i].Value; break;
                        case CStats.Stats.NaturalResistancePercentage: Stats[i].text += "Natural Resistance +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.LightningResistanceFlat: Stats[i].text += "Lightning Resistance +" + item.cStats[i].Value; break;
                        case CStats.Stats.LightningResistancePercentage: Stats[i].text += "Lightning Resistance +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.PhysicalResistanceFlat: Stats[i].text += "Physical Resistance +" + item.cStats[i].Value; break;
                        case CStats.Stats.PhysicalResistancePercentage: Stats[i].text += "Physical Resistance +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.DarkResistanceFlat: Stats[i].text += "Dark Resistance +" + item.cStats[i].Value; break;
                        case CStats.Stats.DarkResistancePercentage: Stats[i].text += "Dark Resistance +" + item.cStats[i].Value + "%"; break;
                        case CStats.Stats.LightResistanceFlat: Stats[i].text += "Light Resistance +" + item.cStats[i].Value; break;
                        case CStats.Stats.LightResistancePercentage: Stats[i].text += "Light Resistance +" + item.cStats[i].Value + "%"; break;
                        default: return;
                    }
                    Stats[i].transform.position += new Vector3(0, -1 * i, 0);
                }
            }
        }
        catch { }
    }
}
