using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RadioToggleGroup : MonoBehaviour
{
    public Toggle mapToggle1;
    public Toggle mapToggle2;

    // Add references to your map images here
    public Image mapImage1;
    public Image mapImage2;

    public Toggle[] mapToggles; // Array of map toggles

    private void Start()
    {
        // Initialize the radio toggle group
        InitializeRadioToggleGroup(mapToggles);

        // Make sure one of the toggles is selected by default (map 1).
        mapToggle1.isOn = true;

        // Debug message to show that Map1 is the default selection
        Debug.Log("Default Map Selection: Map1");
    }

    public void TransitionToFlipCoinScene()
    {
        // Check which toggle is selected and load the corresponding scene.
        if (mapToggle1.isOn)
        {
            // Set the selected map
            SceneNavigator.selectedMap = "Map1";

            // Load the FlipCoin scene with map 1 data.
            SceneManager.LoadScene("FlipCoin");
        }
        else if (mapToggle2.isOn)
        {
            // Set the selected map
            SceneNavigator.selectedMap = "Map2";

            // Load the FlipCoin scene with map 2 data.
            SceneManager.LoadScene("FlipCoin");
        }
    }

    private void InitializeRadioToggleGroup(Toggle[] toggles)
    {
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(toggle); });
        }
    }

    private void ToggleValueChanged(Toggle changedToggle)
    {
        if (changedToggle.isOn)
        {
            foreach (var toggle in mapToggles)
            {
                if (toggle != changedToggle)
                {
                    toggle.isOn = false;
                }
            }

            // Debug message to show which map is chosen
            Debug.Log("Selected Map: " + (changedToggle == mapToggle1 ? "Map1" : "Map2"));
        }
    }
}
