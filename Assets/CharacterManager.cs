using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public List<Sprite> characters;

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

    public void SetSelectedCharacters(int player1Index, int player2Index)
    {
        PlayerPrefs.SetInt("Player1CharacterIndex", player1Index);
        PlayerPrefs.SetInt("Player2CharacterIndex", player2Index);
    }

    
    public Sprite GetPlayer1CharacterSprite()
    {
        int player1Index = PlayerPrefs.GetInt("Player1CharacterIndex", 0); 
        return characters[player1Index];
    }

    public Sprite GetPlayer2CharacterSprite()
    {
        int player2Index = PlayerPrefs.GetInt("Player2CharacterIndex", 1); 
        return characters[player2Index];
    }
    public Sprite FindCharacterSprite(string characterName)
    {
        return characters.Find(sprite => sprite.name == characterName);
    }
}
