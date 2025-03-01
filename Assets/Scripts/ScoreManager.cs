using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CoinPointCollection[] coins = FindObjectsByType<CoinPointCollection>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CoinPointCollection c in coins)
        {
            c.CoinCollection.AddListener(UpdateScore);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScore ()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }
}
