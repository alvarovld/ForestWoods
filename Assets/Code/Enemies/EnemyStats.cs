using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyStats : MonoBehaviour
{
    [Header("Raw stats")]
    public float health;
    public float fear;
    public float aggresiveness;
    public float strength;

    [Header("Raw movement speed stats")]
    public float speed;
    public float guardSpeed;
    public float rotationSpeed;

    [Header("WalkFree behaviour stats")]
    public float walkSpeed;
    public float walkRadius;

    [Header("Situation awareness stats")]
    [Tooltip("The less the value the more the enemy hears")]
    public float hearingThreshold;
    public float visionDistance;
    public float losePlayerDistance;
    public float lightVisionDistance;
    public int angleOfSight;

    [Header("Behaviour Handler stats")]
    public float performAttackDistance;
    public float stopAttackDistance;
    public float runBackwardsDistance;
    public float increaseAttackProbabilitiesDistance;
    public float stopAttackTime;

    [Header("Walk to campfire behaviour")]
    public float minDistanceToCampfire;
    public float maxDistanceToCampfire;

    [Header("Spawn")]
    public float distanceToDespawn;

    [HideInInspector]
    public Vector3 closestCampfire = Vector3.zero;

    void Start()
    {
        health = 100;   
    }


    public void ReduceHealth(float amount)
    {
        health -= amount;
    }


}
