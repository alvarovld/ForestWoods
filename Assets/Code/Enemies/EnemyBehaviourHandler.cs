using System;
using System.Collections;
using UnityEngine;
using Utils;
using Behaviour = GameData.Enums.BehaviourEnum;
public class EnemyBehaviourHandler : MonoBehaviour
{
    public Behaviour behaviour;
    public bool behaviourGeneratorLogs;
    bool stopAttack;
    BehaviourGenerator generator;

    private void Start()
    {
        generator = new BehaviourGenerator(transform, behaviourGeneratorLogs);
        SetNewBehaviour();
        stopAttack = false;
    }

    private void OnEnable()
    {
        SetNewBehaviour();
    }

    void SetNewBehaviour()
    {
        SetRandomBehaviour();
    }

    float DistanceToPlayer()
    {
        return (GameObjectRefs.player.transform.position - transform.position).magnitude;
    }

    void CallSetNewBehaviourWhenCloseToCampfire()
    {
        var stats = gameObject.GetComponent<EnemyStats>();
        Func<bool> condition = () =>
        {
            if ((transform.position - stats.closestCampfire).magnitude <= 
                stats.minDistanceToCampfire ||
                PlayerSituationAwareness.IsEnemyAwayeOfPlayer(transform))
            {
                return true;
            }
            return false;
        };
        StartCoroutine(CallSetNewBehaviourWhenConditionApplies(condition));
    }


    void SetNewBehaviourCallback()
    {
        float minTime = 0;
        float maxTime = 0;
        switch (behaviour)
        {
            case Behaviour.Attack:
                CallSetNewBehaviourWhenPerformAttackAppliesOrTimesOut();
                return;
            case Behaviour.WalkToCampfire:
                CallSetNewBehaviourWhenCloseToCampfire();
                return;
            case Behaviour.RunBackwards:
                minTime = 0.7f;
                maxTime = 1f;
                break;
            case Behaviour.KeepGuardLeft:
                minTime = 2;
                maxTime = 5;
                break;
            case Behaviour.KeepGuardRight:
                minTime = 2;
                maxTime = 5;
                break;
            case Behaviour.KeepGuardStand:
                minTime = 1f;
                maxTime = 4f;
                break;
            case Behaviour.PerformAttack:
                minTime = 0.3f;
                maxTime = 0.8f;
                break;
            case Behaviour.Idle:
                minTime = 3f;
                maxTime = 4f;
                break;
            case Behaviour.WalkFree:
                minTime = 3f;
                maxTime = 4f;
                break;
            default:
                Debug.LogWarning("[EnemyBehaviourHanlder] [setBehaviourFinishCallBack] unknown behaviour: " + behaviour);
                return;
        }
        var actionTime = GetActionTime(minTime, maxTime);
        StartCoroutine(CallSetNewBehaviour(actionTime));
    }

    float GetActionTime(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }


    public void SetRandomBehaviour()
    {
        if(generator == null)
        {
            return;
        }
        behaviour = generator.GetBehaviour();
        SetNewBehaviourCallback();
    }

    public Behaviour GetBehaviour()
    {
        return behaviour;
    }

    IEnumerator CallSetNewBehaviour(float actionTime)
    {
        yield return new WaitForSeconds(actionTime);
        SetNewBehaviour();
    }

    bool AttackUntilCondition()
    {
        return DistanceToPlayer() <= GetComponent<EnemyStats>().performAttackDistance ||
                DistanceToPlayer() >= GetComponent<EnemyStats>().stopAttackDistance ||
               !PlayerSituationAwareness.IsEnemyAwayeOfPlayer(transform) ||
               stopAttack == true;
    }

    IEnumerator StopAttackTimer()
    {
        yield return new WaitForSeconds(GetComponent<EnemyStats>().stopAttackTime);
        stopAttack = true;
    }

    public void CallSetNewBehaviourWhenPerformAttackAppliesOrTimesOut()
    {
        stopAttack = false;
        StartCoroutine(StopAttackTimer());
        StartCoroutine(CallSetNewBehaviourWhenConditionApplies(AttackUntilCondition));
    }

    IEnumerator CallSetNewBehaviourWhenConditionApplies(Func<bool> condition)
    {
        yield return new WaitUntil(condition);
        SetNewBehaviour();
    }
}
