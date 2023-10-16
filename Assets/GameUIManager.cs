using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public Button soundButton;
    public Button restartButton;
    public Button homeButton;
    public GameProper gameProper; // Reference to the GameProper script

    public Button menuButton;
    public GameObject menuCanvasObject;
    public GameObject howToPlayCanvasObject;


    public Button continueButton;
    public Button backButton;
    public Button howToPlayButton;
    public Button quitButton;

    private bool soundEnabled = true;
    private bool isPaused = false;

    private float previousTimeScale; // Store the previous time scale for resuming
    MatchManager matchInstance;

    private void Start()
    {
        matchInstance = MatchManager.instance;
        // Disable the initial canvas GameObject
        if (menuCanvasObject != null)
        {
            menuCanvasObject.SetActive(false);
        }
        // Disable the initial canvas GameObject
        if (howToPlayCanvasObject != null)
        {
            howToPlayCanvasObject.SetActive(false);
        }

        // Add click listeners for the sound, restart, and pause buttons
        soundButton.onClick.AddListener(ToggleSound);
        restartButton.onClick.AddListener(RestartGame);
        homeButton.onClick.AddListener(GoToMainMenu);

        // Add a click listener for the menu button
        menuButton.onClick.AddListener(ShowCanvas);

        // Add click listeners for the menu buttons
        continueButton.onClick.AddListener(ContinueGame);
        backButton.onClick.AddListener(GoToMenuCanvas);
        howToPlayButton.onClick.AddListener(GoToHowToPlay);
        quitButton.onClick.AddListener(QuitGame);
    }


    private void ToggleSound()
    {
        Debug.Log("ToggleSound called");
        soundEnabled = !soundEnabled;

        // Implement logic to enable/disable sounds based on the 'soundEnabled' variable
        // For example, you can use AudioListener.volume to adjust the volume:
        AudioListener.volume = soundEnabled ? 1f : 0f;

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

        replayStartMatch(matchInstance.firstPlayer);

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

            // Stop all audio sources when paused
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in allAudioSources)
            {
                audioSource.Pause();
            }

            // Handle pausing of player input in the GameProper script (if needed)
            if (gameProper != null)
            {
                gameProper.isGameRunning = false; // Pause the game in GameProper
            }
        }
        else
        {
            Time.timeScale = previousTimeScale; // Restore the previous time scale
            Debug.Log("Game Resumed");

            // Resume all audio sources when resumed
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audioSource in allAudioSources)
            {
                audioSource.UnPause();
            }

            // Handle resuming of player input in the GameProper script (if needed)
            if (gameProper != null)
            {
                gameProper.isGameRunning = true; // Resume the game in GameProper
            }
        }
    }


    public void ShowCanvas()
    {
        if (menuCanvasObject != null)
        {
            menuCanvasObject.SetActive(true);
            TogglePause(); // Pause the game when showing the menu canvas
        }
    }

    public void HideCanvas()
    {
        if (menuCanvasObject != null)
        {
            menuCanvasObject.SetActive(false);
            TogglePause(); // Resume the game when hiding the menu canvas
        }
    }

    public void ShowHowToPlayCanvas()
    {
        if (howToPlayCanvasObject != null)
        {
            howToPlayCanvasObject.SetActive(true);
            TogglePause(); // Pause the game when showing the menu canvas
        }
    }

    public void HideHowToPlayCanvas()
    {
        if (howToPlayCanvasObject != null)
        {
            howToPlayCanvasObject.SetActive(false);
            TogglePause(); // Resume the game when hiding the menu canvas
        }
    }


    private void ContinueGame()
    {
        // Implement the continue functionality
        HideCanvas(); // Hide the menu canvas
    }

    private void GoToHowToPlay()
    {
        // Implement going back to the main menu
        //SceneManager.LoadScene("MainMenu"); // Replace with the name of your main menu scene
        TogglePause();
        ShowHowToPlayCanvas();
    }

    private void GoToMenuCanvas()
    {
        // Implement the continue functionality
        TogglePause();
        HideHowToPlayCanvas(); // Hide the menu canvas
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

    private void startMatch(bool firstPlayer)
    {
        playerObj player1 = new playerObj("Player 1", 100, 26, 30);
        playerObj player2 = new playerObj("Player 2", 100, 26, 30);
        matchInstance.newMatch(player1, player2);
        matchInstance.setFirstPlayer(firstPlayer);
    }

    private void replayStartMatch(bool firstPlayer)
    {
        Debug.Log("Restarting Match");
        matchInstance.setFirstPlayer(firstPlayer);
        matchInstance.restartMatch();
    }
}
