using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class aiRequester
{
    private string apiUrl = "http://127.0.0.1:8000/cnnsvm/";
    public delegate void judgeCallback(statementObj result);
    private judgeCallback judgementCallback;
    private double threshHold;

    public aiRequester()
    {
        this.threshHold = 0.5;
    }

    public IEnumerator SendJudgementPostRequest(statementObj textData, judgeCallback callback)
    {
        judgementCallback = callback;
        yield return judgeText(textData);
    }


    private IEnumerator judgeText(statementObj textsToSend)
    {
        statementObj editThis = textsToSend;
        yield return (SendChatFilterRequest(editThis));
        string urlAppend = "aiJudge/";
        string jsonData = $"{{ \"text\": [\"{textsToSend.statement}\"]}}";

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl + urlAppend, jsonData))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError("POST Request Failed: " + request.error);
            }
            else
            {
                string responseData = request.downloadHandler.text;
                if (!string.IsNullOrEmpty(responseData))
                {
                    VerdictData verdictdata = JsonUtility.FromJson<VerdictData>(responseData);
                    double judgementNumber = verdictdata.verdict;
                    double roundedValue = Math.Round(judgementNumber, 2);
                    editThis.ratingCNNSVM = roundedValue;
                    if (judgementNumber >= 1)
                    {
                        editThis.ratingCNNSVM = 1;
                    }

                    if (roundedValue > threshHold)
                    {
                        editThis.boolCNNSVM = 1;
                    }
                    else
                    {
                        editThis.boolCNNSVM = 0;
                    }

                }
                else
                {
                    Debug.LogError("No result or request failed.");
                }
            }
        }
        judgementCallback?.Invoke(textsToSend);
    }

    private IEnumerator SendChatFilterRequest(statementObj textsToSend)
    {
        string urlAppend = "chatfilter/";
        string jsonData = $"{{ \"text\": \"{textsToSend.statement}\"}}";
        

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl + urlAppend, jsonData))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError("POST Request Failed: " + request.error);
            }
            else
            {
                string responseData = request.downloadHandler.text;
                if (!string.IsNullOrEmpty(responseData))
                {
                    ToxicData toxicdata = JsonUtility.FromJson<ToxicData>(responseData);
                    VerdictData verdictdata = JsonUtility.FromJson<VerdictData>(responseData);
                    double judgementNumber = verdictdata.verdict;
                    int toxicOrNot = (int)toxicdata.toxic;
                    double roundedValue = Math.Round(judgementNumber, 2);
                    textsToSend.ratingChatFilter = roundedValue;

                    if(textsToSend.ratingChatFilter > 1)
                    {
                        textsToSend.ratingChatFilter = 1;
                    }
                    textsToSend.boolChatFilter = toxicOrNot;
                }
            }
        }
    }
    public void RefreshAI()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl);
        webRequest.SendWebRequest();
    }

    [System.Serializable]
    public class VerdictData
    {
        public List<double> verdictList;
        public double verdict;
    }

    [System.Serializable]
    public class VerdictArrData
    {
        public double[] verdict;
    }

    [System.Serializable]
    public class ToxicData
    {
        public double toxic;
    }
}
