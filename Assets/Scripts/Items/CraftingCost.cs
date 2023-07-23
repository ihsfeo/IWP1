using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingCost : MonoBehaviour
{
    CraftingManager CM;
    public bool UpdateCost(ItemBase item)
    {
        CM = transform.parent.parent.GetComponent<CraftingManager>();
        CM.AmountList.Clear();
        CM.MaterialList.Clear();

        Transform Amount = transform.GetChild(0);
        Transform Material = transform.GetChild(1);
        Transform Owned = transform.GetChild(2);
        switch (item.itemID)
        {
            case ItemBase.ItemID.Sword:
                CM.AmountList.Add(5);
                CM.MaterialList.Add(ItemBase.ItemID.WeaponFragment1);
                break;
            default:
                CM.AmountList.Add(5);
                CM.MaterialList.Add(ItemBase.ItemID.WeaponFragment1);
                break;
        }

        bool CanCraft = true;
        for (int i = 0; i < 3; i++)
        {
            if (CM.AmountList.Count > i)
            {
                int AmountHave = CM.inventoryManager.GetMaterials(CM.MaterialList[i]);
                Amount.GetChild(i).GetComponent<TMP_Text>().text = (CM.AmountList[i] * CM.CraftCount).ToString();
                Material.GetChild(i).GetComponent<TMP_Text>().text = CM.itemManager.GetItemBase(CM.MaterialList[i]).ItemName;
                Owned.GetChild(i).GetComponent<TMP_Text>().text = AmountHave.ToString();
                Amount.GetChild(i).gameObject.SetActive(true);
                Material.GetChild(i).gameObject.SetActive(true);
                Owned.GetChild(i).gameObject.SetActive(true);
                if (AmountHave < CM.AmountList[i] * CM.CraftCount)
                    CanCraft = false;
            }
            else
            {
                Amount.GetChild(i).gameObject.SetActive(false);
                Material.GetChild(i).gameObject.SetActive(false);
                Owned.GetChild(i).gameObject.SetActive(false);
            }
        }
        return CanCraft;
    }
}
