using System.Collections;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public static MatchManager instance;
    public playerObj player1 { get; set; }
    public playerObj player2 { get; set; }
    public bool cnnsvmSetting { get; set; }
    public int matchId { get; set; }


    private SQLliteHandler handleSql;
    private string sqlLoc;

    private playerObj currentPlayer;

    public string firstPlayer { get; set; }
    private aiRequester theJudge;

    public double result { get; set; }


    void Start()
    {
        handleSql.CreateTable();
        string template = "URI=file:";
        string relativeLoc = "Assets/Code/data/loggedArbitration.db";
        sqlLoc = $"{template}{relativeLoc}";

        this.theJudge = new aiRequester();
        this.handleSql = new SQLliteHandler(sqlLoc, relativeLoc);

        player1 = new playerObj("Player 1", 100, 20, 20);
        player2 = new playerObj("Player 2", 100, 20, 20);

        this.matchId = handleSql.newMatch();
        player1.playerId = handleSql.newPlayer(player1);
        player2.playerId = handleSql.newPlayer(player2);

        if (firstPlayer == firstPlayer)
        {
            this.currentPlayer = player1;
        }
        else
        {
            this.currentPlayer = player2;
        }
    }

    void Update()
    {
        
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void judging(string text)
    {
        StartCoroutine(ArbitrateAndSwap(text));
    }

    private IEnumerator ArbitrateAndSwap(string text)
    {
        string input = text;
        yield return StartCoroutine(arbitrate(input));

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
        yield return StartCoroutine(theJudge.SendJudgementPostRequest(newstatementChatFilter, handleJudgeMent));
    }


    public void newMatch()
    {
        this.matchId = handleSql.newMatch();

        player1 = new playerObj("Player 1", 100, 20, 20);
        player2 = new playerObj("Player 2", 100, 20, 20);

        player1.playerId = handleSql.newPlayer(player1);
        player2.playerId = handleSql.newPlayer(player2);
    }

    private void handleJudgeMent(statementObj result)
    {
        handleSql.InsertStatement(result);
        resultJudge(result);

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

    private void resultJudge(statementObj result)
    {
        if (this.cnnsvmSetting)
        {
            this.result = result.ratingCNNSVM;
        }
        else
        {
            this.result = result.ratingChatFilter;
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
