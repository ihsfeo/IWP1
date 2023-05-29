using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public bool Broken = false;

    public void Hit()
    {
        if (!Broken)
        {
            Broken = true;
            Destroy(transform.parent.gameObject); // temporary bop
            // Drop things
            // Break animation
        }
    }
}
