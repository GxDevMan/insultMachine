using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RadioToggleGroup : MonoBehaviour
{
    public Toggle mapToggle1;
    public Toggle mapToggle2;
    public Toggle[] mapToggles; // Array of map toggles

    private void Start()
    {
        // Initialize the radio toggle group
        InitializeRadioToggleGroup(mapToggles);

        // Load the last selected map from MapManager (default to Map1 if not found)
        MapManager.Instance.selectedMap = PlayerPrefs.GetString("SelectedMap", "Map1");

        // Set the toggle based on the loaded map
        if (MapManager.Instance.selectedMap == "Map1")
        {
            mapToggle1.isOn = true;
        }
        else if (MapManager.Instance.selectedMap == "Map2")
        {
            mapToggle2.isOn = true;
        }

        // Debug the selected map
        Debug.Log("Selected Map: " + MapManager.Instance.selectedMap);

    }

    public void TransitionToFlipCoinScene()
    {
        // Check which toggle is selected and save the selected map in PlayerPrefs
        if (mapToggle1.isOn)
        {
            MapManager.Instance.selectedMap = "Map1";
        }
        else if (mapToggle2.isOn)
        {
            MapManager.Instance.selectedMap = "Map2";
        }

        // Save the selected map in PlayerPrefs
        PlayerPrefs.SetString("SelectedMap", MapManager.Instance.selectedMap);
        PlayerPrefs.Save();

        // Load the FlipCoin scene
        SceneManager.LoadScene("FlipCoin");
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
            else
    {
        Debug.Log("Toggle turned off.");
    }
    }
}
