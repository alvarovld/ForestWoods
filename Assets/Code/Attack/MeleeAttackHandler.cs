using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tag = GameData.Tags;

public class MeleeAttackHandler : MonoBehaviour
{
    float hitStrength;
    GameObject currentTarget;
    public string targetTag;

    private void Start()
    {
        hitStrength = Manager.GetHitStrength(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals(targetTag))
        {
            currentTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(targetTag))
        {
            currentTarget = null;
        }
    }

    bool IsAttackingTarget()
    {
        return currentTarget != null;
    }

    public void Attack()
    {
        Manager.Attack(gameObject);
    }


    public void SendHitToTarget()
    {
        if(!IsAttackingTarget())
        {
            return;
        }
        Manager.SendHit(currentTarget, hitStrength);
        //Debug.Log("[MeleeAttackHandler] GameObject: "+ gameObject.name+" sent attack to target: " + currentTarget);
    }


    // Manager class to handle either enemy or player actions

    private class Manager
    {
        public static float GetHitStrength(GameObject gameObject)
        {
            switch(gameObject.tag)
            {
                case Tag.Player:
                    return PlayerStats.strength;
                case Tag.Enemy:
                    return gameObject.GetComponent<EnemyStats>().strength;
                default:
                    Debug.LogError("[MeleeAttackHandler] Unknown tag: " + gameObject.tag);
                    return -1;
            }
        }
        public static void SendHit(GameObject other, float health)
        {
            switch (other.tag)
            {
                case Tag.Enemy:
                    EnemyGetHit(other, health);
                    break;
                case Tag.Player:
                    PlayerGetHit(other, health);
                    break;
                default:
                    Debug.LogError("[HitHandlerManager] Tag not found: " + other.tag);
                    break;

            }
        }

        static void EnemyGetHit(GameObject other, float health)
        {
            other.GetComponent<EnemyStats>().ReduceHealth(health);
            other.GetComponent<EnemyAnimatorHandler>().PlayGetHit();
        }

        static void PlayerGetHit(GameObject other, float health)
        {
            PlayerStats.ReduceHealth(health);
            other.GetComponent<PlayerAnimatorHandler>().PlayGetHit();
        }
        public static void Attack(GameObject obj)
        {
            if (obj.GetComponent<EnemyAnimatorHandler>() != null)
            {
                obj.GetComponent<EnemyAnimatorHandler>().PlayMeleeAttack();
                return;
            }
            else if (obj.GetComponent<PlayerAnimatorHandler>() != null)
            {
                obj.GetComponent<PlayerAnimatorHandler>().PlayMeleeAttack();
                return;
            }
            Debug.LogError("[MeleeAttackHandler] No Animator handler found for GameObject: " + obj.name);
        }
    }
}

