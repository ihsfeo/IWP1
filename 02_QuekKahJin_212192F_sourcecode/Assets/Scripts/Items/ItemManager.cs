using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] public List<ItemBase> ItemList = new List<ItemBase>();
    [SerializeField] public GameObject droppedItem;

    public GameObject GetItem(ItemBase.ItemID itemID)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].itemID == itemID)
                return ItemList[i].gameObject;
        }
        return null;
    }

    public ItemBase GetItemBase(ItemBase.ItemID itemID)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].itemID == itemID)
                return ItemList[i];
        }
        return null;
    }
}
