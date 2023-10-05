using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinBanner : MonoBehaviour
{
    public Text player1WinnerText;
    public Text player2WinnerText;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the winner's ID from PlayerPrefs
        int winnerID = PlayerPrefs.GetInt("WinnerID", 0);

        // Check the winner and enable the corresponding text
        if (winnerID == 1)
        {
            player1WinnerText.gameObject.SetActive(true);
        }
        else if (winnerID == 2)
        {
            player2WinnerText.gameObject.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
