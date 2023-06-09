using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    TMP_Text healthText;

    [SerializeField]
    GameObject RedBar;
    [SerializeField]
    GameObject GreyBar;

    public void UpdateHealthBar(int MaxHealth, int Health)
    {
        GreyBar.transform.localScale = new Vector3(MaxHealth * 4, 100, 1);
        GreyBar.transform.position = new Vector3(MaxHealth * 2 + 100, 945, 1);

        RedBar.transform.localScale = new Vector3(Health * 4, 100, 1);
        RedBar.transform.position = new Vector3(Health * 2 + 100, 945, 0);

        healthText.text = Health + "/" + MaxHealth;
        healthText.gameObject.transform.position = GreyBar.transform.position;
    }
}
