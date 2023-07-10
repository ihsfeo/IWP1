using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIdle : StateMachineBehaviour
{
    PlayerInfo player;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsSword", false);
        animator.SetBool("IsSpear", false);
        animator.SetBool("IsBow", false);
        switch (player.GetWeaponType())
        {
            case ItemBase.WeaponType.Sword:
                animator.SetBool("IsSword", true);
                break;
            case ItemBase.WeaponType.Spear:
                animator.SetBool("IsSpear", true);
                break;
            case ItemBase.WeaponType.Bow:
                animator.SetBool("IsBow", true);
                break;
            case ItemBase.WeaponType.Total:
            default:
                break;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetBool("LeftClick", true);
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
