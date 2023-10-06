using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] Toggle[] toggleButtons;

    private void Start()
    {
        // Subscribe to the onValueChanged event of each toggle button
        foreach (Toggle toggle in toggleButtons)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        // Initialize the toggle buttons based on the initial state of cnnsvmSetting
        foreach (Toggle toggle in toggleButtons)
        {
            if (toggle.name == "BagOfWordsToggle") // Replace with the actual name of the BagOfWords toggle
            {
                toggle.isOn = !MatchManager.instance.CnnsvmSetting;
                Debug.Log("Bag of Words Toggle Toggled (Default)");
            }
            else if (toggle.name == "CNNSVMToggle") // Replace with the actual name of the CNNSVM toggle
            {
                toggle.isOn = MatchManager.instance.CnnsvmSetting;
                Debug.Log("CNNSVM Toggle Toggled (Default) - cnnsvmSetting set to " + MatchManager.instance.CnnsvmSetting);
            }
        }
    }

    private void OnToggleValueChanged(bool isOn)
    {
        Toggle selectedToggle = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject?.GetComponent<Toggle>();

        if (selectedToggle != null)
        {
            // Update the cnnsvmSetting in the MatchManager based on the selected toggle
            if (selectedToggle.name == "BagOfWordsToggle") // Replace with the actual name of the BagOfWords toggle
            {
                MatchManager.instance.CnnsvmSetting = false;
                Debug.Log("cnnsvmSetting set to false");
            }
            else if (selectedToggle.name == "CNNSVMToggle") // Replace with the actual name of the CNNSVM toggle
            {
                MatchManager.instance.CnnsvmSetting = true;
                Debug.Log("cnnsvmSetting set to true");
            }

            // Uncheck all other toggle buttons in the group
            foreach (Toggle toggle in toggleButtons)
            {
                if (toggle != selectedToggle)
                {
                    toggle.isOn = false;
                }
            }
        }
    }



}
