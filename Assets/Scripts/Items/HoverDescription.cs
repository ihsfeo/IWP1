using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDescription : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public ItemDescription Display;

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Getting item
        ClickAndDrag temp = GetComponent<ClickAndDrag>();
        ItemBase item = null;
        if (temp.gameObject.tag == "iSlot")
            item = temp.inventoryManager.Inventory[temp.Slot];
        else if (temp.gameObject.tag == "iEquip")
            item = temp.inventoryManager.EquippedItems[temp.Slot];
        try
        {
            item.gameObject.SetActive(true);
        }
        catch
        {
            return;
        }

        /*
        
        Item Name
        Description
        Category of item

        Weapon* Element Damage
        Weapon* What kind of Weapon

        Equipment* Weapon* Extra Stats
         
         */
        Display.Show(item);
        // Change position
        if (transform.position.x < 960)
            this.Display.transform.position = new Vector3(transform.position.x + 500, 540, 0);
        else
            this.Display.transform.position = new Vector3(transform.position.x - 500, 540, 0);


        Display.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Display.gameObject.SetActive(false);
    }
}
