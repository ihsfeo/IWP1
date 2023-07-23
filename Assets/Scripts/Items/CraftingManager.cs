using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] List<ItemBase> CraftableList = new List<ItemBase>();
    [SerializeField] GameObject Content;
    [SerializeField] CraftingDescription Description;
    [SerializeField] CraftingCost Cost;
    [SerializeField] public ItemManager itemManager;
    [SerializeField] public InventoryManager inventoryManager;
    [SerializeField] TMPro.TMP_Text CountText;

    public int CraftCount = 1;
    int CurrentSelect = 0;
    bool CanCraft = false;

    List<Button> ContentButtons = new List<Button>();
    public List<ItemBase.ItemID> MaterialList = new List<ItemBase.ItemID>();
    public List<int> AmountList = new List<int>();

    private void Awake()
    {
        for (int i = 0; i < CraftableList.Count; i++)
        {
            CraftableList[i].Init();
            ContentButtons.Add(Content.transform.GetChild(i).GetComponent<Button>());
            ContentButtons[i].gameObject.SetActive(true);
            TMPro.TMP_Text text = ContentButtons[i].transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            text.text = CraftableList[i].ItemName;
            switch (CraftableList[i].Rarity)
            {
                case ItemBase.ItemRarity.Uncommon:
                    text.color = Color.green;
                    break;
                case ItemBase.ItemRarity.Rare:
                    text.color = Color.cyan;
                    break;
                default: break;
            }
        }
        SelectCraft(0);
    }

    // LeftPanel Buttons
    public void SelectCraft(int selection)
    {
        /*
        Update Description
        Update Cost
        etc
         */

        CurrentSelect = selection;
        Description.UpdateDescription(CraftableList[selection]);
        CanCraft = Cost.UpdateCost(CraftableList[selection]);
        CraftCount = 1;
        CountText.text = CraftCount.ToString();
    }

    public void CraftCraft(int selection)
    {
        // Check for materials * craft count
        // CraftCount at a time max at item max count and inventory space
    }

    public void IncreaseCraftCount()
    {
        if (CraftCount != 99)
        {
            CraftCount++;
            CanCraft = Cost.UpdateCost(CraftableList[CurrentSelect]);
            CountText.text = CraftCount.ToString();
        }
    }

    public void DecreaseCraftCount()
    {
        if (CraftCount != 1)
        {
            CraftCount--;
            CanCraft = Cost.UpdateCost(CraftableList[CurrentSelect]);
            CountText.text = CraftCount.ToString();
        }
    }

    public void Craft()
    {
        if (CanCraft)
        {
            // Craft
            ItemBase temp = Instantiate(itemManager.GetItemBase(CraftableList[CurrentSelect].itemID));
            temp.Init();
            inventoryManager.AddToInventory( temp );
            for (int i = 0; i < MaterialList.Count; i++)
            {
                inventoryManager.Remove(MaterialList[i], AmountList[i] * CraftCount);
            }
            CanCraft = Cost.UpdateCost(CraftableList[CurrentSelect]);
        }
    }
}
