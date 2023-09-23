using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    public Image backgroundImage;

    // Public variables to hold the map sprites
    public Sprite map1Sprite;
    public Sprite map2Sprite;

    private void Start()
    {
        // Check the selected map and change the background accordingly
        Debug.Log("Selected Map in BackgroundChanger: " + MapManager.Instance.selectedMap); // Debug the selected map
        if (MapManager.Instance.selectedMap == "Map1")
        {
            // Set the background image using the map1Sprite variable
            backgroundImage.sprite = map1Sprite;
            Debug.Log("Background changed to Map 1");
        }
        else if (MapManager.Instance.selectedMap == "Map2")
        {
            // Set the background image using the map2Sprite variable
            backgroundImage.sprite = map2Sprite;
            Debug.Log("Background changed to Map 2");
        }
        else
        {
            Debug.LogWarning("Unknown Map: " + MapManager.Instance.selectedMap); // Debug unknown maps
        }
        // Add more conditions for additional maps as needed
    }
}