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

    public GameObject statementEntryPrefabP1; 
    public Transform statementListContainerP1;
    public GameObject statementEntryPrefabP2; 
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
        PopulateStatementListUI();
    }

    private void PopulateStatementListUI()
    {
        int statementCount = 0;
        foreach (var statementObj in player1Msg)
        {
            int currentStatementIndex = statementCount;
            GameObject statementItem = Instantiate(statementEntryPrefabP1, statementListContainerP1);
            Transform container = statementItem.transform.Find("Content");
            Text statementText = statementItem.GetComponentInChildren<Text>();
            Toggle[] checkboxes = statementItem.GetComponentsInChildren<Toggle>();

            statementText.text = "Player 1: " + statementObj.statement;
            for (int i = 0; i < checkboxes.Length; i++)
            {
                int checkboxIndex = i; 

                checkboxes[i].onValueChanged.AddListener(isChecked =>
                {
                    if(checkboxIndex == 1 && isChecked)
                    {
                        player1Msg[currentStatementIndex].trueEval = 0;
                    }
                    if(checkboxIndex == 0 && isChecked)
                    {
                        player1Msg[currentStatementIndex].trueEval = 1;
                    }
                });
            }
            statementCount++;
        }

        statementCount = 0;
        foreach (var statementObj in player2Msg)
        {
            int currentStatementIndex = statementCount;
            GameObject statementItem = Instantiate(statementEntryPrefabP2, statementListContainerP2);
            Transform container = statementItem.transform.Find("Content"); 
            Text statementText = statementItem.GetComponentInChildren<Text>();
            Toggle[] checkboxes = statementItem.GetComponentsInChildren<Toggle>();
            
            statementText.text = "Player 2: " + statementObj.statement;
            for (int i = 0; i < checkboxes.Length; i++)
            {
                int checkboxIndex = i; 

                checkboxes[i].onValueChanged.AddListener(isChecked =>
                {
                    if (checkboxIndex == 1 && isChecked)
                    {
                        player2Msg[currentStatementIndex].trueEval = 0;
                    }
                    if(checkboxIndex == 0 && isChecked)
                    {
                        player2Msg[currentStatementIndex].trueEval = 1;
                    }
                });
            }
            statementCount++;
        }

        Destroy(statementEntryPrefabP1);
        Destroy(statementEntryPrefabP2);

        // Once you've populated the UI with clones, you can destroy or deactivate the prefab.
        // Destroy the prefab:
        // Or, if you want to deactivate them (hide them) rather than destroying them:
        // statementEntryPrefabP1.SetActive(false);
        // statementEntryPrefabP2.SetActive(false);
    }


    void OnDestroy()
    {
        getData.rateStatements(player1Msg);
        getData.rateStatements(player2Msg);
    }
}
