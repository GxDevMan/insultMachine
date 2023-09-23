using UnityEngine;
using UnityEngine.UI;

public class GameProperAvatars : MonoBehaviour
{
    public Image player1CharacterImage;
    public Image player2CharacterImage;
    public Text player1CharacterNameText;
    public Text player2CharacterNameText;

    private void Start()
    {
        // Retrieve the selected character indices from PlayerPrefs
        int player1CharacterIndex = PlayerPrefs.GetInt("Player1CharacterIndex", 0);
        int player2CharacterIndex = PlayerPrefs.GetInt("Player2CharacterIndex", 1);

        // Get the corresponding character sprites from CharacterManager
        Sprite player1CharacterSprite = CharacterManager.instance.characters[player1CharacterIndex];
        Sprite player2CharacterSprite = CharacterManager.instance.characters[player2CharacterIndex];

        // Update the character images in the UI
        player1CharacterImage.sprite = player1CharacterSprite;
        player2CharacterImage.sprite = player2CharacterSprite;

        // Define the character names based on the selected characters
        string player1CharacterName = GetCharacterName(player1CharacterIndex);
        string player2CharacterName = GetCharacterName(player2CharacterIndex);

        // Update the character name labels in the UI
        player1CharacterNameText.text = player1CharacterName;
        player2CharacterNameText.text = player2CharacterName;
    }

    private string GetCharacterName(int characterIndex)
    {
        // Map character indices to names (you may add more mappings as needed)
        switch (characterIndex)
        {
            case 0: // Totoy
                return "Totoy";
            case 1: // Karen
                return "Karen";
            default:
                return "Unknown"; // Default or unknown character
        }
    }

    // Other game-related code...
}
