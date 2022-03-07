using UnityEngine;
using Utils;
using Behaviour = GameData.Enums.BehaviourEnum;
public class BehaviourExecutor : MonoBehaviour
{
    Transform player;
    EnemyBehaviourHandler behaviourHandler;
    WalkDirectionHandler walkDirection;

    float speed;
    float guardSpeed;

    private void Start()
    {
        player = GameObjectRefs.player;
        behaviourHandler = GetComponent<EnemyBehaviourHandler>();
        walkDirection = GetComponent<WalkDirectionHandler>();
        walkDirection.enabled = false;
        speed = GetComponent<EnemyStats>().speed;
        guardSpeed = GetComponent<EnemyStats>().guardSpeed;
    }

    void FixedUpdate()
    {
        ExecuteBehaviour(behaviourHandler.GetBehaviour());
    }

    void ExecuteBehaviour(Behaviour behaviour)
    {
        GetComponent<EnemyAnimatorHandler>().UpdateAnimator(behaviour);
        switch (behaviour)
        {
            case Behaviour.GetCrazy:
                break;
            case Behaviour.Attack:
                Attack();
                break;
            case Behaviour.KeepGuardStand:
                break;
            case Behaviour.RunBackwards:
                RunBackwards();
                break;
            case Behaviour.KeepGuardRight:
                KeepGuardRight();
                break;
            case Behaviour.KeepGuardLeft:
                KeepGuardLeft();
                break;
            case Behaviour.Idle:
                break;
            case Behaviour.PerformAttack:
                PerformAttack();
                break;
            case Behaviour.WalkFree:
                WalkFree();
                break;
            case Behaviour.WalkToCampfire:
                WalkToCampfire();
                break;
            default:
                Debug.Log("[BehaviourExecutor] Unknown behaviour: " + behaviour+ " Enemy: "+gameObject.name);
                break;
        }
    }

    public void WalkToCampfire()
    {
        var dir = (gameObject.GetComponent<EnemyStats>().closestCampfire - transform.position).normalized;
        gameObject.GetComponent<EnemyRotation>().RotateTowardsDirection(-dir);
        moveEnemy(dir, gameObject.GetComponent<EnemyStats>().walkSpeed);
    }
    void WalkFree()
    {
        walkDirection.enabled = true;
        moveEnemy(walkDirection.GetDirecion(), GetComponent<EnemyStats>().walkSpeed);
    }

    void PerformAttack()
    {
        //GetComponent<MeleeAttackHandler>().Attack();
    }

    void Attack()
    {
        RunTowardsPlayer();
    }

    void RunTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        Vector3 unitaryDir = direction.normalized;
        moveEnemy(unitaryDir, speed);
    }

    float DistanceToPlayer()
    {
        return (player.position - transform.position).magnitude;
    }
    public void moveEnemy(Vector3 dir, float speed)
    {
        dir = dir.normalized;
        const float minDistance = 0.5f;
        if(DistanceToPlayer() <= minDistance)
        {
            return;
        }
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;
    }



    void RunBackwards()
    {
        Vector3 direction = player.position - transform.position;
        Vector3 unitaryDir = direction.normalized;
        moveEnemy(unitaryDir * (-1), speed);
    }

    void KeepGuardRight()
    {
        Vector3 direction = transform.position - player.position;
        Vector2 plain = new Vector2(direction.x, direction.z);
        Vector2 perpendicular = Vector2.Perpendicular(plain);
        Vector3 perpendicularV3 = new Vector3(perpendicular.x, 0, perpendicular.y);
        moveEnemy(perpendicularV3.normalized, guardSpeed);
    }

    void KeepGuardLeft()
    {
        Vector3 direction = transform.position - player.position;
        Vector2 plain = new Vector2(direction.x, direction.z);
        Vector2 perpendicular = Vector2.Perpendicular(plain);
        Vector3 perpendicularV3 = new Vector3(perpendicular.x, 0, perpendicular.y);
        moveEnemy(perpendicularV3.normalized * -1, guardSpeed);
    }


}
