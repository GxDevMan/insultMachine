using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public List<Sprite> characters;

    // Create variables to store the selected character indices
    private int player1CharacterIndex = 0;
    private int player2CharacterIndex = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Store the selected character indices in PlayerPrefs
    public void SetSelectedCharacters(int player1Index, int player2Index)
    {
        PlayerPrefs.SetInt("Player1CharacterIndex", player1Index);
        PlayerPrefs.SetInt("Player2CharacterIndex", player2Index);
    }

    // Get the selected character sprites
    public Sprite GetPlayer1CharacterSprite()
    {
        int player1Index = PlayerPrefs.GetInt("Player1CharacterIndex", 0); // Default to 0 if not found
        return characters[player1Index];
    }

    public Sprite GetPlayer2CharacterSprite()
    {
        int player2Index = PlayerPrefs.GetInt("Player2CharacterIndex", 1); // Default to 1 if not found
        return characters[player2Index];
    }


    // Find a character sprite based on the character's name
    public Sprite FindCharacterSprite(string characterName)
    {
        return characters.Find(sprite => sprite.name == characterName);
    }
}
