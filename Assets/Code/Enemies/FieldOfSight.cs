using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FieldOfSight
{
    public static bool IsTransformInFieldOfSight(Transform enemy, Transform other)
    {
        var forward = GetForward(enemy);
        Vector3 otherVector = other.position - enemy.transform.position;
        Vector2 plain = new Vector2(otherVector.x, -otherVector.z);
        Vector2 forwardPlain = new Vector2(forward.x, -forward.z);
        float angleFromOtherToForward = Vector2.Angle(forwardPlain, plain);
        var distanceToOther = (enemy.position - other.position).magnitude;
        if (angleFromOtherToForward > (enemy.GetComponent<EnemyStats>().angleOfSight / 2))
        {
            return false;
        }
        else if(distanceToOther <= enemy.gameObject.GetComponent<EnemyStats>().visionDistance)
        {
            return true;
        }
        else if(PlayerInfo.lightOn && distanceToOther <= enemy.gameObject.GetComponent<EnemyStats>().lightVisionDistance)
        {
            return true;
        }
        return false;
    }

    static Vector3 GetForward(Transform me)
    {
        float fixAngle = 0;
        Quaternion fixedRotation = Quaternion.Euler(0, fixAngle, 0) * me.rotation;
        return fixedRotation * new Vector3(0, 0, 1);
    }
}
