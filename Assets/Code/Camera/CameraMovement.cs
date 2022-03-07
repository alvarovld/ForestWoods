using UnityEditor;
using UnityEngine;
using Utils;

public class CameraMovement : MonoBehaviour
{

    [Header("General")]
    public float smoothSpeed;
    public Transform target;
    public float yRotationOffset;

    [Header("Offset")]

    Vector3 offset;
    float offsetMultiplier;

    [Header("Zoom")]
    public float maxOffset;
    public float minOffset;
    public float offsetSpeed;

    [Header("Circular rotation")]
    public float rotationSpeed;
    float maxDistanceToPlayer;

    [Header("Look at target")]
    public float lookSpeed;

    [Header("Reset Position")]
    public float horizontalOffset;
    public float verticalOffset;

    [Header("Fog")]
    public bool fog;

    private void Start()
    {
        transform.position = target.transform.position + offset;
        ResetPosition();
        maxDistanceToPlayer = (transform.position - target.position).magnitude;
    }

    private void OnEnable()
    {
        if (fog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.1176f, 0.1176f, 0.146f);
        }
    }

    void SetMouseScrollWheelOffset()
    {
        Vector3 prevDir = offset.normalized;
        offsetMultiplier = Input.GetAxis("Mouse ScrollWheel") * offsetSpeed;
        if(offsetMultiplier >= 60)
        {
            offsetMultiplier = 60;
        }
        offset = offset + offset.normalized * offsetMultiplier * (-1);

        if (offset.magnitude > maxOffset)
        {
            offset = maxOffset * prevDir;
        }
        else if (offset.magnitude < minOffset)
        {
            offset = minOffset * prevDir;
        }
        maxDistanceToPlayer = offset.magnitude;
    }



    void UpdateFinalPosition()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPos = Vector3.Slerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.position = desiredPosition;

    }

    void CircularMovement()
    {
        if(!Input.GetMouseButton(1))
        {
            return;
        }

        var vertical = Input.GetAxis("Mouse Y") * rotationSpeed;
        var horizontal = Input.GetAxis("Mouse X") * rotationSpeed;

        var initialPosOffset = transform.position - target.position;
        Plane offsetPlane = new Plane(initialPosOffset, target.position);
        Plane horizontalPlane = new Plane(new Vector3(0, target.position.y, 0), target.transform.position);
        var horizontalAxis = Vector3.Cross(offsetPlane.normal, horizontalPlane.normal).normalized;
        var yOffset = Quaternion.AngleAxis(-vertical, horizontalAxis) * initialPosOffset;

        var desiredPosOffset = Quaternion.Euler(0, horizontal, 0) * yOffset;
        offset = desiredPosOffset.normalized * maxDistanceToPlayer;
    }


    void ResetPosition()
    {
        transform.position = target.position - horizontalOffset * target.transform.forward + Vector3.up * verticalOffset;
        var prevXRot = transform.rotation.eulerAngles.x;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                                              target.transform.rotation.eulerAngles.y,
                                              transform.rotation.eulerAngles.z) ;
        offset = transform.position - target.transform.position;
    }

    bool IsHittingTheGround()
    {
        RaycastHit[] rays = Physics.RaycastAll(transform.position, Vector3.down, horizontalOffset + verticalOffset);
        foreach(var ray in rays)
        {
            if(!ray.collider.gameObject.tag.Equals(GameData.Tags.Terrain))
            {
                continue;
            }
            if((transform.position-ray.collider.transform.position).magnitude < 5)
            {
                return true;
            }
        }
        return false;
    }

    void LookAtTarget()
    {
        var dir = (target.position - transform.position).normalized + Vector3.up * yRotationOffset;
        var rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookSpeed);
    }

    private void LateUpdate()
    {
        SetMouseScrollWheelOffset();
        UpdateFinalPosition();
        CircularMovement();
        LookAtTarget();
    }
}
