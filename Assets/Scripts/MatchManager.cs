using System.Collections;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public static MatchManager instance;
    public static event HandleResult OnFinishTurn;
    public delegate void HandleResult(double result);

    public statementObj mostDamagingStatement { get; private set; }

    public playerObj player1 { get; set; }
    public playerObj player2 { get; set; }
    public bool cnnsvmSetting { get; set; }
    public int matchId { get; set; }

    public SQLliteHandler handleSql;
    private string sqlLoc;

    private playerObj currentPlayer;

    private aiRequester theJudge;
    public bool firstPlayer { get; set; }

    public double result { get; set; }
    void Start()
    {
        cnnsvmSetting = false;
        string template = "URI=file:";
        string relativeLoc = "Assets/Scripts/data/loggedArbitration.db";
        sqlLoc = $"{template}{relativeLoc}";

        this.theJudge = new aiRequester();
        this.handleSql = new SQLliteHandler(sqlLoc, relativeLoc);
        handleSql.CreateTable();
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

    public void setFirstPlayer(bool firstPlayer)
    {
        this.firstPlayer = firstPlayer;
        if (firstPlayer)
        {
            this.currentPlayer = player1;
            Debug.Log($"first turn is player1: {currentPlayer.playerId}");
        }
        else
        {
            this.currentPlayer = player2;
            Debug.Log($"first turn is player2: {currentPlayer.playerId}");
        }
    }

    public void newMatch(playerObj player1, playerObj player2)
    {
        this.matchId = handleSql.newMatch();
        this.player1 = player1;
        this.player2 = player2;
        player1.playerId = handleSql.newPlayer(player1);
        player2.playerId = handleSql.newPlayer(player2);
    }

    public void judging(string text)
    {
        if (text != null)
        {
            StartCoroutine(arbitrate(text));
        }
        else
        {
            swapPlayers();
        }
    }
    private IEnumerator arbitrate(string input)
    {
        statementObj newstatementChatFilter = new statementObj(currentPlayer.playerId, matchId, input);
        yield return StartCoroutine(theJudge.SendJudgementPostRequest(newstatementChatFilter, handleJudgeMent));
    }
    private void handleJudgeMent(statementObj result)
    {
        handleSql.InsertStatement(result);
        resultJudge(result);
        setMaxDamagingStatement(result);
        

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

        swapPlayers();

        if (cnnsvmSetting)
        {
            OnFinishTurn?.Invoke(result.ratingCNNSVM);
        }
        else
        {
            OnFinishTurn?.Invoke(result.ratingChatFilter);
        }
    }

    private void setMaxDamagingStatement(statementObj newStatement)
    {
        if (mostDamagingStatement == null)
            mostDamagingStatement = newStatement;
        else
        {
            if (cnnsvmSetting)
            {
                if (mostDamagingStatement.ratingCNNSVM < newStatement.ratingCNNSVM)
                    mostDamagingStatement = newStatement;
            }
            else
            {
                if (mostDamagingStatement.ratingChatFilter < newStatement.ratingChatFilter)
                    mostDamagingStatement = newStatement;
            }
        }
    }

    public void swapPlayers()
    {
        if (currentPlayer == player1)
        {
            currentPlayer = player2;

        }
        else
        {
            currentPlayer = player1;
        }

        Debug.Log($"Switching players. Current player ID: {currentPlayer.playerId}");
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
            int damage = (int)(player1.maxDamage * percentDamage);
            player2.health -= damage;
            //Debug.Log($"player 1 damage: {damage}");
        }
        else
        {
            int damage = (int)(player2.maxDamage * percentDamage);
            player1.health -= damage;
            //Debug.Log($"player 2 damage: {damage}");
        }
    }

    private void handleHeal(double percentHeal)
    {
        if (currentPlayer == player1)
        {
            player1.health += (int)(player1.maxHeal * percentHeal);
            if (player1.health > player1.maxHealth)
            {
                player1.health = player1.maxHealth;
            }
        }
        else
        {
            player2.health += (int)(player2.maxHeal * percentHeal);
            if (player2.health > player2.maxHealth)
            {
                player2.health = player2.maxHealth;
            }
        }
    }

}
