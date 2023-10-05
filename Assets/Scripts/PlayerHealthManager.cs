using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerHealthManager : MonoBehaviour
{
    public Slider player1HealthBar;
    public Slider player2HealthBar;

    private float player1Health = 100f;
    private float player2Health = 100f;

    MatchManager matchInstance;
    private bool gameOver = false; // Track if the game is over
    public GameObject player1WinBanner; // Reference to Player 1's win banner GameObject
    public GameObject player2WinBanner; // Reference to Player 2's win banner GameObject

    public AudioSource gameoverAudioSource; // Reference to the game over audio source
    public AudioClip winSound; // Sound effect to play when a win banner is displayed

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
        sceneSwitch();
    }

    private void sceneSwitch()
    {
        if (!gameOver && (matchInstance.player1.health <= 0 || matchInstance.player2.health <= 0))
        {
            // Set the game over flag to prevent multiple executions
            gameOver = true;

            // Play the game over sound
            gameoverAudioSource.Play();

            int winnerID;

            // Determine the winner
            if (matchInstance.player1.health <= 0)
            {
                // Player 2 wins
                player2WinBanner.SetActive(true);
                winnerID = 2;
            }
            else
            {
                // Player 1 wins
                player1WinBanner.SetActive(true);
                winnerID = 1;
            }

            // Save the winner's ID to PlayerPrefs
            PlayerPrefs.SetInt("WinnerID", winnerID);

            // Play the win sound effect
            if (winSound != null)
            {
                AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
            }

            // Transition to the "WinBanner" scene after a delay
            StartCoroutine(DelayedTransition());
        }
    }


    IEnumerator DelayedTransition()
    {
        // Wait for 5 seconds before transitioning to the "WinBanner" scene
        yield return new WaitForSeconds(5f);

        // Load the "WinBanner" scene
        SceneManager.LoadScene("WinBanner"); // Replace with your scene name
    }
}
