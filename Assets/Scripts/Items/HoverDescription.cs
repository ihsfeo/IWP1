using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverDescription : MonoBehaviour
{
    public GameObject Display;

    private void OnMouseEnter()
    {
        // Getting item
        ClickAndDrag temp = GetComponent<ClickAndDrag>();
        ItemBase item = temp.inventoryManager.Inventory[temp.Slot];

        /*
        
        Item Name
        Description
        Category of item

        Weapon* Element Damage
        Weapon* What kind of Weapon

        Equipment* Weapon* Extra Stats
         
         */


        Display.SetActive(true);
    }

    private void OnMouseOver()
    {
        // Change position if you want
    }

    private void OnMouseExit()
    {
        Display.SetActive(false);
    }
}
