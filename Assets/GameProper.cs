using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameProper : MonoBehaviour
{
    public float gameDuration = 180.0f;
    private float gameTimer;
    public Text timerText;

    private float turnDuration = 30.0f;
    public float player1TurnDuration = 20.0f;
    public float player2TurnDuration = 20.0f;
    public Text player1TimerText;
    public Text player2TimerText;

    private float currentPlayerTurnTimer;
    public MessagingScript messagingScript;

    private bool gameOver = false;
    private bool isPlayer1Turn = true;

    public Text gameTimerText;

    private bool currentPlayerInputDisabled = false;

    // Add a variable to track whether it's Player 1's first turn
    private bool player1FirstTurn = true;

    // Add a variable to control the countdown
    private float countdownDuration = 5.0f; // 5-second countdown
    private bool isCountingDown = false;

    public Text countdownTimerText;

    public GameObject countdownPanel; // Reference to the Countdown Panel GameObject
    public GameObject gameOverPanel; // Drag and drop the Game Over Panel GameObject into this field in the Inspector

    public bool isGameRunning = false; // Flag to track if the game is running

    public CoinFlip coinFlip; // Reference to the CoinFlip script

void Start()
{
    gameTimer = gameDuration;
    messagingScript.gameProper = this;

    // Start the countdown when the game starts
    StartCountdown();

    // Read the result of the coin flip from PlayerPrefs
    int coinResult = PlayerPrefs.GetInt("CoinResult", 0);

    // Check the result and enable/disable text fields accordingly
    if (coinResult == 1) // Heads
    {
        messagingScript.EnablePlayer1InputField();
        messagingScript.DisablePlayer2InputField();

        // Set the initial timer text and timer for Player 1
        player1TimerText.text = player1TurnDuration.ToString();
        currentPlayerTurnTimer = player1TurnDuration;
        isPlayer1Turn = true;
    }
    else if (coinResult == 2) // Tails
    {
        messagingScript.EnablePlayer2InputField();
        messagingScript.DisablePlayer1InputField();

        // Set the initial timer text and timer for Player 2
        player2TimerText.text = player2TurnDuration.ToString();
        currentPlayerTurnTimer = player2TurnDuration;
        isPlayer1Turn = false;
    }
}


    void Update()
    {
        if (gameOver)
        {
            return;
        }

        // Check if we are in the countdown phase
        if (isCountingDown)
        {
            countdownDuration -= Time.deltaTime;

            // Display the countdown on the countdownTimerText
            int countdownSeconds = Mathf.CeilToInt(countdownDuration);
            countdownTimerText.text = "Game starts in: " + countdownSeconds.ToString();

            if (countdownDuration <= 0)
            {
                // Countdown finished, start the game
                isCountingDown = false;
                countdownTimerText.text = ""; // Clear the countdown text

                // Disable the countdown panel
                countdownPanel.SetActive(false);

                // Start the game
                isGameRunning = true;
            }
        }
        else if (isGameRunning) // Only update timers and gameplay if the game is running
        {
            // Game is running, update timers and gameplay

            gameTimer -= Time.deltaTime;

            if (gameTimer <= 0)
            {
                gameTimer = 0; // Ensure the timer doesn't go below zero
                EndGame();
            }

            int minutes = Mathf.FloorToInt(gameTimer / 60);
            int seconds = Mathf.FloorToInt(gameTimer % 60);

            if (timerText != null)
            {
                timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
            }
            else
            {
                Debug.LogError("timerText is not assigned.");
            }

            currentPlayerTurnTimer -= Time.deltaTime;
            UpdateTimerText();

            if (currentPlayerTurnTimer <= 0)
            {
                Debug.Log("Player " + (isPlayer1Turn ? "1" : "2") + "'s turn timer has expired.");

                if (isPlayer1Turn)
                {
                    messagingScript.DisablePlayer1InputField();
                    messagingScript.SendMessageFromInputField(messagingScript.player1InputField);
                }
                else
                {
                    messagingScript.DisablePlayer2InputField();
                    messagingScript.SendMessageFromInputField(messagingScript.player2InputField);
                }

                SwitchTurns();
            }

        }
    }

    // Function to start the countdown
    private void StartCountdown()
    {
        isCountingDown = true;
        int countdownSeconds = Mathf.CeilToInt(countdownDuration);
        countdownTimerText.text = "Game starts in: " + countdownSeconds.ToString();

        // Enable the countdown panel
        countdownPanel.SetActive(true);
    }

    public void SwitchTurns()
    {
        currentPlayerTurnTimer = isPlayer1Turn ? player2TurnDuration : player1TurnDuration;
        isPlayer1Turn = !isPlayer1Turn;

        if (isPlayer1Turn)
        {
            messagingScript.EnablePlayer1InputField();
            messagingScript.DisablePlayer2InputField();
            player1TimerText.text = currentPlayerTurnTimer.ToString();
        }
        else
        {
            messagingScript.EnablePlayer2InputField();
            messagingScript.DisablePlayer1InputField();
            player2TimerText.text = currentPlayerTurnTimer.ToString();
        }

        // Check if it's Player 1's first turn, and if yes, reset their timer
        if (isPlayer1Turn && player1FirstTurn)
        {
            ResetPlayer1Timer();
            player1FirstTurn = false;
        }
    }



    public void ResetGameTimer()
    {
        if (gameTimerText == null)
        {
            Debug.LogError("gameTimerText is not assigned.");
            return;
        }

        gameTimer = gameDuration;
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        Debug.Log("Updating Timer Text");
        int seconds = Mathf.CeilToInt(currentPlayerTurnTimer);

        if (isPlayer1Turn)
        {
            player1TimerText.text = seconds.ToString();
        }
        else
        {
            player2TimerText.text = seconds.ToString();
        }
    }

    void EndGame()
    {
        Debug.Log("Game over!");
        gameOver = true;

        // Display the Game Over Panel
        gameOverPanel.SetActive(true);

        // Trigger the scene transition with a delay
        TransitionToWinBanner();
    }

    void TransitionToWinBanner()
    {
        StartCoroutine(DelayedTransition());
    }

    IEnumerator DelayedTransition()
    {
        // Wait for 5 seconds before transitioning to the "WinBanner" scene
        yield return new WaitForSeconds(5f);

        // Load the "WinBanner" scene
        SceneManager.LoadScene("WinBanner"); // Replace with your scene name
    }

    public void ResetPlayer1Timer()
    {
        player1TurnDuration = 20.0f;
        player1TimerText.text = "20"; // Update the UI text to show 20 seconds
        currentPlayerTurnTimer = player1TurnDuration; // Reset the current turn timer
        Debug.Log("Player 1 Timer Reset to 20");
    }

    public void ResetPlayer2Timer()
    {
        player2TurnDuration = 20.0f;
        player2TimerText.text = "20"; // Update the UI text to show 20 seconds
        currentPlayerTurnTimer = player2TurnDuration; // Reset the current turn timer
        Debug.Log("Player 2 Timer Reset to 20");
    }



}
