using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public Slider player1HealthBar;
    public Slider player2HealthBar;

    private float player1Health = 100f;
    private float player2Health = 100f;

    void Start()
    {
        player1HealthBar.maxValue = player1Health;
        player2HealthBar.maxValue = player2Health;

        player1HealthBar.value = player1Health;
        player2HealthBar.value = player2Health;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DamagePlayer(2, 20f); // Damage Player 2
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DamagePlayer(1, 20f); // Damage Player 1
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HealPlayer(1, 20f); // Heal Player 1
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HealPlayer(2, 20f); // Heal Player 2
        }
    }

    void DamagePlayer(int playerNumber, float damageAmount)
    {
        if (playerNumber == 1)
        {
            player1Health -= damageAmount;
            player1Health = Mathf.Max(0f, player1Health);
            player1HealthBar.value = player1Health;
        }
        else if (playerNumber == 2)
        {
            player2Health -= damageAmount;
            player2Health = Mathf.Max(0f, player2Health);
            player2HealthBar.value = player2Health;
        }
    }

    void HealPlayer(int playerNumber, float healAmount)
    {
        if (playerNumber == 1)
        {
            player1Health += healAmount;
            player1Health = Mathf.Min(100f, player1Health);
            player1HealthBar.value = player1Health;
        }
        else if (playerNumber == 2)
        {
            player2Health += healAmount;
            player2Health = Mathf.Min(100f, player2Health);
            player2HealthBar.value = player2Health;
        }
    }
}


