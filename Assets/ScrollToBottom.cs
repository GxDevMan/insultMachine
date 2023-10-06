using UnityEngine;
using UnityEngine.UI;

public class ScrollToBottom : MonoBehaviour
{
    public ScrollRect scrollRect;

    public void ScrollToBottomOnClick()
    {
        // Set the verticalNormalizedPosition to 0 to scroll to the bottom.
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
