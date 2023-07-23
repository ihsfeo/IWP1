using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy1 : EnemyBase
{
    [SerializeField] ItemManager itemManager;
    // Starts from N clockwise
    List<float> SteeringDesire = new List<float>();
    List<GameObject> CollisionObjects = new List<GameObject>();
    List<GameObject> DetectedObjects = new List<GameObject>();

    float Up;
    float Right;
    float TerminalVelocity = 4f;
    float FallSpeed = 1;

    int JumpCount = 0;
    bool InWater = false;

    float TargetSpotted = 0;
    Vector3 TargetLocation;
    bool TargetInSight = false;

    float AttackTime = 0;

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

    void GoUp()
    {
        if (!status.IsShocked)
            Up = 1f * Time.deltaTime;
    }

    void GoDown()
    {
        if (!status.IsShocked)
            Up = -1f * Time.deltaTime;
    }

    void GoRight()
    {
        if (!status.IsShocked)
            Right = 1f * Time.deltaTime;
    }

    void GoLeft()
    {
        if (!status.IsShocked)
            Right = -1f * Time.deltaTime;
    }

    void dotIntent(Vector3 direction, float magnitude = 1)
    {
        for (int i = 0; i < 12; i++)
        {
            SteeringDesire[i] += Vector3.Dot(Quaternion.AngleAxis(30 * i, Vector3.back) * Vector3.up, direction.normalized) * magnitude;
        }
    }

    void dotIntent(Vector3 direction, float distance, float maxDistance)
    {
        if (distance < 2f)
        {
            for (int i = 0; i < 12; i++)
            {
                float value = 0;

                Vector3 temp = Quaternion.AngleAxis(30 * i, Vector3.back) * Vector3.up;
                if (direction == temp)
                {
                    SteeringDesire[i] = 0;
                    continue;
                }
                value = Vector3.Dot(Quaternion.AngleAxis(30 * i, Vector3.back) * Vector3.up, direction.normalized) * 3;

                if (value < 0)
                    continue;
                else if (value > 0)
                {
                    if (SteeringDesire[i] > 0 && SteeringDesire[i] - value <= 0)
                        SteeringDesire[i] = 0.1f;
                    else
                        SteeringDesire[i] -= value;
                }

                // SteeringDesire[i] -= value;
            }
        }
    }

    void dotDangers()
    {
        // check 12 directions for obstacles
        for (int i = 0; i < 12; i++)
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            RaycastHit2D hit2D;
            LayerMask layerMask;
            layerMask = 1 << 2;
            hit2D = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(30 * i, Vector3.back) * Vector3.up, 10);

            gameObject.layer = LayerMask.NameToLayer("Default");

            if (hit2D.collider == null)
            {
                Debug.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(30 * i, Vector3.back) * Vector3.up * 10, Color.green);
                continue;
            }

            if (hit2D.distance <= 10 && hit2D.collider.gameObject.tag != "Player")
            {
                dotIntent(Quaternion.AngleAxis(30 * i, Vector3.back) * Vector3.up, hit2D.distance, 10);
                Debug.DrawLine(transform.position, hit2D.point, Color.red);
            }
        }
    }

    int GetMostIntent()
    {
        int most = 0;
        for (int i = 0; i < 12; i++)
        {
            if (SteeringDesire[i] > SteeringDesire[most])
                most = i;
        }

        if (SteeringDesire[most] == 0)
            return 12;
        return most;
    }

    bool SeePlayer()
    {
        for (int i = 0; i < DetectedObjects.Count; i++)
        {
            if (DetectedObjects[i].tag == "Player")
            {
                gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                RaycastHit2D hit2D;

                // While does not hit player, raycast 5 times, 4 corners and middle
                hit2D = Physics2D.Raycast(transform.position, DetectedObjects[i].transform.position - transform.position, 10);
                //Debug.DrawRay(transform.position, CollisionObjects[i].transform.position - transform.position, Color.red, 1.0f);
                gameObject.layer = LayerMask.NameToLayer("Default");
                if (hit2D.collider == null || hit2D.collider.gameObject.tag != "Player")
                    return false;

                dotIntent(DetectedObjects[i].transform.position - new Vector3(hit2D.point.x, hit2D.point.y, 0), 5);
                TargetSpotted = 3;
                TargetLocation = new Vector3(hit2D.point.x, hit2D.point.y, 0);
                return true;
            }
        }
        return false;
    }

    bool Attacking()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        AttackTime -= Time.deltaTime;
        if (AttackTime > 0)
            return false;

        target.GetComponent<PlayerInfo>().TakeDamage(ItemBase.TypeOfDamage.Ice, 5);
        AttackTime = 1;

        return true;
    }

    void dotClear()
    {
        for (int i = 0; i < 12; i++)
        {
            SteeringDesire[i] = 0;
        }
    }

    void PrintDot()
    {
        Debug.Log("Dot: " + SteeringDesire[0] + ", " + SteeringDesire[1] + ", " + SteeringDesire[2] + ", " + SteeringDesire[3] + ", " + SteeringDesire[4] + ", " + SteeringDesire[5] + ", " + SteeringDesire[6] + ", " + SteeringDesire[7] + ", " + SteeringDesire[8] + ", " + SteeringDesire[9] + ", " + SteeringDesire[10] + ", " + SteeringDesire[11]);
    }

    private void Awake()
    {
        status = GetComponent<Status>();
        status.InitStats();
        ShieldMax = 100;
        Shield = ShieldMax;
        ShieldType = ItemBase.TypeOfDamage.Physical;
        Level = 1;
        // ShieldWeakness ?

        eEnemyType = EnemyType.Normal;
        eEnemyVariation = EnemyVariation.Basic;
        eEnemyMovementType = EnemyMovementType.Normal;

        status.SetMaxHealth(100);
        status.Health = (int)status.GetStat(CStats.Stats.MaxHealth);
        BaseDamage = 5;
        for (int i = 0; i < 12; i++)
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
        DetectedObjects = transform.GetChild(0).GetComponent<DetectionRange>().DetectedObjects;
        Vector3 oldPosition = transform.position;

        if (DamageTime > 0)
        {
            DamageTime -= Time.deltaTime;
            if (DamageTime <= 0)
            {
                DamageTime = 0;
                GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            }
        }

        TargetInSight = SeePlayer();

        dotDangers();

        // Decisions
        {
            /*
             Move Left
            Move Right
            Jump
            Attacks
             */
            if (status.Health <= 0)
            {
                eEnemyState = EnemyState.Dead;
            }

            switch (eEnemyState)
            {
                case EnemyState.Idle:
                    // Walks around a safe location about current position
                    if (TargetInSight && (transform.position - TargetLocation).magnitude < 2)
                        eEnemyState = EnemyState.Attacking;
                    if (TargetSpotted > 0)
                        eEnemyState = EnemyState.Chase;


                    break;
                case EnemyState.Attacking:
                    // Pick what attack is happening
                    Attacking();
                    eEnemyState = EnemyState.Idle;

                    break;
                case EnemyState.Chase:
                    // Going to last seem player location
                    if (TargetSpotted <= 0)
                        eEnemyState = EnemyState.Idle;

                    if (TargetInSight && (transform.position - TargetLocation).magnitude < 2)
                        eEnemyState = EnemyState.Attacking;

                    TargetSpotted -= Time.deltaTime;
                    dotIntent(TargetLocation - transform.position, 5);

                    break;
                case EnemyState.Dead:
                    // Death Animation
                    // Drop Loot etc
                    // Destroy
                    DroppedItem temp = GameObject.Instantiate(itemManager.droppedItem).GetComponent<DroppedItem>();
                    temp.transform.position = transform.position;
                    temp.Init(ItemBase.ItemID.WeaponFragment1, Random.Range(2, 5), itemManager);
                    Destroy(gameObject);
                    break;
            }

            int MostIntent;
            if ((MostIntent = GetMostIntent()) < 12)
            {
                if (SteeringDesire[MostIntent] > 0)
                {
                    switch (MostIntent)
                    {
                        case 0: // Up
                            GoUp();
                            break;
                        case 1: // Up Up Right
                            GoRight();
                            GoUp();
                            break;
                        case 2: // Up Right Right
                            GoUp();
                            GoRight();
                            break;
                        case 3: // Right
                            GoRight();
                            break;
                        case 4: // Down Right Right
                            GoRight();
                            GoDown();
                            break;
                        case 5: // Down down Right
                            GoDown();
                            GoRight();
                            break;
                        case 6: // Down
                            GoDown();
                            break;
                        case 7: // Down Down Left
                            GoDown();
                            GoLeft();
                            break;
                        case 8: // Down Left Left
                            GoDown();
                            GoLeft();
                            break;
                        case 9: // Left
                            GoLeft();
                            break;
                        case 10: // Up Left Left
                            GoUp();
                            GoLeft();
                            break;
                        case 11: // Up Up Left
                            GoLeft();
                            GoUp();
                            break;
                        default: break;
                    }
                }
            }
        }

        //Up -= Time.deltaTime / 2 * FallSpeed;
        //if (Up < -TerminalVelocity / 2 * FallSpeed)
        //    Up = -TerminalVelocity / 2 * FallSpeed;
        //else if (Up > TerminalVelocity / 2 * FallSpeed)
        //    Up = TerminalVelocity / 2 * FallSpeed;
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
        for (int i = 0; i < status.StatusAilmentsList.Count; i++)
        {
            if (!status.StatusAilmentsList[i].UpdateAilment(status))
            {
                status.StatusAilmentsList.Remove(status.StatusAilmentsList[i]); // Remove
                i--;
            }
        }

        // PrintDot();
        // Reset Intent
        dotClear();
    }

    public override void Reset()
    {
        status.Health = (int)status.GetStat(CStats.Stats.MaxHealth);
        eEnemyState = EnemyState.Idle;

        Right = 0;
        Up = 0;
        TargetInSight = false;
    }
}
