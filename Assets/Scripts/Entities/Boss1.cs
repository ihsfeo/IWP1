using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : EnemyBase
{
    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    // Movement
    bool IsGrounded;
    float Up;
    float Right;
    float TerminalVelocity = 8f;
    float FallSpeed = 1;

    // Attacks
    bool JumpAttack = false;
    float JumpAttackCD = 0;
    bool FrontalAttack = false;
    float FrontalAttackCD = 0;

    // Idle
    float IdleTime = 0;

    PlayerInfo Player;
    Direction FacingDirection;
    List<GameObject> CollisionObjects = new List<GameObject>();

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

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        Vector3 oldPosition = transform.position;
        Vector3 Direction = Player.transform.position - transform.position;
        JumpAttackCD -= Time.deltaTime;
        FrontalAttackCD -= Time.deltaTime;


        switch (eEnemyState)
        {
            case EnemyState.Attacking: {
                    if (JumpAttack && IsGrounded)
                    {
                        JumpAttack = false;
                        eEnemyState = EnemyState.Idle;
                        IdleTime = 1;

                        // Do Damage in area
                        if (Direction.x <= 3)
                        {
                            Player.TakeDamage(ItemBase.TypeOfDamage.Physical, 25);
                        }

                        break;
                    }
                    if (FrontalAttack && FrontalAttackCD <= 1)
                    {
                        FrontalAttack = false;
                        eEnemyState = EnemyState.Idle;
                        IdleTime = 1;

                        // Do Damage in area Depending on direction
                        if (FacingDirection == Boss1.Direction.Left && 
                            Direction.x >= -3 && Direction.x <= 0 && 
                            Mathf.Abs(Direction.y) < 2)
                        {
                            Player.TakeDamage(ItemBase.TypeOfDamage.Natural, 10);
                        }
                        else if (FacingDirection == Boss1.Direction.Right &&
                            Direction.x <= 3 && Direction.x >= 0 &&
                            Mathf.Abs(Direction.y) < 2)
                        {
                            Player.TakeDamage(ItemBase.TypeOfDamage.Natural, 10);
                        }
                    }

                    break;
                }
            case EnemyState.Chase: {
                    // If can attack
                    if (Mathf.Abs(Direction.x) > 10)
                        break;
                    if (JumpAttackCD <= 0)
                    {
                        // In Range
                        if (Mathf.Abs(Direction.x) >= 4)
                        {
                            eEnemyState = EnemyState.Attacking;
                            JumpAttack = true;
                            JumpAttackCD = 10;
                            // Jump Movement
                            break;
                        }
                    }
                    if (FrontalAttackCD <= 0)
                    {
                        // In Range
                        if (Mathf.Abs(Direction.x) <= 1)
                        {
                            eEnemyState = EnemyState.Attacking;
                            FrontalAttack = true;
                            FrontalAttackCD = 2;
                            break;
                        }
                    }

                    // If can't attack
                    if (Direction.x < 0) Right -= 1f * Time.deltaTime;
                    else Right += 1f * Time.deltaTime;

                    break;
                }
            case EnemyState.Idle: {
                    if (IdleTime <= 0) eEnemyState = EnemyState.Chase;
                    else IdleTime -= Time.deltaTime;
                    break;
                }
            case EnemyState.Dead:
                break;
        }

        if (JumpAttack)
        {
            if (JumpAttackCD >= 9)
            {
                transform.position += new Vector3(0, 5 * Time.deltaTime, 0);
                if (Direction.x < 0) transform.position -= new Vector3(5 * Time.deltaTime, 0, 0);
                else if (Direction.x > 0) transform.position += new Vector3(5 * Time.deltaTime, 0, 0);
            }
            else transform.position -= new Vector3(0, 5 * Time.deltaTime, 0);
        }
        else if (!FrontalAttack)
        {
            // Vertical
            Up -= Time.deltaTime / 2 * FallSpeed;
            if (Up < -TerminalVelocity / 2 * FallSpeed)
                Up = -TerminalVelocity / 2 * FallSpeed;
            else if (Up > TerminalVelocity / 2 * FallSpeed)
                Up = TerminalVelocity / 2 * FallSpeed;
            transform.position += new Vector3(0, Up, 0);

            // Horizontal
            transform.position += new Vector3(Right * 0.7f, 0, 0);
            if (Mathf.Abs(Right) <= 0.055f)
                Right /= 1.04f + Mathf.Abs(Right * 1.2f * 1.3f);
            else
                Right /= 1f + Mathf.Abs(Right * 0.69f * 1.3f);
        }

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
                    if (oldPosition.y - transform.localScale.y / 2>= obj.transform.position.y + obj.transform.localScale.y / 2)
                    {
                        if (transform.position.y - transform.localScale.y / 2 < obj.transform.position.y + obj.transform.localScale.y / 2)
                        {
                            transform.position = new Vector3(transform.position.x, obj.transform.position.y + obj.transform.localScale.y / 2 + transform.localScale.y / 2, 0);
                            IsGrounded = true;
                            Up = 0.0f;
                        }
                    }

                    // Under the block
                    else if (oldPosition.y + transform.localScale.y / 2 <= obj.transform.position.y - obj.transform.localScale.y / 2)
                    {
                        if (transform.position.y > obj.transform.position.y - obj.transform.localScale.y / 2 - 0.5f)
                        {
                            transform.position = new Vector3(transform.position.x, obj.transform.position.y - obj.transform.localScale.y / 2 - transform.localScale.y / 2, 0);
                            Up = 0.0f;
                        }
                    }
                    break;
                case "Platform":
                    // Stand on top
                    if (obj.transform.position.y + 0.5 > transform.position.y - transform.localScale.y / 2)
                    {
                        if (Mathf.Abs(oldPosition.x - obj.transform.position.x) <= obj.transform.localScale.x / 2 + 0.5 && oldPosition.y >= obj.transform.position.y + 1)
                        {
                            transform.position = new Vector3(transform.position.x, obj.transform.position.y + 0.5f + transform.localScale.y / 2, 0);
                            IsGrounded = true;
                            Up = 0.0f;
                        }
                    }
                    break;
            }
        }

        // Horizontal Collisions
        for (int i = 0; i < CollisionObjects.Count; i++)
        {
            GameObject obj = CollisionObjects[i];
            switch (obj.tag)
            {
                case "Destructible":
                case "Solid":
                    if (transform.position.y - transform.localScale.y / 2 >= obj.transform.position.y + obj.transform.localScale.y / 2 || transform.position.y + transform.localScale.y / 2 <= obj.transform.position.y - obj.transform.localScale.y / 2)
                        continue;

                    // Left of block
                    if (oldPosition.x + transform.localScale.x / 2<= obj.transform.position.x - obj.transform.localScale.x / 2)
                    {
                        if (transform.position.x > obj.transform.position.x - obj.transform.localScale.x / 2 - 0.5f)
                        {
                            transform.position = new Vector3(obj.transform.position.x - obj.transform.localScale.x / 2 - transform.localScale.x / 2, transform.position.y, 0);
                            Right = 0.0f;
                        }
                    }

                    // Right of block
                    else if (oldPosition.x - transform.localScale.x / 2 >= obj.transform.position.x + obj.transform.localScale.x / 2)
                    {
                        if (transform.position.x < obj.transform.position.x + obj.transform.localScale.x / 2 + 0.5f)
                        {
                            transform.position = new Vector3(obj.transform.position.x + obj.transform.localScale.x / 2 + transform.localScale.x / 2, transform.position.y, 0);
                            Right = 0.0f;
                        }
                    }
                    break;
                case "Platform":
                    break;
                case "Water":
                    break;
            }
        }
    }

    public override void Reset()
    {
        
    }
}
