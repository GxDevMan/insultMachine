using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public Button soundButton;
    public Button restartButton;
    public Button pauseButton;
    public GameProper gameProper; // Reference to the GameProper script

    public Button menuButton;
    public Canvas initialCanvas;

    public Button continueButton;
    public Button mainMenuButton;
    public Button quitButton;

    private bool soundEnabled = true;
    private bool isPaused = false;

    private float previousTimeScale; // Store the previous time scale for resuming

    private void Start()
    {
        // Disable the initial canvas
        if (initialCanvas != null)
        {
            initialCanvas.enabled = false;
        }

        // Add click listeners for the sound, restart, and pause buttons
        soundButton.onClick.AddListener(ToggleSound);
        restartButton.onClick.AddListener(RestartGame);
        pauseButton.onClick.AddListener(TogglePause);

        // Add a click listener for the menu button
        menuButton.onClick.AddListener(ShowCanvas);

        // Add click listeners for the menu buttons
        continueButton.onClick.AddListener(ContinueGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void ToggleSound()
    {
        soundEnabled = !soundEnabled;

        // Implement logic to enable/disable sounds based on the 'soundEnabled' variable
        // For example, you can use AudioListener.pause to mute/unmute audio:
        AudioListener.pause = !soundEnabled;

        // Log the sound state
        if (soundEnabled)
        {
            Debug.Log("Sound: On");
        }
        else
        {
            Debug.Log("Sound: Off");
        }
    }

    private void RestartGame()
    {
        Debug.Log("Restart button clicked"); // Add this line

        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Log the restart action
        Debug.Log("Game Restarted");

        // Reset the game timer in the GameProper script
        if (gameProper != null)
        {
            gameProper.ResetGameTimer();
        }
        else
        {
            Debug.LogError("GameProper script reference is missing.");
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            previousTimeScale = Time.timeScale; // Store the previous time scale
            Time.timeScale = 0f; // Set time scale to 0 to pause the game
            Debug.Log("Game Paused");
        }
        else
        {
            Time.timeScale = previousTimeScale; // Restore the previous time scale
            Debug.Log("Game Resumed");
        }
    }

    public void ShowCanvas()
    {
        if (initialCanvas != null)
        {
            initialCanvas.enabled = true;
            TogglePause(); // Pause the game when showing the menu canvas
        }
    }

    public void HideCanvas()
    {
        if (initialCanvas != null)
        {
            initialCanvas.enabled = false;
            TogglePause(); // Resume the game when hiding the menu canvas
        }
    }

    private void ContinueGame()
    {
        // Implement the continue functionality
        HideCanvas(); // Hide the menu canvas
    }

    private void GoToMainMenu()
    {
        // Implement going back to the main menu
        SceneManager.LoadScene("MainMenu"); // Replace with the name of your main menu scene
    }

    private void QuitGame()
    {
        // Implement quitting the application (only works in standalone builds)
        Application.Quit();
    }
}
