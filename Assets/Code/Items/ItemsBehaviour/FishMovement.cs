using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float movementRadius;
    Vector3 initialPosition;
    public float speed;
    public float rotationSpeed;
    float posY;
    Vector3 direction;
    public float angleOffset;

    void Start()
    {
        initialPosition = transform.position;
        posY = initialPosition.y;
    }


    bool FishOffPerimeter()
    {
        return (transform.position - initialPosition).magnitude >= movementRadius;
    }


    float GetRandomDirectionTime()
    {
        return Random.Range(2f, 4f);
    }

    IEnumerator TimeToSetNewDirection()
    {
        yield return new WaitForSeconds(GetRandomDirectionTime());
        SetDirection();
    }

    public void SetDirection()
    {
        StartCoroutine(TimeToSetNewDirection());
        if (FishOffPerimeter())
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


    private void OnEnable()
    {
        SetDirection();
        initialPosition = transform.position;
    }

    void Rotate()
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + angleOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), Time.deltaTime * rotationSpeed);
    }


    public void Move()
    {
        direction = direction.normalized;
        transform.position += new Vector3(direction.x, 0, direction.z) * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }

    void Update()
    {
        Move();
        Rotate();
    }
}
