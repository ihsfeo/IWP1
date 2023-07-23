using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingNPC : MonoBehaviour
{
    public void Interact(bool b)
    {
        transform.GetChild(0).gameObject.SetActive(b);
    }
}
