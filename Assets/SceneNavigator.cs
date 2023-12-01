using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    private int currentIndex; // Keep track of the current scene index
    private string[] sceneNames = {
        "MainMenu",
        "CharacterSelect",
        "MapSelect",
        "FlipCoin",
        "GameProper",
        "WinBanner",
        "EvaluateBanner",
    };

    // Add a variable to store the selected map
    public static string selectedMap;

    private void Start()
    {
        // Initialize currentIndex to the build index of the current scene
        currentIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadNextScene()
    {
        if (currentIndex < sceneNames.Length - 1)
        {
            currentIndex++;
            Debug.Log("Loading next scene: " + sceneNames[currentIndex]);

            // If the next scene is GameProper, set the selected map before loading it
            if (sceneNames[currentIndex] == "GameProper")
            {
                SceneManager.LoadScene("MapSelect");
            }
            else
            {
                SceneManager.LoadScene(sceneNames[currentIndex]);
            }
        }
    }

    public void LoadPreviousScene()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            Debug.Log("Loading previous scene: " + sceneNames[currentIndex]);
            SceneManager.LoadScene(sceneNames[currentIndex]);
        }
    }

    public void LoadHomeScene()
    {
        // Replace "MainMenu" with the name of your home scene
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCharacterSelectScene()
    {
        // Replace "MainMenu" with the name of your home scene
        SceneManager.LoadScene("CharacterSelect");
    }

    public void LoadGuideScene()
    {
        SceneManager.LoadScene("Guide");
    }

    public void LoadSettingsScene()
    {
        SceneManager.LoadScene("Settings");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
