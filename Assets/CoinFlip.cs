﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinFlip : MonoBehaviour
{
    public Transform coinTransform;
    public GameObject player1Checkmark;
    public GameObject player2Checkmark;
    public Sprite headsSprite;
    public Sprite tailsSprite;
    public float spinDuration = 2f;
    public int spins = 2;
    public float headsSpinSpeed = 900f; // Spin speed for heads.
    public float tailsSpinSpeed = 500f; // Spin speed for tails.
    public Image player1CharacterImage; // Reference to Player 1's character image.
    public Image player2CharacterImage; // Reference to Player 2's character image.

    public Text player1CharacterNameText;
    public Text player2CharacterNameText;


    public bool isHeads;
    private SpriteRenderer coinRenderer;

    public AudioSource coinFlipAudioSource;
    public AudioSource checkMarkAudioSource;

    MatchManager matchInstance;

    private void Start()
    {

        matchInstance = MatchManager.instance;

        Debug.Log("CoinFlip script started.");
        coinRenderer = GetComponent<SpriteRenderer>();
        player1Checkmark.SetActive(false);
        player2Checkmark.SetActive(false);
        coinFlipAudioSource.Play();
        StartCoroutine(SpinCoin());

        // Load the selected character names from PlayerPrefs
        string player1CharacterName = PlayerPrefs.GetString("character_TotoyIdle");
        string player2CharacterName = PlayerPrefs.GetString("character_AsianMomIdle");

        // Update the character names in the scene
        player1CharacterNameText.text = player1CharacterName;
        player2CharacterNameText.text = player2CharacterName;

        // Load the selected character sprites from CharacterManager
        Sprite player1CharacterSprite = CharacterManager.instance.GetPlayer1CharacterSprite();
        Sprite player2CharacterSprite = CharacterManager.instance.GetPlayer2CharacterSprite();

        // Update the character images in the scene
        player1CharacterImage.sprite = player1CharacterSprite;
        player2CharacterImage.sprite = player2CharacterSprite;
    }

    private Sprite FindCharacterSprite(string characterName)
    {
        // Find the character sprite based on the name in the characters list
        return CharacterManager.instance.characters.Find(sprite => sprite.name == characterName);
    }

    private IEnumerator SpinCoin()
    {
        Debug.Log("SpinCoin coroutine started.");
        int spinsRemaining = spins;

        // Randomly determine if it's heads or tails.
        bool isHeads = Random.value < 0.5f;

        // Store the result in PlayerPrefs
        PlayerPrefs.SetInt("CoinResult", isHeads ? 1 : 2); // 1 for heads, 2 for tails

        while (spinsRemaining > 0)
        {
            // Simulate a single spin.
            float spinProgress = 0f;
            float spinSpeed = isHeads ? headsSpinSpeed : tailsSpinSpeed;

            while (spinProgress < spinDuration)
            {
                coinRenderer.sprite = isHeads ? headsSprite : tailsSprite;

                coinTransform.Rotate(Vector3.up, spinSpeed * Time.deltaTime); // Rotate around the Y-axis (upwards).
                spinProgress += Time.deltaTime;

                yield return null;
            }

            spinsRemaining--;
        }

        // Ensure the coin completes a full rotation cycle.
        float fullRotationTime = 360f / (isHeads ? headsSpinSpeed : tailsSpinSpeed);
        yield return new WaitForSeconds(fullRotationTime);

        // Show the checkmark for the respective player.
        if (isHeads)
        {
            checkMarkAudioSource.Play();
            Debug.Log("Player 1 (Heads) goes first.");
            player1Checkmark.SetActive(true);
            player2Checkmark.SetActive(false); // Hide Player 2's checkmark

            startMatch(true);
        }
        else
        {
            checkMarkAudioSource.Play();
            Debug.Log("Player 2 (Tails) goes first.");
            player1Checkmark.SetActive(false); // Hide Player 1's checkmark
            player2Checkmark.SetActive(true);

            startMatch(false);
        }

        Debug.Log("SpinCoin coroutine completed.");

        // Transition to the "GameProper" scene after a delay (e.g., 2 seconds).
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameProper");
    }


    private void startMatch(bool firstPlayer)
    {
        playerObj player1 = new playerObj("Player 1", 100, 18, 20);
        playerObj player2 = new playerObj("Player 2", 100, 18, 20);
        matchInstance.newMatch(player1, player2);
        matchInstance.setFirstPlayer(firstPlayer);
    }
}
