using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector3> OnDirection = new UnityEvent<Vector3>();
    public UnityEvent OnSpacePressed = new UnityEvent();
    public UnityEvent OnShiftPressed = new UnityEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnShiftPressed?.Invoke();
        }

        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        OnDirection?.Invoke(direction);
    }
}
