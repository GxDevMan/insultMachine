using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Image player1CharacterImage;
    public Image player2CharacterImage;
    private int player1CurrentCharacterIndex = 0;
    private int player2CurrentCharacterIndex = 1;

    private void Start()
    {
        // Load the selected characters from PlayerPrefs, or set them to default (0 and 1) if not found
        int player1Index = PlayerPrefs.GetInt("Player1CharacterIndex", 0);
        int player2Index = PlayerPrefs.GetInt("Player2CharacterIndex", 1);

        // Set the initial character images based on the saved or default indices
        UpdateCharacterImage(player1CharacterImage, player1Index);
        UpdateCharacterImage(player2CharacterImage, player2Index);

        // Set the selected characters in the CharacterManager
        CharacterManager.instance.SetSelectedCharacters(player1Index, player2Index);

        // Display debug information for the initial character selections
        Debug.Log("Player 1 selected character index: " + player1Index);
        Debug.Log("Player 2 selected character index: " + player2Index);

        // Load the selected character sprites based on their indices
        Sprite player1Sprite = CharacterManager.instance.GetPlayer1CharacterSprite();
        Sprite player2Sprite = CharacterManager.instance.GetPlayer2CharacterSprite();

        // Update the character images in the scene
        player1CharacterImage.sprite = player1Sprite;
        player2CharacterImage.sprite = player2Sprite;
    }


    // Called when the Right button is clicked for Player 1
    public void Player1NextCharacter()
    {
        player1CurrentCharacterIndex = (player1CurrentCharacterIndex + 1) % CharacterManager.instance.characters.Count;
        UpdateCharacterImage(player1CharacterImage, player1CurrentCharacterIndex);

        // Map the character name to the desired label
        string selectedCharacterName = CharacterManager.instance.characters[player1CurrentCharacterIndex].name;
        string selectedCharacterLabel = GetCharacterLabel(selectedCharacterName);

        Debug.Log("Setting Player 1 character name to: " + selectedCharacterLabel);

        // Save the selected character index for Player 1
        PlayerPrefs.SetInt("Player1CharacterIndex", player1CurrentCharacterIndex);
        PlayerPrefs.SetString("character_TotoyIdle", selectedCharacterLabel);

        // Display debug information for the selected character for Player 1
        Debug.Log("Player 1 selected character: " + CharacterManager.instance.characters[player1CurrentCharacterIndex].name);
    }

    // Called when the Left button is clicked for Player 1
    public void Player1PreviousCharacter()
    {
        player1CurrentCharacterIndex = (player1CurrentCharacterIndex - 1 + CharacterManager.instance.characters.Count) % CharacterManager.instance.characters.Count;
        UpdateCharacterImage(player1CharacterImage, player1CurrentCharacterIndex);

        // Map the character name to the desired label
        string selectedCharacterName = CharacterManager.instance.characters[player1CurrentCharacterIndex].name;
        string selectedCharacterLabel = GetCharacterLabel(selectedCharacterName);

        // Save the selected character index for Player 1
        PlayerPrefs.SetInt("Player1CharacterIndex", player1CurrentCharacterIndex);
        PlayerPrefs.SetString("character_TotoyIdle", selectedCharacterLabel);

        // Display debug information for the selected character for Player 1
        Debug.Log("Player 1 selected character: " + CharacterManager.instance.characters[player1CurrentCharacterIndex].name);
    }

    // Called when the Right button is clicked for Player 2
    public void Player2NextCharacter()
    {
        player2CurrentCharacterIndex = (player2CurrentCharacterIndex + 1) % CharacterManager.instance.characters.Count;
        UpdateCharacterImage(player2CharacterImage, player2CurrentCharacterIndex);

        // Map the character name to the desired label
        string selectedCharacterName = CharacterManager.instance.characters[player2CurrentCharacterIndex].name;
        string selectedCharacterLabel = GetCharacterLabel(selectedCharacterName);

        // Save the selected character index for Player 2
        PlayerPrefs.SetInt("Player2CharacterIndex", player2CurrentCharacterIndex);
        PlayerPrefs.SetString("character_AsianMomIdle", selectedCharacterLabel);

        // Display debug information for the selected character for Player 2
        Debug.Log("Player 2 selected character: " + CharacterManager.instance.characters[player2CurrentCharacterIndex].name);
    }

    // Called when the Left button is clicked for Player 2
    public void Player2PreviousCharacter()
    {
        player2CurrentCharacterIndex = (player2CurrentCharacterIndex - 1 + CharacterManager.instance.characters.Count) % CharacterManager.instance.characters.Count;
        UpdateCharacterImage(player2CharacterImage, player2CurrentCharacterIndex);

        // Map the character name to the desired label
        string selectedCharacterName = CharacterManager.instance.characters[player2CurrentCharacterIndex].name;
        string selectedCharacterLabel = GetCharacterLabel(selectedCharacterName);

        // Save the selected character index for Player 2
        PlayerPrefs.SetInt("Player2CharacterIndex", player2CurrentCharacterIndex);
        PlayerPrefs.SetString("character_AsianMomIdle", selectedCharacterLabel);

        // Display debug information for the selected character for Player 2
        Debug.Log("Player 2 selected character: " + CharacterManager.instance.characters[player2CurrentCharacterIndex].name);
    }

    private string GetCharacterLabel(string characterName)
    {
        // Map character names to labels
        switch (characterName)
        {
            case "character_TotoyIdle":
                return "Totoy";
            case "character_AsianMomIdle":
                return "Karen";
            default:
                return characterName; // If not found, use the original name
        }
    }

    private void UpdateCharacterImage(Image image, int index)
    {
        image.sprite = CharacterManager.instance.characters[index];
    }

    // Function to reset character selections to default
    public void ResetCharacterSelection()
    {
        PlayerPrefs.DeleteKey("Player1CharacterIndex");
        PlayerPrefs.DeleteKey("Player2CharacterIndex");

        // Set character selections to default (0)
        player1CurrentCharacterIndex = 0;
        player2CurrentCharacterIndex = 1;

        // Update character images to default
        UpdateCharacterImage(player1CharacterImage, player1CurrentCharacterIndex);
        UpdateCharacterImage(player2CharacterImage, player2CurrentCharacterIndex);

        // Display debug information for the default character selections
        Debug.Log("Player 1 selected character: " + CharacterManager.instance.characters[player1CurrentCharacterIndex].name);
        Debug.Log("Player 2 selected character: " + CharacterManager.instance.characters[player2CurrentCharacterIndex].name);
    }
}
