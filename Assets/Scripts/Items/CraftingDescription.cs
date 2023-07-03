using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingDescription : MonoBehaviour
{
    [SerializeField] TextMeshPro Name;
    [SerializeField] TextMeshPro MainDescription;
    [SerializeField] TextMeshPro Rarity;
    public void UpdateDescription(ItemBase item)
    {
        Name.text = item.ItemName;

        //Color Based on Rarity
        switch (item.Rarity)
        {
            case ItemBase.ItemRarity.Common:
                Name.color = Color.black;
                break;
            case ItemBase.ItemRarity.Uncommon:
                Name.color = Color.green;
                break;
            case ItemBase.ItemRarity.Rare:
                Name.color = Color.cyan;
                break;
            default: break;
        }

        // Description
        switch (item.itemID)
        {
            case ItemBase.ItemID.Sword: 
            case ItemBase.ItemID.Helmet:
            default: break;
        }
    }
}
