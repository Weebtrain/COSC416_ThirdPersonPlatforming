using UnityEngine;
using Unity.Cinemachine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpStrength = 1;
    [SerializeField] private float maxSpeed = 2;
    [SerializeField] private Transform camera;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        inputManager.OnDirection.AddListener(MovePlayer);
        inputManager.OnSpacePressed.AddListener(Jump);
    }

    // Update is called once per frame
    void Update()
    {
        faceCamera();
        maxVelocity();
    }

    public void MovePlayer (Vector3 direction)  // moves player relative to angle they are facing
    {
        rb.AddRelativeForce(direction * speed);
    }

    public void Jump ()
    {
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
        Vector3 floorVelocity = Vector3.zero;
        floorVelocity.x += rb.linearVelocity.x;
        floorVelocity.z += rb.linearVelocity.z;
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
}
