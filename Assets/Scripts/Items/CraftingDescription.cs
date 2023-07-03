using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CraftingDescription : MonoBehaviour
{
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text MainDescription;
    [SerializeField] TMP_Text Rarity;
    public void UpdateDescription(ItemBase item)
    {
        Name.text = item.ItemName;

        //Color Based on Rarity
        switch (item.Rarity)
        {
            case ItemBase.ItemRarity.Common:
                Name.color = Color.black;
                Rarity.text = "Common";
                Rarity.color = Name.color;
                break;
            case ItemBase.ItemRarity.Uncommon:
                Name.color = Color.green;
                Rarity.text = "Uncommon";
                Rarity.color = Name.color;
                break;
            case ItemBase.ItemRarity.Rare:
                Name.color = Color.cyan;
                Rarity.text = "Rare";
                Rarity.color = Name.color;
                break;
            default: break;
        }

        // Description
        switch (item.itemID)
        {
            case ItemBase.ItemID.Sword:
                MainDescription.text = "This is a Sword.";
                break;
            case ItemBase.ItemID.Helmet:
            default: break;
        }
    }
}
