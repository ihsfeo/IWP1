using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAndDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public int Slot;
    public int SlotToSwapWith;
    public bool CanSwap;
    GameObject Clone;
    public InventoryManager inventoryManager;
    private Vector2 OGPosition;
    public Canvas canvas;
    private RectTransform dragRectTransform;
    private string SlotName;
    float TimeFromClick = 1;

    private void Awake()
    {
        dragRectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if ((tag == "iSlot" && inventoryManager.Inventory[Slot]) ||
            (tag == "iEquip" && inventoryManager.EquippedItems[Slot]))
        {
            if (TimeFromClick <= 0.1f)
            {
                inventoryManager.UseItem(Slot);
            }
            TimeFromClick = 0;
            OGPosition = gameObject.transform.position;
            Clone = Instantiate(this.gameObject, transform.parent);
            Clone.name = this.name;
            Clone.GetComponent<ClickAndDrag>().enabled = false;
            Clone.GetComponent<HoverDescription>().enabled = false;
            Clone.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if ((tag == "iSlot" && inventoryManager.Inventory[Slot]) || 
            (tag == "iEquip" && inventoryManager.EquippedItems[Slot]))
        {
            TimeFromClick += Time.deltaTime;
            dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            CheckForAvailableSlot();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if ((tag == "iSlot" && inventoryManager.Inventory[Slot]) ||
            (tag == "iEquip" && inventoryManager.EquippedItems[Slot]))
        {
            gameObject.transform.position = OGPosition;
            SelectSwappableSlot();
            Destroy(Clone);
        }
    }

    void SelectSwappableSlot()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        for (int i = 0; i < raycastResults.Count; i++)
        {
            print(raycastResults[i].gameObject.name);
            if (raycastResults[i].gameObject.CompareTag("iSlot") || raycastResults[i].gameObject.CompareTag("iEquip") && SlotName != raycastResults[i].gameObject.name)
            {
                SlotToSwapWith = raycastResults[i].gameObject.GetComponentInChildren<ClickAndDrag>().Slot;

                
                if (Clone.tag == "iSlot" && raycastResults[i].gameObject.tag == "iSlot")
                {
                    inventoryManager.SwapTwoSlots(Slot, SlotToSwapWith);
                }
                else if (Clone.tag == "iEquip" && raycastResults[i].gameObject.tag == "iEquip")
                {
                    inventoryManager.SwapTwoEquipment(Slot, SlotToSwapWith);
                }
                else if (Clone.tag == "iEquip" && raycastResults[i].gameObject.tag == "iSlot")
                {
                    inventoryManager.SwapEquipmentItem(Slot, SlotToSwapWith);
                }
                else if(Clone.tag == "iSlot" && raycastResults[i].gameObject.tag == "iEquip")
                {
                    inventoryManager.SwapEquipmentItem(SlotToSwapWith, Slot);
                }


                return;
            }
            //else if (raycastResults[i].gameObject.CompareTag("dropArea") && SlotName != raycastResults[i].gameObject.name)
            //{
            //    inventoryManager.Drop(Slot);
            //    return;
            //}
        }
    }

    public void CheckForAvailableSlot()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);


        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.CompareTag("iSlot") && SlotName != raycastResults[i].gameObject.name)
            {
                CanSwap = true;
                return;
            }
        }
        CanSwap = false;

    }
}
