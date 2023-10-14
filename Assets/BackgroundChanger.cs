using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    public Image backgroundImage;

    
    public Sprite map1Sprite;
    public Sprite map2Sprite;

    private void Start()
    {
        if (MapManager.Instance.selectedMap == "Map1")
        {
            backgroundImage.sprite = map1Sprite;
        }
        else if (MapManager.Instance.selectedMap == "Map2")
        {
            backgroundImage.sprite = map2Sprite;
        }
        else
        {
            Debug.LogWarning("Unknown Map: " + MapManager.Instance.selectedMap); // Debug unknown maps
        }
    }
}