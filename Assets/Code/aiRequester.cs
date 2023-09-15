using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class aiRequester
{
    private string apiUrl = "http://127.0.0.1:8000/cnnsvm/aiJudge/";
    public delegate void PostRequestCallback(string result);
    private PostRequestCallback postRequestCallback;

    public IEnumerator SendPostRequest(List<string> textData, PostRequestCallback callback)
    {
        postRequestCallback = callback;
        yield return judgeText(textData);
    }

    private IEnumerator judgeText(List<string> textsToSend)
    {
        string compiledArr = "";

        int size = textsToSend.Count;
        int index = 0;

        while (index < size)
        {
            if (size == 1)
            {
                compiledArr += $"\"{textsToSend[index]}\"";
                index++;
            }
            else if(size > 1)
            {
                if (index == size-1)
                {
                    compiledArr += $"\"{textsToSend[index]}\"";
                    index++;

                }
                else
                {
                    compiledArr += $"\"{textsToSend[index]}\",";
                    index++;
                }
            }
        }
        string jsonData = $"{{ \"text\": [{compiledArr}]}}";

        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl, jsonData))
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
                postRequestCallback?.Invoke(responseData);
            }
        }
    }

    public void RefreshAI()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl);
        webRequest.SendWebRequest();
    }
}
