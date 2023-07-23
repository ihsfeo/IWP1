using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeaponDisplay : MonoBehaviour
{
    public void UpdateUI(List<ItemBase> weaponList, int current)
    {
        for (int i = 0; i < 3; i++)
        {
            if (transform.GetChild(i).childCount > 0)
                DestroyImmediate(transform.GetChild(i).GetChild(0).gameObject);

            if (weaponList[i] != null)
            {
                Instantiate(weaponList[i], transform.GetChild(i));
                transform.GetChild(i).GetChild(0).localPosition = new Vector3(0, 0, 0);
                transform.GetChild(i).GetChild(0).localScale = new Vector3(weaponList[i].transform.localScale.x * 100, weaponList[i].transform.localScale.y * 100, 1);
                transform.GetChild(i).GetChild(0).GetComponent<Image>().enabled = true;
                transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                transform.GetChild(i).GetChild(0).gameObject.SetActive(true);

                if (current == i)
                {
                    transform.GetChild(i).GetComponent<Image>().color = new Color(0.36f, 1f, 0.60f);
                }
                else
                    transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
                transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
}
