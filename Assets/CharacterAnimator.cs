using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAnimator : MonoBehaviour
{
    public Image player1CharacterImage;
    public Image player2CharacterImage;

    public Sprite[] totoySprites; 
    public Sprite[] karenSprites; 

    private void Awake()
    {
        int player1CharacterIndex = PlayerPrefs.GetInt("Player1CharacterIndex", 0);
        int player2CharacterIndex = PlayerPrefs.GetInt("Player2CharacterIndex", 1);

        player1CharacterImage.sprite = CharacterManager.instance.characters[player1CharacterIndex];
        player2CharacterImage.sprite = CharacterManager.instance.characters[player2CharacterIndex];
    }

    public void PlayPlayer1AttackAnimation()
    {
        int player1CharacterIndex = PlayerPrefs.GetInt("Player1CharacterIndex", 0);

        if (player1CharacterIndex == 0)
        {
            StartCoroutine(AnimateCharacter(player1CharacterImage, totoySprites));
        }
        else if (player1CharacterIndex == 1)
        {
            StartCoroutine(AnimateCharacter(player1CharacterImage, karenSprites));
        }
    }

    public void PlayPlayer2AttackAnimation()
    {
        int player2CharacterIndex = PlayerPrefs.GetInt("Player2CharacterIndex", 1);

        if (player2CharacterIndex == 0)
        {
            StartCoroutine(AnimateCharacter(player2CharacterImage, totoySprites));
        }
        else if (player2CharacterIndex == 1)
        {
            StartCoroutine(AnimateCharacter(player2CharacterImage, karenSprites));
        }
    }

    private IEnumerator AnimateCharacter(Image characterImage, Sprite[] sprites)
    {
        if (characterImage == null || sprites.Length == 0)
            yield break;

        Sprite defaultSprite = characterImage.sprite;
        foreach (Sprite sprite in sprites)
        {
            characterImage.sprite = sprite;
            yield return new WaitForSeconds(0.2f); 
        }
        characterImage.sprite = defaultSprite;
    }
}
