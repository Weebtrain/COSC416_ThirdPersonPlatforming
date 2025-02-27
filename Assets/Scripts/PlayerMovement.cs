using UnityEngine;
using Unity.Cinemachine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed = 1;
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
        Vector3 faceDirection = transform.position;
        faceDirection.x += camera.forward.x;
        faceDirection.z += camera.forward.z;
        transform.LookAt(faceDirection);
    }

    public void MovePlayer (Vector3 direction)
    {
        rb.AddRelativeForce(direction * speed);
    }

    public void Jump ()
    {

    }
}
