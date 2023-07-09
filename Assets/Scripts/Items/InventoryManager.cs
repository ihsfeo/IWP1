using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] ItemManager itemManager;
    [SerializeField] List<GameObject> InventoryRows = new List<GameObject>();
    [SerializeField] GameObject SlotHelmet, SlotChestplate, SlotLegging, SlotShoe, SlotWeapon1, SlotWeapon2, SlotWeapon3;

    List<ItemBase> Inventory = new List<ItemBase>();
    List<ItemBase> EquippedItems = new List<ItemBase>();
    List<GameObject> InventorySlot = new List<GameObject>();

    public bool CanPickUp(ItemBase Item)
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i] == null)
                return true;
            else if (Inventory[i].itemID == Item.itemID && Inventory[i].Count < Inventory[i].MaxCount)
                return true;
        }
        return false;
    }

    public int AddToInventory(ItemBase Item)
    {
        int AmountToAdd = Item.Count;
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (AmountToAdd <= 0)
                return 0;

            ItemBase temp = Inventory[i];
            if (temp == null)
            {
                Inventory[i] = Item;
                Inventory[i].Count = AmountToAdd;

                Instantiate(Inventory[i].gameObject, InventorySlot[i].transform);

                InventorySlot[i].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                InventorySlot[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                InventorySlot[i].transform.GetChild(0).gameObject.SetActive(true);
                InventorySlot[i].transform.GetChild(0).localScale = new Vector3(Item.transform.localScale.x * 100, Item.transform.localScale.y * 100, 1);

                if (AmountToAdd > Item.MaxCount)
                {
                    Inventory[i].Count = Item.MaxCount;
                    AmountToAdd -= Item.MaxCount;
                }
                else
                    AmountToAdd = 0;
            }
            else if (temp.itemID == Item.itemID && temp.Count < temp.MaxCount)
            {
                if (AmountToAdd + temp.Count - temp.MaxCount < 0)
                {
                    Inventory[i].Count += AmountToAdd;
                    AmountToAdd -= Inventory[i].MaxCount - Inventory[i].Count;
                    return 0;
                }
                else
                {
                    AmountToAdd -= temp.MaxCount - temp.Count;
                    Inventory[i].Count = temp.MaxCount;
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
            return AmountToAdd;

        return 0;
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

    void Equip(int slot)
    {
        switch(Inventory[slot].ItemType)
        {
            case ItemBase.TypeOfItems.Helmet:
            case ItemBase.TypeOfItems.Chestplate:
            case ItemBase.TypeOfItems.Legging:
            case ItemBase.TypeOfItems.Shoe:
            {
                ItemBase item = EquippedItems[(int)Inventory[slot].ItemType];
                EquippedItems[(int)Inventory[slot].ItemType] = Inventory[slot];
                Inventory[slot] = item;
                break;
                }
            case ItemBase.TypeOfItems.Weapon:
            default: break;
        }
    }

    public void init()
    {
        // Init Inventory List
        for (int i = 0; i < InventoryRows.Count; i++)
        {
            for (int j = 0; j < InventoryRows[i].transform.childCount; j++)
            {
                InventorySlot.Add(InventoryRows[i].transform.GetChild(j).gameObject);
                InventorySlot[i * 6 + j].AddComponent<ClickAndDrag>();
                InventorySlot[i * 6 + j].GetComponent<ClickAndDrag>().Slot = i * 6 + j;
                Inventory.Add(null);
            }
        }
        // Init Equipped List
        for (int i = 0; i < 7; i++)
        {
            EquippedItems.Add(null);
        }
    }

    private void Update()
    {
        
    }
}
