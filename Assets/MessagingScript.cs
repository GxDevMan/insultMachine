using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MessagingScript : MonoBehaviour
{
    public InputField player1InputField;
    public InputField player2InputField;
    public Text conversationText;
    public Button player1SendButton;
    public Button player2SendButton;

    private void Start()
    {
        player1InputField.onEndEdit.AddListener(OnPlayer1EndEdit);
        player2InputField.onEndEdit.AddListener(OnPlayer2EndEdit);

        player1SendButton.onClick.AddListener(OnPlayer1SendButtonClicked);
        player2SendButton.onClick.AddListener(OnPlayer2SendButtonClicked);
    }

    private void OnPlayer1SendButtonClicked()
    {
        SendMessage("Player 1", player1InputField.text);
        player1InputField.text = "";
    }

    private void OnPlayer2SendButtonClicked()
    {
        SendMessage("Player 2", player2InputField.text);
        player2InputField.text = "";
    }

    private void OnPlayer1EndEdit(string text)
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            SendMessage("Player 1", player1InputField.text);
            player1InputField.text = "";
        }
    }

    private void OnPlayer2EndEdit(string text)
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            SendMessage("Player 2", player2InputField.text);
            player2InputField.text = "";
        }
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
}


