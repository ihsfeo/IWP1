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

    private void Awake()
    {
        
    }

    private void Update()
    {
        Vector3 oldPosition = transform.position;
        JumpAttackCD -= Time.deltaTime;
        FrontalAttackCD -= Time.deltaTime;


        switch (eEnemyState)
        {
            case EnemyState.Attacking: {
                    Vector3 Direction = Player.transform.position - transform.position;
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
                    Vector3 Direction = Player.transform.position - transform.position;
                    // If can attack
                    if (JumpAttackCD <= 0)
                    {
                        // In Range
                        if (Direction.x >= 4)
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
                        if (Direction.x <= 1)
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
    }

    public override void Reset()
    {
        
    }
}
