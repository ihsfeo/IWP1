using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSelect : MonoBehaviour
{
    [SerializeField]
    CraftingManager craftingManager;

    public void CraftClicked(Button button)
    {
        Debug.Log(button.name);
    }
}
