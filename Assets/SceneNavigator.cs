using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    private int currentIndex = 0; // Keep track of the current scene index
    private string[] sceneNames = {
        "CharacterSelect",
        "MapSelect",
        "FlipCoin",
        "GameProper",
        "WinBanner",
        "EvaluateBanner",
    };

    public void LoadNextScene()
    {
        if (currentIndex < sceneNames.Length - 1)
        {
            currentIndex++;
            SceneManager.LoadScene(sceneNames[currentIndex]);
        }
    }

    public void LoadPreviousScene()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            SceneManager.LoadScene(sceneNames[currentIndex]);
        }
    }

    public void LoadHomeScene()
    {
        // Replace "Home" with the name of your home scene
        SceneManager.LoadScene("MainMenu");
    }

}
