using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy1 : EnemyBase
{
    enum EnemyState
    {
        Idle,
        Attacking,
        Dead,
        Chase
    }

    // N NE E SE S SW W NW 
    List<float> SteeringDesire = new List<float>();

    List<GameObject> CollisionObjects = new List<GameObject>();

    float Up;
    float Right;
    float TerminalVelocity = 4f;
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
        if (!InWater)
        {
            if (JumpCount == 0)
            {
                Up = 12f * Time.deltaTime;
                JumpCount++;
            }
        }
        else
        {
            Up += 1 * Time.deltaTime;
        }
    }

    void GoRight()
    {
        Right = 1f * Time.deltaTime;
    }

    void GoLeft()
    {
        Right = -1f * Time.deltaTime;
    }

    void dotIntent(Vector3 direction, float magnitude = 1)
    {
        for (int i = 0; i < 8; i++)
        {
            SteeringDesire[i] += Vector3.Dot(Quaternion.AngleAxis(45 * i, Vector3.back) * Vector3.up, direction.normalized) * magnitude;
        }
    }

    void dotIntent(Vector3 direction, float distance, float maxDistance)
    {
        for (int i = 0; i < 8; i++)
        {
            float value = Vector3.Dot(Quaternion.AngleAxis(45 * i, Vector3.back) * Vector3.up, direction.normalized) / maxDistance * (maxDistance - distance);
            if (value < 0)
                value = 0;

            SteeringDesire[i] -= value;
        }
    }

    void dotDangers()
    {
        // check 8 directions for obstacles
        for (int i = 0; i < 8; i++)
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            RaycastHit2D hit2D;
            LayerMask layerMask;
            layerMask = 1 << 2;
            hit2D = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(45 * i, Vector3.back) * Vector3.up, layerMask);

            gameObject.layer = LayerMask.NameToLayer("Default");

            if (hit2D.collider == null)
            {
                Debug.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(45 * i, Vector3.back) * Vector3.up * 5, Color.green);
                continue;
            }

            if (hit2D.distance <= 5 && hit2D.collider.gameObject.tag != "Player")
            {
                dotIntent(Quaternion.AngleAxis(45 * i, Vector3.back) * Vector3.up, hit2D.distance, 5);
                Debug.DrawLine(transform.position, hit2D.point, Color.red);
            }
        }
    }

    int GetMostIntent()
    {
        int most = 0;
        for (int i = 0; i < 8; i++)
        {
            if (SteeringDesire[i] > SteeringDesire[most])
                most = i;
        }
        return most;
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
                    dotIntent(CollisionObjects[i].transform.position - transform.position, 5);
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

    void PrintDot()
    {
        Debug.Log("Dot: " + SteeringDesire[0] + ", " + SteeringDesire[1] + ", " + SteeringDesire[2] + ", " + SteeringDesire[3] + ", " + SteeringDesire[4] + ", " + SteeringDesire[5] + ", " + SteeringDesire[6] + ", " + SteeringDesire[7]);
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

        if (SeePlayer())
        {
            // Debug.Log("Sheesh");
        }

        dotDangers();

        // Decisions
        {
            /*
             Move Left
            Move Right
            Jump
            Attacks
             */

            int MostIntent = GetMostIntent();
            if (SteeringDesire[MostIntent] > 0)
            {
                switch (MostIntent)
                {
                    case 0: // Up
                        Jump();
                        break;
                    case 1: // Up Right
                        Jump();
                        GoRight();
                        break;
                    case 2: // Right
                        GoRight();
                        break;
                    case 3: // Down Right
                        GoRight();
                        break;
                    case 4: // Down
                        break;
                    case 5: // Down Left
                        GoLeft();
                        break;
                    case 6: // Left
                        GoLeft();
                        break;
                    case 7: // Up Left
                        Jump();
                        GoLeft();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // Idle

            }
        }

        Up -= Time.deltaTime / 2 * FallSpeed;
        if (Up < -TerminalVelocity / 2 * FallSpeed)
            Up = -TerminalVelocity / 2 * FallSpeed;
        else if (Up > TerminalVelocity / 2 * FallSpeed)
            Up = TerminalVelocity / 2 * FallSpeed;
        transform.position += new Vector3(0, Up, 0);

        // Vertical Collisions
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

        PrintDot();
        // Reset Intent
        dotClear();
    }
    
}
