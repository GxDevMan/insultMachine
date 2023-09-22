using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class testSceneC : MonoBehaviour
{
    private aiRequester theJudge;
    public InputField inputArea;
    public InputField outputArea;
    public Button sendText;
    void Start()
    {
        theJudge = new aiRequester();
        sendText.onClick.AddListener(arbritate);
    }

    private void arbritate()
    {
        char[] delimiters = { ';' };

        string[] items = inputArea.text.Split(delimiters, StringSplitOptions.None);
        List<string> textsToSend = new List<string>();

        for(int i = 0; i < items.Length; i++)
        {
            textsToSend.Add(items[i]);
        }

        StartCoroutine(theJudge.SendPostRequest(textsToSend, HandlePostRequestResult));
    }

    private void HandlePostRequestResult(string result)
    {
        string resultJudge = "";
        if (!string.IsNullOrEmpty(result))
        {
            VerdictData verdictdata = JsonUtility.FromJson<VerdictData>(result);
            List<double> judgementNumbers = verdictdata.verdict;

            if (judgementNumbers.Count > 0)
            {
                if (judgementNumbers.Count == 1)
                {
                    resultJudge += judgementNumbers[0];
                }
                else
                {
                    int index = 0;

                    while(index < judgementNumbers.Count)
                    {
                        if (index == judgementNumbers.Count - 1)
                        {
                            resultJudge += judgementNumbers[index];
                            index++;
                        }
                        else
                        {
                            resultJudge += judgementNumbers[index] + ",";
                            index++;
                        }
                    }
                }
                outputArea.text = resultJudge;
            }
            else
            {
                outputArea.text = "No Result";
            }
        }
        else
        {
            Debug.LogError("No result or request failed.");
        }
    }
    [System.Serializable]
    public class VerdictData
    {
        public List<double> verdict;
    }
}
