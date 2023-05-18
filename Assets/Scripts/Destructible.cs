using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    bool Broken = false;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
