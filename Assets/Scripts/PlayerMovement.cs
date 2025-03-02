using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpStrength = 1;
    [SerializeField] private float maxSpeed = 2;
    [SerializeField] private Transform camera;
    [SerializeField] private float jumpCheckDistance = 0.1f;

    private int jumpCounter = 0; //Counter is set to 0 when touching ground. Need extra jumps to jump more than touching the ground.
    [SerializeField] private int jumps = 2;
    private bool preJump = false;
    private float preJumpTime;
    [SerializeField] private float preJumpTimeLimit = 0.1f;

    private float resetTimer = 0;
    [SerializeField] private float resetTime = 0.1f;

    private static int groundLayer = 6;
    private static int groundMask = 1 << groundLayer;

    [SerializeField] private float dashSpeed = 1;
    [SerializeField] private float dashTime = 1;
    private bool isDashing = false;
    private bool hasDashed = false;
    private float dampingReturn;

    private BoxCollider bCollider;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bCollider = GetComponent<BoxCollider> ();
        rb = GetComponent<Rigidbody>();

        dampingReturn = rb.linearDamping;

        inputManager.OnDirection.AddListener(MovePlayer);
        inputManager.OnSpacePressed.AddListener(Jump);
        inputManager.OnShiftPressed.AddListener(Dash);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            faceCamera();
            maxVelocity();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground")) {
            if (preJump && Time.time - preJumpTime < preJumpTimeLimit)
            {
                Debug.Log("Pre");
                preJump = false;
                jumpCounter = 1;
                JumpActivate();
                hasDashed = false;
            } else if (Time.time - resetTimer > resetTime)
            {
                Debug.Log("Reset");
                jumpCounter = 0;
                hasDashed = false;
            }
        }
    }

    public void MovePlayer (Vector3 direction)  // moves player relative to angle they are facing
    {
        rb.AddRelativeForce(direction * speed);
    }

    public void Jump ()
    {
        if (!isDashing)
        {
            Vector3 boxCast = new Vector3(bCollider.size.x * transform.localScale.x, 0, bCollider.size.z * transform.localScale.z);
            if (Physics.BoxCast(transform.position, boxCast, Vector3.down, Quaternion.identity, jumpCheckDistance, groundMask))
            {
                if (jumpCounter > 0)
                {
                    preJump = true;
                    preJumpTime = Time.time;
                }
                else
                {
                    Debug.Log("Jump");
                    jumpCounter = 1;
                    JumpActivate();
                }
            }
            else if (jumpCounter < jumps)
            {
                JumpActivate();
                if (jumpCounter == 0) //discounts first jump since not touching ground, counts as double jump but set up to allow for more jumps
                {
                    jumpCounter = 2;
                }
                else
                {
                    jumpCounter++;
                }
            }
        }
    }
    private void JumpActivate ()
    {
        resetTimer = Time.time;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpStrength);
    }

    private void faceCamera ()  //Faces the player cube to the camera direction along x and z axis
    {
        Vector3 faceDirection = transform.position;
        faceDirection.x += camera.forward.x;
        faceDirection.z += camera.forward.z;
        transform.LookAt(faceDirection);
    }
    private void maxVelocity () //Forces velocity along x and z axis to combine to less than maxSpeed. Normal vector maintains this condition over combined x and z movement.
    {
        Vector3 floorVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 floorNormal = Vector3.Normalize(floorVelocity);
        floorVelocity.x *= floorNormal.x;
        floorVelocity.z *= floorNormal.z;
        if (floorVelocity.x + floorVelocity.z > maxSpeed)
        {
            floorNormal *= maxSpeed;
            floorNormal.y = rb.linearVelocity.y;
            rb.linearVelocity = floorNormal;
        }
    }

    public void Dash ()
    {
        if (!isDashing && !hasDashed)
        {
            isDashing = true;
            hasDashed = true;
            StartCoroutine(HandleDashAction());
        }
    }

    IEnumerator HandleDashAction()
    {
        rb.useGravity = false;
        rb.linearDamping = 0;
        rb.AddRelativeForce(Vector3.forward * dashSpeed);
        yield return new WaitForSeconds(dashTime);
        rb.linearDamping = dampingReturn;
        rb.useGravity = true;
        isDashing = false;
    }
}
