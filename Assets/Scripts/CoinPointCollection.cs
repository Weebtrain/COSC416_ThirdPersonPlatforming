using UnityEngine;
using UnityEngine.Events;

public class CoinPointCollection : MonoBehaviour
{
    public UnityEvent CoinCollection = new UnityEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider triggeredObject)
    {
        if (triggeredObject.CompareTag("Player")) {
            CoinCollection?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
