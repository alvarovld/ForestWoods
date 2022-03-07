using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EnemyRotation : MonoBehaviour
{
    public float angleFix;
    Transform other;

    void RotateTowardsPlayer()
    {
        other = GameObjectRefs.player;
        Vector3 direction = transform.position - other.position;
        RotateTowardsDirection(direction);
    }


    public void RotateTowardsDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + angleFix;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * GetComponent<EnemyStats>().rotationSpeed);
    }


    private void Update()
    {
        if(PlayerSituationAwareness.IsEnemyAwayeOfPlayer(transform))
        {
            RotateTowardsPlayer();
        }
        else if(GetComponent<EnemyBehaviourHandler>().GetBehaviour().Equals(GameData.Enums.BehaviourEnum.WalkFree))
        {
            RotateTowardsDirection(-1* GetComponent<WalkDirectionHandler>().GetDirecion());
        }
    }


}
