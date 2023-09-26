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

    void Start()
    {
        player1InputField.onValueChanged.AddListener(OnPlayer1ValueChanged);
        player1InputField.onEndEdit.AddListener(OnPlayer1EndEdit);

        player2InputField.onValueChanged.AddListener(OnPlayer2ValueChanged);
        player2InputField.onEndEdit.AddListener(OnPlayer2EndEdit);

        // Store the initial placeholder texts
        player1PlaceholderText = player1InputField.placeholder.GetComponent<Text>().text;
        player2PlaceholderText = player2InputField.placeholder.GetComponent<Text>().text;

        DisablePlayer2InputField();
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
            GameProper gameProper = FindObjectOfType<GameProper>(); // Find the GameProper script

            if (gameProper != null && gameProper.isGameRunning) // Check if the game is running
            {
                if (!string.IsNullOrEmpty(text) && !IsWordLimitExceeded(text))
                {
                    string sender = "Player 1";
                    SendMessage(sender, text);
                    ClearInputField(player1InputField);

                    gameProper.ResetPlayer1Timer();

                    if (isPlayer1Turn)
                    {
                        gameProper.SwitchTurns();
                        isPlayer1Turn = false;
                    }

                    ToggleInputFields(false, true); // Disable Player 1 input field, enable Player 2 input field
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
            GameProper gameProper = FindObjectOfType<GameProper>(); // Find the GameProper script

            if (gameProper != null && gameProper.isGameRunning) // Check if the game is running
            {
                if (!string.IsNullOrEmpty(text) && !IsWordLimitExceeded(text))
                {
                    string sender = "Player 2";
                    SendMessage(sender, text);
                    ClearInputField(player2InputField);

                    gameProper.ResetPlayer2Timer();

                    if (!isPlayer1Turn)
                    {
                        gameProper.SwitchTurns();
                        isPlayer1Turn = true;
                    }

                    ToggleInputFields(true, false); // Enable Player 1 input field, disable Player 2 input field
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
        if (!string.IsNullOrEmpty(newMessage))
        {
            AddMessageToConversation(sender, newMessage);
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

    public void DisablePlayer2InputField()
    {
        player2InputField.interactable = false;
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
