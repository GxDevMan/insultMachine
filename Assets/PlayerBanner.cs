using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBanner : MonoBehaviour
{
    public Image checkmarkImage;

    public void SetAsActivePlayer(bool isActive)
    {
        // Activate/deactivate the checkmark image based on the player's turn.
        checkmarkImage.enabled = isActive;
    }
}

