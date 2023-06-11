using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] ItemManager itemManager;
    [SerializeField] List<GameObject> InventorySlot = new List<GameObject>();
    [SerializeField] GameObject HelmetSlot, WeaponSlot1, WeaponSlot2, WeaponSlot3;

    List<ItemBase> Inventory = new List<ItemBase>();
    List<ItemBase> EquippedItems = new List<ItemBase>();

    //bool CanPickUp(ItemBase Item)
    //{
    //    for (int i = 0; i < Inventory.Count; i++)
    //    {
    //        if (Inventory[i].itemID == Item.itemID && Inventory[i].Count < Inventory[i].MaxCount)
    //            return true;
    //        else if (Inventory[i] == null)
    //            return true;
    //    }
    //    return false;
    //}

    void AddToInventory(ItemBase Item)
    {
        int AmountToAdd = Item.Count;
        for (int i = 0; i < Inventory.Count; i++)
        {
            ItemBase temp = Inventory[i];
            if (temp.itemID == Item.itemID && temp.Count < temp.MaxCount)
            {
                if (AmountToAdd + temp.Count - temp.MaxCount < 0)
                {
                    temp.Count += AmountToAdd;
                    AmountToAdd -= temp.MaxCount - temp.Count;
                    return;
                }
                else
                {
                    AmountToAdd -= temp.MaxCount - temp.Count;
                    temp.Count = temp.MaxCount;
                }
            }
        }

        if (AmountToAdd > 0)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i] == null)
                {
                    Inventory[i] = Instantiate(Item);
                    Inventory[i].transform.parent = InventorySlot[i].transform;
                    if (AmountToAdd > Item.MaxCount)
                    {
                        Inventory[i].Count = Item.MaxCount;
                        AmountToAdd -= Item.MaxCount;
                    }
                    else
                    {
                        Inventory[i].Count = AmountToAdd;
                        AmountToAdd = 0;
                    }
                }

                if (AmountToAdd == 0)
                    break;
            }
        }

        if (AmountToAdd > 0)
            Item.Count = AmountToAdd;
        else
            Destroy(Item.gameObject);
    }

    public bool HasMaterials(ItemBase Material1, int count1, ItemBase Material2 = null, int count2 = 0, ItemBase Material3 = null, int count3 = 0)
    {
        if (!HasMaterial(Material1, count1))
            return false;
        if (Material2 == null || !HasMaterial(Material2, count2))
            return false;
        if (Material3 == null || !HasMaterial(Material3, count3))
            return false;

        return true;
    }

    bool HasMaterial(ItemBase Material, int count)
    {
        int Check_Count = 0;
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].itemID == Material.itemID)
                Check_Count += Inventory[i].Count;
        }
        if (Check_Count >= count)
            return true;

        return false;
    }

    private void Awake()
    {
        for (int i = 0; i < 30; i++)
        {
            Inventory.Add(null);
        }
    }

    private void Update()
    {
        
    }
}
