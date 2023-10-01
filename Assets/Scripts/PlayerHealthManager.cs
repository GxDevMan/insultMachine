using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public Slider player1HealthBar;
    public Slider player2HealthBar;

    private float player1Health = 100f;
    private float player2Health = 100f;

    MatchManager matchInstance;

    void Start()
    {
        player1HealthBar.maxValue = player1Health;
        player2HealthBar.maxValue = player2Health;

        player1HealthBar.value = player1Health;
        player2HealthBar.value = player2Health;

        matchInstance = MatchManager.instance;
    }

    void Update()
    {
        player1HealthBar.value = matchInstance.player1.health;
        player2HealthBar.value = matchInstance.player2.health;
    }  
}


