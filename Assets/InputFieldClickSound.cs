using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFieldClickSound : MonoBehaviour
{
    public InputField inputField1;
    public InputField inputField2;
    public AudioSource audioSource;
    public AudioClip clickSound;

    private bool isTyping = false;
    private bool hasClicked = false;

    private void Start()
    {
        // Make sure to set the inputField1, inputField2, audioSource, and clickSound references in the Inspector.
        // Also, assign the clickSound AudioClip to the clickSound field for both input fields.

        // Add event triggers for both input fields
        AddClickEvent(inputField1);
        AddClickEvent(inputField2);
    }

    private void AddClickEvent(InputField inputField)
    {
        if (inputField == null) return;

        EventTrigger trigger = inputField.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = inputField.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => OnInputFieldClicked(inputField));

        trigger.triggers.Add(entry);
    }

    private void OnInputFieldClicked(InputField inputField)
    {
        // Check if the input field is enabled before playing the click sound
        if (inputField != null && inputField.interactable)
        {
            // Play the click sound when the input field is clicked
            if (audioSource && clickSound)
            {
                audioSource.PlayOneShot(clickSound);
            }

            // Check if this is the first click on the input field (not typing)
            if (!hasClicked)
            {
                hasClicked = true;
                return;
            }

            // Check if the user is currently typing in the input field
            if (!isTyping)
            {
                isTyping = true;
            }
        }
    }
}
