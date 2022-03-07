using UnityEngine;
using Utils;

public static class PlayerSituationAwareness
{
    public static bool IsEnemyAwayeOfPlayer(Transform enemy)
    {
        var hearingThreshold = enemy.GetComponent<EnemyStats>().hearingThreshold;
        var noise = FootFallNoiseFromPlayer.Get(enemy);

        //UnityEngine.Debug.Log("Hearing threshold: " + hearingThreshold + " noise: " + noise);

        if (noise > hearingThreshold)
        {
            return true;
        }

        if (FieldOfSight.IsTransformInFieldOfSight(enemy, GameObjectRefs.player.transform))
        {
            return true;
        }
        return false;
    }
}
