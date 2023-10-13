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

    public GameObject statementEntryPrefabP1; // Rename this variable for clarity
    public Transform statementListContainerP1;
    public GameObject statementEntryPrefabP2; // Rename this variable for clarity
    public Transform statementListContainerP2;

    private List<statementObj> player1Msg;
    private List<statementObj> player2Msg;

    public Text player1MessageText;
    public Text player2MessageText;
    public int maxCheckboxes = 2;

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
        string player1Messages = "\n";
        string player2Messages = "\n";

        foreach (statementObj statement in player1Msg)
        {
            player1Messages += "Player 1: " + statement.statement + "\n";
        }
        foreach (statementObj statement in player2Msg)
        {
            player2Messages += "Player 2: " + statement.statement + "\n";
        }

        player1MessageText.text = player1Messages;
        player2MessageText.text = player2Messages;
    }

    private void PopulateStatementListUI()
    {
        foreach (var statementObj in player1Msg)
        {
            GameObject statementItem = Instantiate(statementEntryPrefabP1, statementListContainerP1);
            Transform container = statementItem.transform.Find("Content"); // Find the container GameObject.
            Text statementText = statementItem.GetComponentInChildren<Text>();
            Toggle[] checkboxes = statementItem.GetComponentsInChildren<Toggle>();

            // Set the statement text
            statementText.text = "Player 1: " + statementObj.statement;
            //statementText.text = "Player 1: " + statementObj.statement + "\t\t\t\t\t\t";
            // checkboxes[0] represents the first checkbox, checkboxes[1] represents the second checkbox, and so on.

            // Add event listeners for each checkbox to handle changes
            for (int i = 0; i < checkboxes.Length; i++)
            {
                int checkboxIndex = i; // Store the index to access it inside the listener

                checkboxes[i].onValueChanged.AddListener(isChecked =>
                {
                    // Handle checkbox state change for checkboxes[checkboxIndex]
                });
            }
        }

        foreach (var statementObj in player2Msg)
        {
            GameObject statementItem = Instantiate(statementEntryPrefabP2, statementListContainerP2);
            Transform container = statementItem.transform.Find("Content"); // Find the container GameObject.
            Text statementText = statementItem.GetComponentInChildren<Text>();
            Toggle[] checkboxes = statementItem.GetComponentsInChildren<Toggle>();
            
            // Set the statement text
            statementText.text = "Player 2: " + statementObj.statement;
            //statementText.text = "Player 1: " + statementObj.statement + "\t\t\t\t\t\t";
            // checkboxes[0] represents the first checkbox, checkboxes[1] represents the second checkbox, and so on.

            // Add event listeners for each checkbox to handle changes
            for (int i = 0; i < checkboxes.Length; i++)
            {
                int checkboxIndex = i; // Store the index to access it inside the listener

                checkboxes[i].onValueChanged.AddListener(isChecked =>
                {
                    // Handle checkbox state change for checkboxes[checkboxIndex]
                });
            }
        }
    }


    void OnDestroy()
    {
        getData.rateStatements(player1Msg);
        getData.rateStatements(player2Msg);
    }
}
