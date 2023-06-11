using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [SerializeField]
    List<ItemBase> CraftableList = new List<ItemBase>();

    private void Awake()
    {
        for (int i = 0; i < CraftableList.Count; i++)
        {

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
    }

    public void CraftCraft(int selection)
    {

    }
}
