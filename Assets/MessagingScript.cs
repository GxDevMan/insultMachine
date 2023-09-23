using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MessagingScript : MonoBehaviour
{
    public InputField player1InputField;
    public InputField player2InputField;
    public Text conversationText;
    public Text player1CharacterCounter;
    public Text player2CharacterCounter;
    public int maxWords = 20; // Maximum words allowed in the chat

    private bool isPlayer1Turn = true; // Flag to track whose turn it is

    private void Start()
    {
        // Use both onValueChanged and onEndEdit to capture text input
        player1InputField.onValueChanged.AddListener(OnPlayer1ValueChanged);
        player1InputField.onEndEdit.AddListener(OnPlayer1EndEdit);

        player2InputField.onValueChanged.AddListener(OnPlayer2ValueChanged);
        player2InputField.onEndEdit.AddListener(OnPlayer2EndEdit);

        // Initially, it's player 1's turn, so disable player 2's input field
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
            // Check if Enter key was pressed, and the word limit is not exceeded
            if (!IsWordLimitExceeded(text))
            {
                string sender = "Player 1";
                SendMessage(sender, text);
                ClearInputField(player1InputField);
            }
            else
            {
                // Inform the player that the word limit is exceeded
                Debug.Log("Player 1: Word limit exceeded.");
            }
        }
    }

    private void OnPlayer2EndEdit(string text)
    {
        if (Input.GetKey(KeyCode.Return))
        {
            // Check if Enter key was pressed, and the word limit is not exceeded
            if (!IsWordLimitExceeded(text))
            {
                string sender = "Player 2";
                SendMessage(sender, text);
                ClearInputField(player2InputField);
            }
            else
            {
                // Inform the player that the word limit is exceeded
                Debug.Log("Player 2: Word limit exceeded.");
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
        int wordCount = CountWords(text);
        return wordCount > maxWords;
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

    // Disable the other player's input field
    public void DisablePlayer2InputField()
    {
        player2InputField.interactable = false;
    }

    // Enable Player 2's input field
    public void EnablePlayer2InputField()
    {
        player2InputField.interactable = true;
    }

    // Enable Player 1's input field
    public void EnablePlayer1InputField()
    {
        player1InputField.interactable = true;
    }

    // Disable Player 1's input field
    public void DisablePlayer1InputField()
    {
        player1InputField.interactable = false;
    }

    // Clear the input field
    private void ClearInputField(InputField inputField)
    {
        inputField.text = "";
    }
}
