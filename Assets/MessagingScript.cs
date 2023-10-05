using UnityEngine;
using UnityEngine.UI;
using System;

public class MessagingScript : MonoBehaviour
{
    public InputField player1InputField;
    public InputField player2InputField;
    public Text conversationText;
    public Text player1CharacterCounter;
    public Text player2CharacterCounter;
    public int maxWords = 5;

    public GameProper gameProper;

    private bool isPlayer1Turn = true;

    private string player1PlaceholderText;
    private string player2PlaceholderText;

    public AudioSource audioSource; // Drag and drop your AudioSource component here in the Unity Inspector
    public CharacterAnimator player1CharacterAnimator;
    public CharacterAnimator player2CharacterAnimator;

    MatchManager matchInstance;

    void Start()
    {
        matchInstance = MatchManager.instance;

        player1InputField.onValueChanged.AddListener(OnPlayer1ValueChanged);
        player1InputField.onEndEdit.AddListener(OnPlayer1EndEdit);

        player2InputField.onValueChanged.AddListener(OnPlayer2ValueChanged);
        player2InputField.onEndEdit.AddListener(OnPlayer2EndEdit);

        // Store the initial placeholder texts
        player1PlaceholderText = player1InputField.placeholder.GetComponent<Text>().text;
        player2PlaceholderText = player2InputField.placeholder.GetComponent<Text>().text;

        //DisablePlayer2InputField();
    }

    private void OnPlayer1ValueChanged(string text)
    {
        UpdateCharacterCounter(text, player1CharacterCounter);
    }

    private void OnPlayer2ValueChanged(string text)
    {
        UpdateCharacterCounter(text, player2CharacterCounter);
    }

    private void OnPlayer1EndEdit(string text)
    {
        if (Input.GetKey(KeyCode.Return))
        {
            GameProper gameProper = FindObjectOfType<GameProper>();

            if (gameProper != null && gameProper.isGameRunning)
            {
                if (!string.IsNullOrEmpty(text) && !IsWordLimitExceeded(text))
                {
                    string sender = "Player 1";
                    SendMessage(sender, text);
                    ClearInputField(player1InputField);

                    // Check if player1CharacterAnimator is assigned
                    if (player1CharacterAnimator != null)
                    {
                        // Play the Totoy attack animation for Player 1
                        player1CharacterAnimator.PlayPlayer1AttackAnimation();
                    }
                    else
                    {
                        Debug.LogError("Player 1 Character Animator is not assigned.");
                    }

                    // Reset Player 1's timer and start it
                    gameProper.ResetPlayer1Timer();
                    gameProper.SwitchTurns();
                }
                else
                {
                    Debug.Log("Player 1: Invalid input.");
                }
            }
            else
            {
                Debug.Log("Game is paused or not running. Input is not allowed.");
            }
        }
    }

    private void OnPlayer2EndEdit(string text)
    {
        if (Input.GetKey(KeyCode.Return))
        {
            GameProper gameProper = FindObjectOfType<GameProper>();

            if (gameProper != null && gameProper.isGameRunning)
            {
                if (!string.IsNullOrEmpty(text) && !IsWordLimitExceeded(text))
                {
                    string sender = "Player 2";
                    SendMessage(sender, text);
                    ClearInputField(player2InputField);

                    // Check if player2CharacterAnimator is assigned
                    if (player2CharacterAnimator != null)
                    {
                        // Play the Totoy attack animation for Player 2 (assuming you want Totoy for Player 2)
                        player2CharacterAnimator.PlayPlayer2AttackAnimation();
                    }
                    else
                    {
                        Debug.LogError("Player 2 Character Animator is not assigned.");
                    }

                    // Reset Player 2's timer and start it
                    gameProper.ResetPlayer2Timer();
                    gameProper.SwitchTurns();
                }
                else
                {
                    Debug.Log("Player 2: Invalid input.");
                }
            }
            else
            {
                Debug.Log("Game is paused or not running. Input is not allowed.");
            }
        }
    }


    void UpdateCharacterCounter(string text, Text characterCounter)
    {
        int wordCount = CountWords(text);
        characterCounter.text = wordCount + "/" + maxWords;
    }

    int CountWords(string text)
    {
        string[] words = text.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }

    bool IsWordLimitExceeded(string text)
    {
        string[] words = text.Split(new char[] { ' ' });
        return words.Length > maxWords; // Check if the number of words is greater than the maximum allowed words (5)
    }

    void SendMessage(string sender, string message)
    {
        string newMessage = message;
        matchInstance.judging(newMessage);
        if (!string.IsNullOrEmpty(newMessage))
        {
            AddMessageToConversation(sender, newMessage);

            // Play the audio clip when a message is sent
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }


    void AddMessageToConversation(string sender, string message)
    {
        string currentConversation = conversationText.text;
        if (!string.IsNullOrEmpty(currentConversation))
        {
            currentConversation += "\n";
        }
        currentConversation += sender + ": " + message;
        conversationText.text = currentConversation;
    }



    public void EnablePlayer2InputField()
    {
        player2InputField.interactable = true;
    }

    public void EnablePlayer1InputField()
    {
        player1InputField.interactable = true;
    }

    public void DisablePlayer1InputField()
    {
        player1InputField.interactable = false;
        if (!string.IsNullOrEmpty(player1InputField.text))
        {
            
            // Display "Player 1: prompt" when disabling player1InputField with content
            SendMessage("Player 1", player1InputField.text);
            ClearInputField(player1InputField);
        }
    }
    public void DisablePlayer2InputField()
    {
        player2InputField.interactable = false;
        if (!string.IsNullOrEmpty(player2InputField.text))
        {
            
            // Display "Player 2: prompt" when disabling player2InputField with content
            SendMessage("Player 2", player2InputField.text);
            ClearInputField(player2InputField);
        }
    }

    private void ClearInputField(InputField inputField)
    {
        inputField.text = "";
    }

    public void SendMessageFromInputField(InputField inputField)
    {
        string sender = isPlayer1Turn ? "Player 1" : "Player 2";
        string message = inputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            SendMessage(sender, message);
            ClearInputField(inputField);
        }
    }

    public void SetCurrentPlayerTurn(InputField inputField)
    {
        isPlayer1Turn = (inputField == player1InputField);
    }

    private void ToggleInputFields(bool enablePlayer1, bool enablePlayer2)
    {
        player1InputField.interactable = enablePlayer1;
        player2InputField.interactable = enablePlayer2;
    }

}
