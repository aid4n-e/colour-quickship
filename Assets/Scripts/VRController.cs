using UnityEngine;
using Valve.VR;

public class VRController : MonoBehaviour
{
    public bool debug;

    public LayerMask groundLayer;

    public CapsuleCollider capsule;
    public Transform hands;
    public Rigidbody rig;
    public GameObject vrcamera;
    public GameObject vroffset;

    public float gravity;
    public float sensitivity = 0.1f;

    public float rotateSensitivity = 0.1f;


    public Vector2 testMove;
    public Vector2 testRot;
    Vector3 moveDirection;
    public SteamVR_Action_Vector2 moveValue = null;
    public SteamVR_Action_Vector2 rotateValue = null;

    public SteamVR_Action_Boolean grabValueR = null;
    public SteamVR_Action_Boolean grabValueL = null;

    private float fallingSpeed;
    private float xMov;
    private float yMov;
    private float xRot;

    void FixedUpdate()
    {
        getInput();
        JoystickMovement();
        JoystickRotation();

        if ( CheckIfGrounded() )
            fallingSpeed = 0;
        else if ( !CheckIfGrounded() )
            fallingSpeed += gravity * Time.fixedDeltaTime;

        rig.AddForce(new Vector3(0,fallingSpeed,0));
        rig.AddForce(moveDirection);
    }

    void getInput()
    {
        if (debug)
        {
            xMov = testMove.x;
            yMov = testMove.y;
            xRot = testRot.x;
        }

        else
        {
            print(moveValue.axis);
            xMov = moveValue.axis.x;
            yMov = moveValue.axis.y;
            xRot = rotateValue.axis.x;
        }
    }

    void JoystickMovement()
    {
        xMov *= sensitivity;
        yMov *= sensitivity;

        Quaternion headYaw = Quaternion.Euler(0, vrcamera.transform.eulerAngles.y, 0);
        moveDirection = headYaw * new Vector3(xMov, 0, yMov);
    }

    void JoystickRotation()
    {
        capsule.transform.RotateAround(vroffset.transform.position, Vector3.up, xRot * rotateSensitivity);
    }

    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(capsule.center);
        float rayLength = capsule.center.y + 0.001f;
        bool hasHit = Physics.SphereCast(rayStart, capsule.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}