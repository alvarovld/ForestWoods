using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviour = GameData.Enums.BehaviourEnum;
public class EnemyAnimatorHandler : MonoBehaviour
{
    Behaviour behaviour;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayGetHit()
    {
        animator.Play("GetHit");
        animator.SetBool("KeepGuardStand", true);
    }

    public void PlayMeleeAttack()
    {
        animator.Play("Attack");
        animator.SetBool("KeepGuardStand", true);
    }

    public void UpdateAnimator(Behaviour behaviour)
    {
        this.behaviour = behaviour;
    }

    void UpdateParameters()
    {
        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("KeepGuardLeft", false);
        animator.SetBool("KeepGuardRight", false);
        animator.SetBool("RunBackwards", false);

        switch (behaviour)
        {
            case Behaviour.Attack:
                animator.SetBool("Run", true);
                animator.SetBool("Walk", false);
                animator.SetBool("KeepGuardLeft", false);
                animator.SetBool("KeepGuardRight", false);
                animator.SetBool("RunBackwards", false);
                break;
            case Behaviour.RunBackwards:
                animator.SetBool("RunBackwards", true);
                animator.SetBool("Run", false);
                animator.SetBool("Walk", false);
                animator.SetBool("KeepGuardLeft", false);
                animator.SetBool("KeepGuardRight", false);
                break;
            case Behaviour.KeepGuardLeft:
                animator.SetBool("KeepGuardLeft", true);
                animator.SetBool("Run", false);
                animator.SetBool("Walk", false);
                animator.SetBool("KeepGuardRight", false);
                animator.SetBool("RunBackwards", false);
                break;
            case Behaviour.KeepGuardRight:
                animator.SetBool("KeepGuardRight", true);
                animator.SetBool("Run", false);
                animator.SetBool("Walk", false);
                animator.SetBool("KeepGuardLeft", false);
                animator.SetBool("RunBackwards", false);
                break;
            case Behaviour.KeepGuardStand:
                animator.SetBool("KeepGuardStand", true);
                animator.SetBool("Run", false);
                animator.SetBool("Walk", false);
                animator.SetBool("KeepGuardLeft", false);
                animator.SetBool("KeepGuardRight", false);
                animator.SetBool("RunBackwards", false);
                break;
            case Behaviour.WalkFree:
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
                animator.SetBool("KeepGuardStand", false);
                animator.SetBool("KeepGuardLeft", false);
                animator.SetBool("KeepGuardRight", false);
                animator.SetBool("RunBackwards", false);
                break;
            case Behaviour.WalkToCampfire:
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
                animator.SetBool("KeepGuardStand", false);
                animator.SetBool("KeepGuardLeft", false);
                animator.SetBool("KeepGuardRight", false);
                animator.SetBool("RunBackwards", false);
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!PlayerSituationAwareness.IsEnemyAwayeOfPlayer(transform))
        {
            animator.SetBool("Idle", true);
            animator.SetBool("KeepGuardStand", false);
        }
        else
        {
            animator.SetBool("Idle", false);
            animator.SetBool("KeepGuardStand", true);
        }

        UpdateParameters();
    }



}
