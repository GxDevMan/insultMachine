using System.Collections;
using System.IO;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public static MatchManager instance { get; private set; }
    public static event HandleResult OnFinishTurn;
    public delegate void HandleResult(double result);

    public statementObj mostDamagingStatement { get; private set; }

    public playerObj player1 { get; set; }
    public playerObj player2 { get; set; }

    [SerializeField] public bool cnnsvmSetting = false;
    public int matchId { get; set; }

    public SQLliteHandler handleSql { get; set; }
    private string sqlLoc;

    private playerObj currentPlayer;

    private aiRequester theJudge;
    public bool firstPlayer { get; set; }

    public double result { get; set; }

   
    public double CnnSvmRating { get; private set; } = 0.0;
    public double ChatFilterRating { get; private set; }

    void Start()
    {
        string dbFileName = "loggedArbitration.db";
        string dbPath = Path.Combine(Application.dataPath, dbFileName);

        // The above code constructs a full path to the database file within the game's directory.

        string connectionString = $"URI=file:{dbPath}";

        this.theJudge = new aiRequester();
        this.handleSql = new SQLliteHandler(connectionString, dbPath);
        handleSql.CreateTable();
    }

    public bool CnnsvmSetting
    {
        get { return cnnsvmSetting; }
        set
        {
            cnnsvmSetting = value;
            Debug.Log("cnnsvmSetting set to: " + cnnsvmSetting);
        }
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
        }
        else
        {
            this.currentPlayer = player2;
        }
    }

    public void newMatch(playerObj player1, playerObj player2)
    {
        this.matchId = handleSql.newMatch();
        this.player1 = player1;
        this.player2 = player2;
        player1.playerId = handleSql.newPlayer(player1);
        player2.playerId = handleSql.newPlayer(player2);
        mostDamagingStatement = null;
    }

    public void restartMatch()
    {
        handleSql.DeleteMatchAndStatements(this.matchId);
        this.player1.health = player1.maxHealth;
        this.player2.health = player2.maxHealth;
        this.matchId = handleSql.newMatch();
        mostDamagingStatement = null;
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

        CnnSvmRating = this.cnnsvmSetting ? result.ratingCNNSVM : result.ratingChatFilter;
        ChatFilterRating = !this.cnnsvmSetting ? result.ratingCNNSVM : result.ratingChatFilter;

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
            if (result.boolChatFilter == 1)
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
    
    public statementObj MostDamagingStatement { get { return mostDamagingStatement; } }

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
        // Calculate base damage based on percent damage
        int baseDamage = (int)(currentPlayer.maxDamage * percentDamage);

        // Calculate a random modifier within a range (e.g., -20% to +10%)
        float randomModifier = UnityEngine.Random.Range(-0.2f, 0.1f); // Modify the range as needed

        // Apply the random modifier to the base damage
        int finalDamage = Mathf.RoundToInt(baseDamage * (1 + randomModifier));

        if (currentPlayer == player1)
        {
            Debug.Log($"Final Damage to player2: {finalDamage}");
            player2.health -= finalDamage;
        }
        else
        {
            Debug.Log($"Final Damage to player1: {finalDamage}");
            player1.health -= finalDamage;
        }
    }

    private void handleHeal(double percentHeal)
    {
        // Calculate base heal amount based on percent heal
        int baseHeal = (int)(currentPlayer.maxHeal * percentHeal);

        // Calculate a random modifier within a range (e.g., -25% to +5%)
        float randomModifier = UnityEngine.Random.Range(-0.25f, 0.05f); // Modify the range as needed

        // Apply the random modifier to the base heal
        int finalHeal = Mathf.RoundToInt(baseHeal * (1 + randomModifier));

        if (currentPlayer == player1)
        {
            player1.health += finalHeal;
            Debug.Log($"Final Heal to player1: {finalHeal}");
            if (player1.health > player1.maxHealth)
            {

                player1.health = player1.maxHealth;
            }
        }
        else
        {
            player2.health += finalHeal;
            Debug.Log($"Final Heal to player2: {finalHeal}");
            if (player2.health > player2.maxHealth)
            {
                player2.health = player2.maxHealth;
            }
        }
    }

}
