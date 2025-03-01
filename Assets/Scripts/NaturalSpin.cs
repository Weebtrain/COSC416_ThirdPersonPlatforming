using UnityEngine;

public class NaturalSpin : MonoBehaviour
{
    [SerializeField] private Vector3 spinSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Mathf.PI * spinSpeed * Time.deltaTime);
    }
}
