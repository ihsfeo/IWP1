using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    ItemBase.TypeOfDamage DamageType;
    int Damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.transform.GetComponent<EnemyBase>().TakeDamage(DamageType, Damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Destructible")
        {
            collision.gameObject.GetComponent<Destructible>().Hit();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Untagged")
        {
            Destroy(gameObject);
        }
    }

    public void Init(int damage, ItemBase.TypeOfDamage type)
    {
        DamageType = type;
        Damage = damage;
    }
}
