using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndDrag : MonoBehaviour, IEventSystemHandler
{
    public int Slot;
    int SlotToSwapWith;
    GameObject Clone;
    InventoryManager inventoryManager;

    private void Awake()
    {
        
    }

    // Short Press
    public void ShowInfo()
    {
        /*
        
        Item Name
        Description
        Category of item

        Weapon* Element Damage
        Weapon* What kind of Weapon

        Equipment* Weapon* Extra Stats
         
         */
    }
}
