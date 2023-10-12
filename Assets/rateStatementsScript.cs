using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatementObjDisp
{
    public string statement;
    public bool trueEval;
}

public class rateStatementsScript : MonoBehaviour
{
    MatchManager matchInstance;
    public SQLliteHandler getData;

    public GameObject statementPrefab;
    public Transform statementListContainer;

    private List<statementObj> player1Msg;
    private List<statementObj> player2Msg;
    void Start()
    {
        matchInstance = MatchManager.instance;
        getData = matchInstance.handleSql;
        player1Msg = getData.selectStatements(matchInstance.matchId, matchInstance.player1.playerId);
        player2Msg = getData.selectStatements(matchInstance.matchId, matchInstance.player2.playerId);
        Debug.Log("RATE STATEMENTS");
        PopulateStatementListUI();
        //displayData();
    }


    void displayData()
    {
        

        
        foreach (statementObj statement in player1Msg)
        {
            statement.trueEval = 0;
            Debug.Log("Player 1: " + statement.statement);
        }
        foreach (statementObj statement in player2Msg)
        {
            Debug.Log("Player 2: " + statement.statement);
        }

    }
    private void PopulateStatementListUI()
    {
        foreach (var statementObj in player1Msg)
        {
            GameObject statementItem = Instantiate(statementPrefab, statementListContainer);
            Text statementText = statementItem.GetComponentInChildren<Text>();
            Toggle toxicToggle = statementItem.GetComponentInChildren<Toggle>();

            statementText.text = statementObj.statement;

            if(statementObj.trueEval == 0)
            {
                toxicToggle.isOn = false;
            }
            else
            {
                toxicToggle.isOn = true;
            }

            // Add an event listener for the toggle to handle changes
            toxicToggle.onValueChanged.AddListener(isChecked =>
            {
                if (isChecked)
                {
                    statementObj.trueEval = 1;
                }
                else
                {
                    statementObj.trueEval = 0;
                }
            });
        }
    }


    void OnDestroy()
    {
        getData.rateStatements(player1Msg);
        getData.rateStatements(player2Msg); 
    }

}
