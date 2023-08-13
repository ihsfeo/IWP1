using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBow : ItemBase
{
    [SerializeField] PlayerInfo playerInfo;

    List<GameObject> HitList = new List<GameObject>();
    List<GameObject> CollisionObjects = new List<GameObject>();

    protected TypeOfDamage eTypeOfDamage;

    protected WeaponProficiency weaponProficiency;

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
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
    public override void Init()
    {
        playerInfo = FindObjectOfType<PlayerInfo>();
        eTypeOfDamage = TypeOfDamage.Natural;
        BaseDamage = 10;
        weaponProficiency = new WeaponProficiency(Rarity);
        eWeaponType = WeaponType.Bow;
        Level = 1;
        MaxCount = 1;
        Count = 1;
        // Tree
        // BonusStats
    }

    // Update is called once per frame
    void Update()
    {
        // when you attack spawn arrow with stats
    }
}
