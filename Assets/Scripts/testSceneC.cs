using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
public class testSceneC : MonoBehaviour
{
    public InputField inputArea;
    public InputField outputArea;
    public Button sendText;

    private int matchId;
    MatchManager matchmanager;
    void Start()
    {
        matchmanager = MatchManager.instance;
        playerObj player1 = new playerObj("player1",50,10,10);
        playerObj player2 = new playerObj("player2", 50, 10, 10);
        matchmanager.newMatch(player1,player2);
        matchmanager.cnnsvmSetting = false;
        matchmanager.setFirstPlayer(false);

        sendText.onClick.AddListener(judgeEvent);
    }

    public void judgeEvent()
    {
        matchmanager.judging(inputArea.text.Trim());
        //outputArea.text = $"result: {matchmanager.result.ToString()}";
    }

    private void OnEnable()
    {
        MatchManager.OnFinishTurn += invokeResult;
    }

    private void OnDisable()
    {
        MatchManager.OnFinishTurn -= invokeResult;
    }

    public void invokeResult(double result)
    {
        outputArea.text = result.ToString();
    }


}
