using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : WeaponBase
{
    [SerializeField]
    PlayerInfo playerInfo;

    List<GameObject> CollisionObjects = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged")
            CollisionObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged")
            CollisionObjects.Remove(collision.gameObject);
    }

    // Start is called before the first frame update
    void Awake()
    {
        eTypeOfDamage = TypeOfDamage.Physical;
        BaseDamage = 10;
        Proficiency = 0;
        ProficiencyExp = 0;
        ProficiencyOwned = 0;
        ProficiencyRequired = 100; // Temp Number
        eWeaponType = WeaponType.Swords;
        Level = 1;
        // Tree
        // BonusStats
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInfo.SwingingSword) // Attacking
        {
            for (int i = 0; i < CollisionObjects.Count; i++)
            {
                GameObject obj = CollisionObjects[i];
                switch (obj.tag)
                {
                    case "Destructible": // Destroy Destructible Objects
                        obj.GetComponent<Destructible>().Hit();
                        break;
                    case "Enemy":
                        break;
                }
            }
        }
    }
}
