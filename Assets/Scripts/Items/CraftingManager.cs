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
    List<Button> ContentButtons;

    private void Awake()
    {
        for (int i = 0; i < CraftableList.Count; i++)
        {
            ContentButtons.Add(Content.transform.GetChild(i).GetComponent<Button>());
            TMPro.TextMeshPro text = ContentButtons[i].transform.GetChild(0).GetComponent<TMPro.TextMeshPro>();
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
    }

    // LeftPanel Buttons
    public void SelectCraft(int selection)
    {
        /*
        Update Description
        Update Cost
        etc
         */

        Description.UpdateDescription(CraftableList[selection]);
        Cost.UpdateCost(CraftableList[selection]);
    }

    public void CraftCraft(int selection)
    {
        // Check for materials * craft count
        // CraftCount at a time max at item max count and inventory space
    }
}
