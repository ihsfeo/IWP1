using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] ItemManager itemManager;
    [SerializeField] List<GameObject> InventoryRows = new List<GameObject>();
    [SerializeField] GameObject Equips;
    [SerializeField] ItemDescription PopUp;

    public List<ItemBase> Inventory = new List<ItemBase>();
    public List<ItemBase> EquippedItems = new List<ItemBase>();
    List<GameObject> InventorySlot = new List<GameObject>();
    List<GameObject> EquippedSlot = new List<GameObject>();
    PlayerInfo player;

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
                Inventory[i] = Instantiate(Item, InventorySlot[i].transform);
                Inventory[i].Init();
                Inventory[i].Count = AmountToAdd;

                Inventory[i].transform.GetComponent<SpriteRenderer>().enabled = false;
                Inventory[i].transform.GetComponent<Image>().enabled = true;
                Inventory[i].transform.gameObject.SetActive(true);
                Inventory[i].transform.localScale = new Vector3(Item.transform.localScale.x * 100, Item.transform.localScale.y * 100, 1);

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

    public void Remove(ItemBase.ItemID ID, int Count)
    {
        int AmountRemoved = 0;
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (!Inventory[i])
                continue;
            if (Inventory[i].itemID == ID)
            {
                if (Inventory[i].Count <= Count)
                {
                    AmountRemoved += Inventory[i].Count;
                    DestroyImmediate(Inventory[i].gameObject);
                }
                else
                {
                    Inventory[i].Count -= Count - AmountRemoved;
                    AmountRemoved = Count;
                    break;
                }
                if (AmountRemoved == Count)
                    break;
            }
        }
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
            if (!Inventory[i])
                continue;
            if (Inventory[i].itemID == Material.itemID)
                Check_Count += Inventory[i].Count;
        }
        if (Check_Count >= count)
            return true;

        return false;
    }

    public int GetMaterials(ItemBase.ItemID ID)
    {
        int Check_Count = 0;
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (!Inventory[i])
                continue;
            if (Inventory[i].itemID == ID)
                Check_Count += Inventory[i].Count;
        }
        return Check_Count;
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
        player = GetComponentInParent<PlayerInfo>();

        // Init Inventory List
        for (int i = 0; i < InventoryRows.Count; i++)
        {
            for (int j = 0; j < InventoryRows[i].transform.childCount; j++)
            {
                InventorySlot.Add(InventoryRows[i].transform.GetChild(j).gameObject);
                InventorySlot[i * 6 + j].AddComponent<ClickAndDrag>();
                InventorySlot[i * 6 + j].GetComponent<ClickAndDrag>().Slot = i * 6 + j;
                InventorySlot[i * 6 + j].GetComponent<ClickAndDrag>().inventoryManager = this;
                InventorySlot[i * 6 + j].GetComponent<ClickAndDrag>().canvas = GetComponentInParent<Canvas>();
                InventorySlot[i * 6 + j].gameObject.tag = "iSlot";

                InventorySlot[i * 6 + j].AddComponent<HoverDescription>();
                InventorySlot[i * 6 + j].GetComponent<HoverDescription>().Display = PopUp;

                InventorySlot[i * 6 + j].AddComponent<BoxCollider2D>();
                InventorySlot[i * 6 + j].GetComponent<BoxCollider2D>().isTrigger = true;
                InventorySlot[i * 6 + j].GetComponent<BoxCollider2D>().size = new Vector2(100, 100);
                Inventory.Add(null);
            }
        }
        // Init Equipment
        for (int i = 0; i < Equips.transform.childCount; i++)
        {
            GameObject slot = Equips.transform.GetChild(i).gameObject;
            EquippedSlot.Add(slot);
            slot.AddComponent<ClickAndDrag>();
            slot.GetComponent<ClickAndDrag>().Slot = i;
            slot.GetComponent<ClickAndDrag>().inventoryManager = this;
            slot.GetComponent<ClickAndDrag>().canvas = GetComponentInParent<Canvas>();
            slot.tag = "iEquip";

            slot.AddComponent<HoverDescription>();
            slot.GetComponent<HoverDescription>().Display = PopUp;

            slot.AddComponent<BoxCollider2D>();
            slot.GetComponent<BoxCollider2D>().isTrigger = true;
            slot.GetComponent<BoxCollider2D>().size = new Vector2(100, 100);
        }
        // Init Equipped List
        for (int i = 0; i < 7; i++)
        {
            EquippedItems.Add(null);
        }

        EquippedItems[4] = Instantiate(itemManager.GetItem(ItemBase.ItemID.Sword), EquippedSlot[4].transform).GetComponent<ItemBase>();
        EquippedItems[4].Init();
        EquippedItems[4].GetComponent<Image>().enabled = true;
        EquippedItems[4].GetComponent<SpriteRenderer>().enabled = false;
        EquippedItems[4].transform.localScale = new Vector3(EquippedItems[4].transform.localScale.x * 100, EquippedItems[4].transform.localScale.y * 100, 1);

        EquippedItems[0] = Instantiate(itemManager.GetItem(ItemBase.ItemID.Helmet), EquippedSlot[0].transform).GetComponent<ItemBase>();
        EquippedItems[0].Init();
        EquippedItems[0].GetComponent<Image>().enabled = true;
        EquippedItems[0].GetComponent<SpriteRenderer>().enabled = false;
        EquippedItems[0].transform.localScale = new Vector3(EquippedItems[0].transform.localScale.x * 100, EquippedItems[0].transform.localScale.y * 100, 1);

        PopUp.Init();
        player.UpdateStatus();
    }

    public void SwapTwoSlots(int Slot1, int Slot2)
    {
        GameObject item1, item2;
        if (InventorySlot[Slot1].transform.childCount > 0)
        {
            //item1 = InventorySlot[Slot1].transform.GetChild(0).gameObject;
            Destroy(InventorySlot[Slot1].transform.GetChild(0).gameObject);
        }
        else item1 = null;
        if (InventorySlot[Slot2].transform.childCount > 0)
        {
            //item2 = InventorySlot[Slot2].transform.GetChild(0).gameObject;
            Destroy(InventorySlot[Slot2].transform.GetChild(0).gameObject);
        }
        else item2 = null;

        //if (item1)
        //{
        //    GameObject tempObj = Instantiate(item1, InventorySlot[Slot2].transform);
        //    tempObj.GetComponent<Image>().enabled = true;
        //}
        //if (item2)
        //{
        //    GameObject tempObj = Instantiate(item2, InventorySlot[Slot1].transform);
        //    tempObj.GetComponent<Image>().enabled = true;
        //}

        ItemBase temp = null;
        if (Inventory[Slot1])
        {
            temp = Instantiate(Inventory[Slot1]);
            temp.name = Inventory[Slot1].name;
            temp.GetComponent<Image>().enabled = true;
            Destroy(Inventory[Slot1]);
        }
        if (Inventory[Slot2])
        {
            Inventory[Slot1] = Instantiate(Inventory[Slot2]);
            Inventory[Slot1].name = Inventory[Slot2].name;
            Inventory[Slot1].GetComponent<Image>().enabled = true;
            Destroy(Inventory[Slot2]);
        }
        else Inventory[Slot1] = null;
        Inventory[Slot2] = temp;
    }

    public void SwapTwoEquipment(int Slot1, int Slot2)
    {
        GameObject item1, item2;

        if (Slot1 <= 3 || Slot2 <= 3)
            return;

        if (EquippedSlot[Slot1].transform.childCount > 0)
        {
            //item1 = EquippedSlot[Slot1].transform.GetChild(0).gameObject;
            Destroy(EquippedSlot[Slot1].transform.GetChild(0).gameObject);
        }
        else item1 = null;
        if (EquippedSlot[Slot2].transform.childCount > 0)
        {
            //item2 = EquippedSlot[Slot2].transform.GetChild(0).gameObject;
            Destroy(EquippedSlot[Slot2].transform.GetChild(0).gameObject);
        }
        else item2 = null;

        //if (item1)
        //{
        //    GameObject tempObj = Instantiate(item1, EquippedSlot[Slot2].transform);
        //    tempObj.GetComponent<Image>().enabled = true;
        //}
        //if (item2)
        //{
        //    GameObject tempObj = Instantiate(item2, EquippedSlot[Slot1].transform);
        //    tempObj.GetComponent<Image>().enabled = true;
        //}

        ItemBase temp = null;
        if (EquippedItems[Slot1])
        {
            temp = Instantiate(EquippedItems[Slot1]);
            temp.name = EquippedItems[Slot1].name;
            temp.GetComponent<Image>().enabled = true;
            Destroy(EquippedItems[Slot1].gameObject);
        }
        if (EquippedItems[Slot2])
        {
            EquippedItems[Slot1] = Instantiate(EquippedItems[Slot2]);
            EquippedItems[Slot1].name = EquippedItems[Slot2].name;
            EquippedItems[Slot1].GetComponent<Image>().enabled = true;
            Destroy(EquippedItems[Slot2].gameObject);
        }
        else EquippedItems[Slot1] = null;

        EquippedItems[Slot2] = temp;

        player.UpdateStatus();
    }

    public void SwapEquipmentItem(int Slot1, int Slot2)
    {
        GameObject item1, item2;
        try
        {
            switch (Inventory[Slot2].ItemType)
            {
                case ItemBase.TypeOfItems.Helmet:
                    if (Slot1 != 0) return;
                    break;
                case ItemBase.TypeOfItems.Chestplate:
                    if (Slot1 != 1) return;
                    break;
                case ItemBase.TypeOfItems.Legging:
                    if (Slot1 != 2) return;
                    break;
                case ItemBase.TypeOfItems.Shoe:
                    if (Slot1 != 3) return;
                    break;
                case ItemBase.TypeOfItems.Weapon:
                    if (Slot1 < 4) return;
                    break;
                default: break;
            }
        }
        catch
        {
        }

        if (EquippedSlot[Slot1].transform.childCount > 0)
        {
            //item1 = EquippedSlot[Slot1].transform.GetChild(0).gameObject;
            Destroy(EquippedSlot[Slot1].transform.GetChild(0).gameObject);
        }
        else item1 = null;
        if (InventorySlot[Slot2].transform.childCount > 0)
        {
            //item2 = InventorySlot[Slot2].transform.GetChild(0).gameObject;
            Destroy(InventorySlot[Slot2].transform.GetChild(0).gameObject);
        }
        else item2 = null;

        //if (item1)
        //{
        //    GameObject tempObj = Instantiate(item1, InventorySlot[Slot2].transform);
        //    tempObj.name = item1.name;
        //}
        //if (item2)
        //{
        //    GameObject tempObj = Instantiate(item2, EquippedSlot[Slot1].transform);
        //    tempObj.name = item2.name;
        //}

        ItemBase temp = null;

        if (EquippedItems[Slot1])
        {
            temp = Instantiate(EquippedItems[Slot1], InventorySlot[Slot2].transform);
            temp.name = EquippedItems[Slot1].name;
            temp.GetComponent<Image>().enabled = true;
            Destroy(EquippedItems[Slot1].gameObject);
        }
        if (Inventory[Slot2])
        {
            EquippedItems[Slot1] = Instantiate(Inventory[Slot2], EquippedSlot[Slot1].transform);
            EquippedItems[Slot1].name = Inventory[Slot2].name;
            EquippedItems[Slot1].GetComponent<Image>().enabled = true;
            Destroy(Inventory[Slot2].gameObject);
        }
        else EquippedItems[Slot1] = null;
        Inventory[Slot2] = temp;

        player.UpdateStatus();
    }

    private void Update()
    {
        
    }
}
