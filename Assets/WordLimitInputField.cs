using UnityEngine;
using UnityEngine.UI;

public class WordLimitInputField : MonoBehaviour
{
    public int maxWordCount = 5; // Change this to the desired word limit
    public InputField inputField1; // Reference to the first InputField
    public InputField inputField2; // Reference to the second InputField

    private void Start()
    {
        if (inputField1 != null)
        {
            inputField1.onValueChanged.AddListener(OnInputValueChanged);
        }
        else
        {
            Debug.LogError("InputField1 reference is not set. Drag and drop the first InputField in the Inspector.");
        }

        if (inputField2 != null)
        {
            inputField2.onValueChanged.AddListener(OnInputValueChanged);
        }
        else
        {
            Debug.LogError("InputField2 reference is not set. Drag and drop the second InputField in the Inspector.");
        }
    }

    private void OnInputValueChanged(string text)
    {
        // Check which InputField triggered the event
        InputField currentInputField = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();

        if (currentInputField == inputField1 || currentInputField == inputField2)
        {
            // Split the text into words
            string[] words = text.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

            // Check if the word count exceeds the limit
            if (words.Length > maxWordCount)
            {
                // Truncate the text to the allowed word limit
                string truncatedText = string.Join(" ", words, 0, maxWordCount);
                currentInputField.text = truncatedText;
            }
        }
    }
}
