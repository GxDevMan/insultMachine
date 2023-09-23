using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class testSceneC : MonoBehaviour
{
    public InputField inputArea;
    public InputField outputArea;
    public Button sendText;
    private string sqlLoc;

    private aiRequester theJudge;
    private int matchId;
    public bool cnnsvmSetting { get; set; }
    private SQLliteHandler handleSql;
    public playerObj player1 { get; set; }
    public playerObj player2 { get; set; }

    public playerObj currentPlayer;
    string firstPlayer;

    void Start()
    {
        string template = "URI=file:";
        string relativeLoc = "Assets/Code/data/loggedArbitration.db";
        sqlLoc = $"{template}{relativeLoc}";

        sendText.onClick.AddListener(judging);
        
        this.cnnsvmSetting = false;
        this.theJudge = new aiRequester();
        this.handleSql = new SQLliteHandler(sqlLoc, relativeLoc);
        this.firstPlayer = "player1";

        handleSql.CreateTable();
        player1 = new playerObj("Player 1", 100, 20, 20);
        player2 = new playerObj("Player 2", 100, 20, 20);

        this.matchId = handleSql.newMatch();
        player1.playerId = handleSql.newPlayer(player1);
        player2.playerId = handleSql.newPlayer(player2);


        if (firstPlayer == "player1")
        {
            this.currentPlayer = player1;
        }
        else
        {
            this.currentPlayer = player2;
        }
    }

    private void judging()
    {
        StartCoroutine(ArbitrateAndSwap());
    }


    private IEnumerator ArbitrateAndSwap()
    {
        // Call the arbitrate method and wait for it to finish
        string input = inputArea.text;
        yield return StartCoroutine(arbitrate(input));

        // Now, currentPlayer is updated after the arbitrate method has completed
        if (currentPlayer.playerName == "Player 1")
        {
            currentPlayer = player2;
            Debug.Log($"Current Player: {currentPlayer.playerId}");
        }
        else
        {
            currentPlayer = player1;
            Debug.Log($"Current Player: {currentPlayer.playerId}");
        }
    }

    private IEnumerator arbitrate(string input)
    {
        statementObj newstatementChatFilter = new statementObj(currentPlayer.playerId, matchId, input);

        // Start the coroutine and wait for it to finish
        yield return StartCoroutine(theJudge.SendJudgementPostRequest(newstatementChatFilter, handleJudgeMent));
    }


    public void displayJudgement(statementObj ratedStatement)
    {
        outputArea.text = ratedStatement.statement;
        outputArea.text = $"ratingCNNSVM: {ratedStatement.ratingCNNSVM} ratingChatFilter: {ratedStatement.ratingChatFilter}";
    }

    private void handleJudgeMent(statementObj result)
    {
        handleSql.InsertStatement(result);
        displayJudgement(result);

        if (this.cnnsvmSetting)
        {
            if (result.ratingCNNSVM > 0.5)
            {
                handleAttack(result.ratingCNNSVM);
            }

            else
            {
                handleHeal(1 - result.ratingCNNSVM);
            }
        }
        else
        {
            if (result.ratingChatFilter > 0.5)
            {
                handleAttack(result.ratingChatFilter);
            }

            else
            {
                handleHeal(1 - result.ratingChatFilter);
            }

        }
    }

    private void handleAttack(double percentDamage)
    {
        if (currentPlayer == player1)
        {
            player2.health -= (int)(player1.maxDamage * percentDamage);
            currentPlayer = this.player2;
        }
        else
        {
            player1.health -= (int)(player2.maxDamage * percentDamage);
            currentPlayer = this.player1;
        }
    }

    private void handleHeal(double percentHeal)
    {
        if (currentPlayer == player1)
        {
            player2.health += (int)(player2.maxHeal * percentHeal);
            if (player2.health > player2.maxHealth)
            {
                player2.health = player2.maxHealth;
            }
            currentPlayer = player1;
        }
        else
        {
            player1.health += (int)(player1.maxHeal * percentHeal);
            if (player1.health > player1.maxHealth)
            {
                player1.health = player1.maxHealth;
            }
            currentPlayer = player2;
        }
    }
}
