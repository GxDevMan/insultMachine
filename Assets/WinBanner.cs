﻿using UnityEngine;
using UnityEngine.UI;

public class WinBanner : MonoBehaviour
{
    public Text player1WinnerText;
    public Text player2WinnerText;
    public Text playerTieText;
    public Text ratingText;
    public Text mostDamagePhrasesText; // Add a reference to the new Text component
    public Text gameDurationText;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve the actual time remaining from PlayerPrefs
        float timeRemaining = PlayerPrefs.GetFloat("TimeRemaining", 0.0f);

        // Display the actual time remaining
        string formattedTime = FormatTime(timeRemaining);
        if (gameDurationText != null)
        {
            gameDurationText.text = "Time Remaining: " + formattedTime;
        }

        string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            return string.Format("{0}:{1:00}", minutes, seconds);
        }

        // Retrieve the winner's ID from PlayerPrefs
        int winnerID = PlayerPrefs.GetInt("WinnerID", 0);

        // Check the winner and enable the corresponding text
        MatchManager matchInstance = MatchManager.instance;
        playerObj player1 = matchInstance.player1;
        playerObj player2 = matchInstance.player2;

        if (player1.health > player2.health)
        {
            player1WinnerText.gameObject.SetActive(true);
        }
        else if (player1.health == player2.health)
        {
            playerTieText.gameObject.SetActive(true);
        }
        else
        {
            player2WinnerText.gameObject.SetActive(true);
        }

        MatchManager managerMatch = MatchManager.instance;
        statementObj damagingStatement = managerMatch.mostDamagingStatement;

        double ratingValue = MatchManager.instance.CnnsvmSetting ? damagingStatement.ratingCNNSVM : damagingStatement.ratingChatFilter;
        

        string ratingLabel = MatchManager.instance.CnnsvmSetting ? "Most Damage Phrases Rating (CNN/SVM): " : "Most Damage Phrases Rating (BagOfWords): ";

        if (ratingText != null)
        {
            ratingText.text = ratingLabel + (ratingValue * 100).ToString("F2") + "%";
        }

        // Retrieve and display the "Most Damage Phrases Text"
        if (mostDamagePhrasesText != null)
        {
            mostDamagePhrasesText.text = "Most Damage Phrases Text: " + MatchManager.instance.MostDamagingStatement.statement;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}