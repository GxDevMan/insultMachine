using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProper : MonoBehaviour
{
    public float gameDuration = 180.0f; // Set the duration of your game in seconds (3 minutes)
    private float gameTimer;
    public Text timerText; // Reference to a UI Text component to display the game timer

    private float turnDuration = 30.0f; // Time per turn in seconds
    private float currentPlayerTurnTimer;
    public MessagingScript messagingScript; // Reference to your MessagingScript

    private bool gameOver = false;
    private bool isPlayer1Turn = true; // Flag to track whose turn it is

    public Text gameTimerText;

    // Flag to track whether the current player's input field is disabled
    private bool currentPlayerInputDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        gameTimer = gameDuration; // Initialize the game timer with the game duration
        currentPlayerTurnTimer = turnDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return; // Don't update timers if the game is over
        }

        // Update the game timer by subtracting deltaTime each frame
        gameTimer -= Time.deltaTime;

        // Calculate minutes and seconds for the game timer
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);

        // Update the UI text to display the remaining game time in the "M:SS" format
        if (timerText != null)
        {
            timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogError("timerText is not assigned.");
        }

        // Check if the game timer has reached 0, indicating the end of the game
        if (gameTimer <= 0)
        {
            EndGame();
        }

        // Update the current player's turn timer
        currentPlayerTurnTimer -= Time.deltaTime;

        // Check if the current player's turn timer has expired
        if (currentPlayerTurnTimer <= 0)
        {
            Debug.Log("Player " + (isPlayer1Turn ? "1" : "2") + "'s turn timer has expired.");

            // Disable the current player's input field using the reference
            if (isPlayer1Turn)
            {
                messagingScript.DisablePlayer1InputField();
            }
            else
            {
                messagingScript.DisablePlayer2InputField();
            }

            // Switch to the other player's turn
            SwitchTurns();
        }
    }

    public void SwitchTurns()
    {
        // Reset the turn timer for the current player
        currentPlayerTurnTimer = turnDuration;

        // Switch the turn to the other player
        isPlayer1Turn = !isPlayer1Turn;

        // Enable the current player's input field using the reference
        if (isPlayer1Turn)
        {
            messagingScript.EnablePlayer1InputField();
        }
        else
        {
            messagingScript.EnablePlayer2InputField();
        }
    }


    public void ResetGameTimer()
    {
        // Check if gameTimerText is assigned
        if (gameTimerText == null)
        {
            Debug.LogError("gameTimerText is not assigned.");
            return;
        }

        // Reset the game timer
        gameTimer = gameDuration;

        // Update the UI text for the game timer
        UpdateTimerText();
    }


    void UpdateTimerText()
    {
        // Debug.Log to confirm that the function is called
        Debug.Log("Updating Timer Text");

        // Calculate minutes and seconds for the game timer
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);

        // Update the UI text to display the remaining game time in the "M:SS" format
        if (gameTimerText != null)
        {
            gameTimerText.text = string.Format("{0}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogError("gameTimerText is not assigned.");
        }
    }

    void EndGame()
    {
        // Game over logic here
        // You can trigger game over, restart the level, or any other action you want
        Debug.Log("Game over!");
        // For example, you can reload the current scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Set the game over flag to prevent further updates
        gameOver = true;
    }
}