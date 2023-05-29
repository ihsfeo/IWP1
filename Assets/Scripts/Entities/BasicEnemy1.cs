using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy1 : EnemyBase
{
    // N NE E SE S SW W NW 
    List<float> SteeringDesire = new List<float>();

    List<GameObject> CollisionObjects = new List<GameObject>();

    float Up;
    float Right;
    float TerminalVelocity = 0.15f;
    float FallSpeed = 1;

    int JumpCount = 0;
    bool InWater = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged" &&
            collision.gameObject.tag != "Destructible")
            CollisionObjects.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged" &&
            collision.gameObject.tag != "Destructible")
            CollisionObjects.Remove(collision.gameObject);
    }

    void Jump()
    {
        if (JumpCount < 1)
        {
            Up = 0.1f;
            JumpCount++;
        }
    }

    void dotIntent(Vector3 direction, bool positive)
    {
        for (int i = 0; i < 8; i++)
        {
            if (positive)
                SteeringDesire[i] += Vector3.Dot(Quaternion.AngleAxis(45 * i, Vector3.back) * Vector3.up, direction.normalized);
            else
                SteeringDesire[i] -= Vector3.Dot(Quaternion.AngleAxis(45 * i, Vector3.back) * Vector3.up, direction.normalized);
        }
    }

    bool SeePlayer()
    {
        for (int i = 0; i < CollisionObjects.Count; i++)
        {
            if (CollisionObjects[i].tag == "Player")
            {
                gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                RaycastHit2D hit2D;
                LayerMask layerMask;
                layerMask = 1 << 2;
                hit2D = Physics2D.Raycast(transform.position, CollisionObjects[i].transform.position - transform.position, layerMask);
                // Debug.DrawRay(transform.position, CollisionObjects[i].transform.position - transform.position, Color.red, 1.0f);
                gameObject.layer = LayerMask.NameToLayer("Default");
                if (hit2D.collider == null)
                    return false;
                if (hit2D.collider.gameObject.tag == "Player")
                {
                    dotIntent(CollisionObjects[i].transform.position - transform.position, true);
                    return true;
                }
                else
                    return false;
            }
        }
        return false;
    }

    void dotClear()
    {
        for (int i = 0; i < 8; i++)
        {
            SteeringDesire[i] = 0;
        }
    }

    private void Awake()
    {
        ShieldMax = 100;
        Shield = ShieldMax;
        ShieldType = WeaponBase.TypeOfDamage.Physical;
        // ShieldWeakness ?

        eEnemyType = EnemyType.Normal;
        eEnemyVariation = EnemyVariation.Basic;
        eEnemyMovementType = EnemyMovementType.Normal;

        HealthMax = 100;
        Health = HealthMax;
        BaseDamage = 5;
        for (int i = 0; i < 8; i++)
            SteeringDesire.Add(0);
    }

    /*
    Actions
    1. Movement AD
    2. Jump, When can Jump and wants to jump
    3. Attack when can attack and target
     */

    private void Update()
    {
        Vector3 oldPosition = transform.position;

        Up -= Time.deltaTime / 8 * FallSpeed;
        if (Up < -TerminalVelocity / 2 * FallSpeed)
            Up = -TerminalVelocity / 2 * FallSpeed;
        else if (Up > TerminalVelocity / 2 * FallSpeed)
            Up = TerminalVelocity / 2 * FallSpeed;
        transform.position += new Vector3(0, Up, 0);

        if (SeePlayer())
        {
            // Debug.Log("Sheesh");
        }

        for (int i = 0; i < CollisionObjects.Count; i++)
        {

        }

        for (int i = 0; i < CollisionObjects.Count; i++)
        {
            GameObject obj = CollisionObjects[i];
            switch (obj.tag)
            {
                case "Destructible":
                case "Solid":
                    // Check if raycast into top/bottom lines

                    if (transform.position.x <= obj.transform.position.x - obj.transform.localScale.x / 2 - 0.5f || transform.position.x >= obj.transform.position.x + obj.transform.localScale.x / 2 + 0.5f)
                        continue;
                    // Above the block
                    if (oldPosition.y >= obj.transform.position.y + obj.transform.localScale.y / 2 + 0.5f)
                    {
                        if (transform.position.y < obj.transform.position.y + obj.transform.localScale.y / 2 + 0.5f)
                        {
                            transform.position = new Vector3(transform.position.x, obj.transform.position.y + obj.transform.localScale.y / 2 + 0.5f, 0);
                            JumpCount = 0;
                            Up = 0.0f;
                        }
                    }

                    // Under the block
                    else if (oldPosition.y <= obj.transform.position.y - obj.transform.localScale.y / 2 - 0.5f)
                    {
                        if (transform.position.y > obj.transform.position.y - obj.transform.localScale.y / 2 - 0.5f)
                        {
                            transform.position = new Vector3(transform.position.x, obj.transform.position.y - obj.transform.localScale.y / 2 - 0.5f, 0);
                            Up = 0.0f;
                        }
                    }
                    break;
                case "Platform":
                    // Stand on top
                    if (obj.transform.position.y + 0.5 > transform.position.y - 0.5)
                    {
                        if (Mathf.Abs(oldPosition.x - obj.transform.position.x) <= obj.transform.localScale.x / 2 + 0.5 && oldPosition.y >= obj.transform.position.y + 1)
                        {
                            transform.position = new Vector3(transform.position.x, obj.transform.position.y + 1, 0);
                            Up = 0.0f;
                            JumpCount = 0;
                        }
                    }
                    break;
            }
        }

        if (!InWater)
        {
            transform.position += new Vector3(Right, 0, 0);
            Right /= 1 + 10 * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(Right * 0.7f, 0, 0);
            if (Mathf.Abs(Right) <= 0.055f)
                Right /= 1.04f + Mathf.Abs(Right * 1.2f * 1.3f);
            else
                Right /= 1f + Mathf.Abs(Right * 0.69f * 1.3f);
        }

        InWater = false;

        for (int i = 0; i < CollisionObjects.Count; i++)
        {
            GameObject obj = CollisionObjects[i];
            switch (obj.tag)
            {
                case "Destructible":
                case "Solid":
                    if (transform.position.y >= obj.transform.position.y + obj.transform.localScale.y / 2 + 0.5f || transform.position.y <= obj.transform.position.y - obj.transform.localScale.y / 2 - 0.5f)
                        continue;

                    // Left of block
                    if (oldPosition.x <= obj.transform.position.x - obj.transform.localScale.x / 2 - 0.5f)
                    {
                        if (transform.position.x > obj.transform.position.x - obj.transform.localScale.x / 2 - 0.5f)
                        {
                            transform.position = new Vector3(obj.transform.position.x - obj.transform.localScale.x / 2 - 0.5f, transform.position.y, 0);
                            Right = 0.0f;
                        }
                    }

                    // Right of block
                    else if (oldPosition.x >= obj.transform.position.x + obj.transform.localScale.x / 2 + 0.5f)
                    {
                        if (transform.position.x < obj.transform.position.x + obj.transform.localScale.x / 2 + 0.5f)
                        {
                            transform.position = new Vector3(obj.transform.position.x + obj.transform.localScale.x / 2 + 0.5f, transform.position.y, 0);
                            Right = 0.0f;
                        }
                    }
                    break;
                case "Platform":
                    break;
                case "Water":
                    if (transform.position.x - transform.localScale.x / 2 < obj.transform.position.x + obj.transform.localScale.x / 2 &&
                       transform.position.x + transform.localScale.x / 2 > obj.transform.position.x - obj.transform.localScale.x / 2 &&
                       transform.position.y - transform.localScale.y / 2 < obj.transform.position.y + obj.transform.localScale.y / 2 &&
                       transform.position.y > obj.transform.position.y - obj.transform.localScale.y / 2 &&
                       obj.transform.localScale.y > 0.5)
                    {
                        InWater = true;
                    }
                    break;
            }
        }


        // Status Ailment Update
        for (int i = 0; i < StatusAilmentsList.Count; i++)
        {
            if (!StatusAilmentsList[i].UpdateAilment())
                StatusAilmentsList.Remove(StatusAilmentsList[i]); // Remove
        }

        // Reset Intent
        dotClear();
    }
    
}
