using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Behaviour = GameData.Enums.BehaviourEnum;

public class BehaviourGenerator
{
    Behaviour behaviour = Behaviour.Unknown;
    bool enableLog;
    Transform enemy;


    Dictionary<Behaviour, float> behaviourProbs = new Dictionary<Behaviour, float>()
    {
        { Behaviour.RunBackwards, 0 },
        { Behaviour.KeepGuard, 0 },
        { Behaviour.Attack, 0 },
        { Behaviour.PerformAttack, 0 },
        { Behaviour.Idle, 0 },
        { Behaviour.WalkFree, 0 },
        { Behaviour.WalkToCampfire, 0 }
    };

    public BehaviourGenerator()
    {
        behaviour = Behaviour.Idle;
        ResetValues();
    }

    public Behaviour GetBehaviour()
    {
        ResetValues();
        CalculateProbabilities();
        SetBehaviour();

        if (enableLog)
        {
            Debug.Log("[BehaviourGenerator] Behaviour set: " + behaviour);
        }

        return behaviour;   
    }

    public BehaviourGenerator(Transform enemy, bool enableLog)
    {
        behaviour = Behaviour.Unknown;
        this.enemy = enemy;
        this.enableLog = enableLog;
        SetDefaultProbabilitie();
    }

    bool EnemyInDangerZone()
    {
        return distanceToPlayer() <= enemy.GetComponent<EnemyStats>().increaseAttackProbabilitiesDistance;
    }

    float distanceToPlayer()
    {
        return (GameObjectRefs.player.transform.position - enemy.position).magnitude;
    }

    bool EnemyInPerformAttackDistance()
    {
        return distanceToPlayer() <= enemy.GetComponent<EnemyStats>().performAttackDistance;
    }

    bool EnemyJustLostPlayer()
    {
        return !PlayerSituationAwareness.IsEnemyAwayeOfPlayer(enemy) &&
               behaviour != Behaviour.WalkFree &&
               behaviour != Behaviour.Idle &&
               behaviour != Behaviour.Unknown;
    }

    bool EnemyAwareOfPlayer()
    {
        return PlayerSituationAwareness.IsEnemyAwayeOfPlayer(enemy);
    }

    Vector3 GetClosestCampfire(GameObject[] campfires)
    {
        if (campfires.Length <= 0)
        {
            return Vector3.zero;
        }

        Vector3 closestCampfire = campfires[0].transform.position;
        foreach (var it in campfires)
        {
            if ((closestCampfire - enemy.transform.position).magnitude >
               (it.transform.position - enemy.transform.position).magnitude)
            {
                closestCampfire = it.transform.position;
            }
        }
        return closestCampfire;
    }

    Vector3 GetClosestVisibleCampfire()
    {
        var campfires = GameObject.FindGameObjectsWithTag(GameData.Tags.CampFire);
        if(campfires.Length == 0)
        {
            return Vector3.zero;
        }

        var closestCampfire = GetClosestCampfire(campfires);
        if(closestCampfire.Equals(Vector3.zero))
        {
            return Vector3.zero;
        }

        var distanceToCampfire = (closestCampfire - enemy.transform.position).magnitude;

        if (distanceToCampfire <= enemy.GetComponent<EnemyStats>().lightVisionDistance &&
            distanceToCampfire >= enemy.GetComponent<EnemyStats>().maxDistanceToCampfire)
        {
            return closestCampfire;
        }
        return Vector3.zero;
    }




    private void CalculateProbabilities()
    {
        if (EnemyJustLostPlayer())
        {
            SetIdle();
            return;
        }

       var closestCampfire = GetClosestVisibleCampfire();
        if(!closestCampfire.Equals(Vector3.zero))
        {
            enemy.GetComponent<EnemyStats>().closestCampfire = closestCampfire;
            if (!EnemyAwareOfPlayer())
            {
                SetWalkToCampfire();
                return;
            }
        }


        if(!EnemyAwareOfPlayer())
        {
            SetEnemyNotAwareOfPlayer();
            return;
        }

        if(EnemyInDangerZone() &&
           !EnemyInPerformAttackDistance() &&
           EnemyLowHealth())
        {
            SetAttackCloseToPlayerAndLowHealtProb();
            return;
        }

        if(EnemyInDangerZone() &&
           !EnemyInPerformAttackDistance() &&
           !EnemyLowHealth())
        {
            SetAttackCloseToPlayerProb();
            return;
        }

        if(!EnemyInDangerZone() &&
           !EnemyInPerformAttackDistance() &&
            EnemyLowHealth())
        {
            SetLowHealthProb();
            return;
        }

        if (!EnemyInDangerZone() &&
            !EnemyInPerformAttackDistance() &&
            !EnemyLowHealth())
        {
            SetAttackCloseToPlayerProb();
            return;
        }

        if(EnemyInPerformAttackDistance() &&
           !EnemyLowHealth())
        {
            SetPerformAttackProb();
            return;
        }

        if (EnemyInPerformAttackDistance() &&
            EnemyLowHealth())
        {
            SetPerformAttackLowHealthProb();
            return;
        }

        SetDefaultProbabilitie();
        
    }

    bool EnemyLowHealth()
    {
        return enemy.GetComponent<EnemyStats>().health <= 25;
    }

    void SetBehaviour()
    {
        Dictionary<Behaviour, Vector2> behaviourPositions = new Dictionary<Behaviour, Vector2>();
        if(behaviourProbs.Count == 0)
        {
            Debug.LogError("[BehaviourGenerator] No behaviours defined");
            return;
        }

        float beginning = 0;

        for (int i = 0; i < behaviourProbs.Count; ++i)
        {
            float relativeEnding = behaviourProbs.Values.ElementAt(i);
            behaviourPositions.Add(behaviourProbs.Keys.ElementAt(i), new Vector2(beginning, beginning + relativeEnding));
            beginning += relativeEnding;
        }

        float behaviourRange = Random.Range(0f, behaviourPositions.Values.Last().y);
        for (int i = 0; i < behaviourPositions.Count; ++i)
        {
            var range = behaviourPositions.Values.ElementAt(i);
            if (!IsValueBetweenPair(behaviourRange, range))
            {
                continue;
            }

            var behaviourFromList = behaviourPositions.Keys.ElementAt(i);
            behaviour = behaviourFromList == Behaviour.KeepGuard ? GetRandomGuard() : behaviourFromList;
            return;     
        }
        Debug.LogError("Behaviour not found in range, range: " + behaviourPositions.Values.Last().y);

    }

    void ResetValues()
    {
        foreach(var key in behaviourProbs.Keys.Reverse())
        {
            behaviourProbs[key] = 0;
        }
    }

    Behaviour GetRandomGuard()
    {
        int value = Random.Range(0, 3);
        if(value == 0)
        {
            return Behaviour.KeepGuardStand;
        }
        else if(value == 1)
        {
            return Behaviour.KeepGuardRight;
        }

        return Behaviour.KeepGuardLeft;
    }

    bool IsValueBetweenPair(float value, Vector2 pair)
    {
        if((pair.x - pair.y) == 0)
        {
            return false;
        }
        return value >= pair.x && value < pair.y;
    }

    void SetDefaultProbabilitie()
    {
        SummonStats(4, 1.5f, 3.5f, 0);
    }

    void SetEnemyNotAwareOfPlayer()
    {
        behaviourProbs[Behaviour.Idle] = 1;
        behaviourProbs[Behaviour.WalkFree] = 1;
    }

    void SetWalkToCampfire()
    {
        behaviourProbs[Behaviour.WalkToCampfire] = 1;
        behaviourProbs[Behaviour.Idle] = 0.3f;
    }
    void SetIdle()
    {
        behaviourProbs[Behaviour.Idle] = 1f;
    }
    void SetAttackCloseToPlayerProb()
    {
        SummonStats(5.5f, 0.75f, 3.5f, 0);
    }

    void SetLowHealthProb()
    {
        SummonStats(1, 5, 3, 0);
    }

    void SetAttackCloseToPlayerAndLowHealtProb()
    {
        SummonStats(4.5f, 3.5f, 1, 0);
    }

    void SetPerformAttackLowHealthProb()
    {
        SummonStats(0, 1, 4, 5);
    }

    void SetPerformAttackProb()
    {
        SummonStats(0, 1, 4.5f, 5.5f);
    }

    void SummonStats(float attack, float runBack, float guard, float performAttack)
    {
        behaviourProbs[Behaviour.Attack] = attack * enemy.GetComponent<EnemyStats>().aggresiveness;
        behaviourProbs[Behaviour.RunBackwards] = runBack * enemy.GetComponent<EnemyStats>().fear;
        behaviourProbs[Behaviour.KeepGuard] = guard * enemy.GetComponent<EnemyStats>().fear;
        behaviourProbs[Behaviour.PerformAttack] = performAttack;
    }

}
