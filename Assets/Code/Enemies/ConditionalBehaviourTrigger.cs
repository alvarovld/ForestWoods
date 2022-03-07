using UnityEngine;
using System.Collections;
using Behaviour = GameData.Enums.BehaviourEnum;
public class ConditionalBehaviourTrigger : MonoBehaviour
{
    EnemyBehaviourHandler handler;
    void Start()
    {
        handler = GetComponent<EnemyBehaviourHandler>();
    }

    void Update()
    {
        if ((handler.behaviour == Behaviour.WalkFree || handler.behaviour == Behaviour.Idle) &&
            PlayerSituationAwareness.IsEnemyAwayeOfPlayer(transform))
        {
            handler.SetRandomBehaviour();
        }
    }
}
