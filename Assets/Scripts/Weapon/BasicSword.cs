using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : ItemBase
{
    [SerializeField] PlayerInfo playerInfo;

    List<GameObject> HitList = new List<GameObject>();
    List<GameObject> CollisionObjects = new List<GameObject>();

    protected TypeOfDamage eTypeOfDamage;

    protected WeaponProficiency weaponProficiency;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged" &&
            collision.gameObject.tag != "Solid" &&
            collision.gameObject.tag != "Water" &&
            collision.gameObject.tag != "Platform")
            CollisionObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged" &&
            collision.gameObject.tag != "Solid" &&
            collision.gameObject.tag != "Water" &&
            collision.gameObject.tag != "Platform")
            CollisionObjects.Remove(collision.gameObject);
    }

    public override void ResetHits()
    {
        HitList.Clear();
    }

    public override bool HitEntity(GameObject target)
    {
        if (!HitList.Contains(target))
        {
            HitList.Add(target);
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        eTypeOfDamage = TypeOfDamage.Physical;
        BaseDamage = 10;
        weaponProficiency = new WeaponProficiency(Rarity);
        eWeaponType = WeaponType.Sword;
        Level = 1;
        itemID = ItemID.Sword;
        MaxCount = 1;
        Count = 1;
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
                if (obj == null)
                {
                    CollisionObjects.Remove(obj);
                    i--;
                    continue;
                }
                switch (obj.tag)
                {
                    case "Destructible": // Destroy Destructible Objects
                        obj.GetComponent<Destructible>().Hit();
                        break;
                    case "Enemy":
                        // Check if This Attack Already Hit this Enemy
                        if (HitEntity(obj))
                        {
                            // Damage Calculation
                            // Enemy TakeDamage( Weapon GetDamage + Player Equipments/Stats )
                            weaponProficiency.IncreaseProficiency(obj.GetComponent<EnemyBase>().TakeDamage(eTypeOfDamage, GetDamage()));
                        }
                        break;
                }
            }
        }
        else
            ResetHits();
    }
}
