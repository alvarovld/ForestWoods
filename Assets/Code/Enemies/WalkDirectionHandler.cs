using System.Collections;
using UnityEngine;

public class WalkDirectionHandler : MonoBehaviour
{
    EnemyBehaviourHandler handler;
    Vector3 direction;
    Vector3 initialPosition;

    private void OnEnable()
    {
        handler = GetComponent<EnemyBehaviourHandler>();
        initialPosition = transform.position;
        SetDirection();
        initialPosition = transform.position;
        StartCoroutine(DisableWhenNotWalking());
    }

    public Vector3 GetDirecion()
    {
        return direction;
    }

    bool EnemyOffPerimeter()
    {
        return (transform.position - initialPosition).magnitude >= GetComponent<EnemyStats>().walkRadius;
    }


    float GetRandomDirectionTime()
    {
        return Random.Range(7f, 12f);
    }

    IEnumerator TimeToSetNewDirection()
    {
        yield return new WaitForSeconds(GetRandomDirectionTime());
        SetDirection();
    }

    public void SetDirection()
    {
        StartCoroutine(TimeToSetNewDirection());
        if(EnemyOffPerimeter())
        {
            direction = initialPosition - transform.position;
        }
        else
        {
            direction = GetRandomDirection();
        }
    }

    Vector3 GetRandomDirection()
    {
        float angle = Random.Range(0f, 120f);
        return Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
    }

    IEnumerator DisableWhenNotWalking()
    {
        yield return new WaitUntil(() =>
        {
            return !handler.GetBehaviour().Equals(GameData.Enums.BehaviourEnum.WalkFree);
        });
        this.enabled = false;
    }
}
