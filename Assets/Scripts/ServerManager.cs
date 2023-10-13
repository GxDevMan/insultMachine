using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class ServerManager : MonoBehaviour
{
    public static ServerManager serverInstance { get; private set; }

    private void Awake()
    {
        if(serverInstance == null)
        {
            serverInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Process.Start("aiInferencing\\Scripts\\Activate");
        Process.Start("cd cnnsvmServer && python manage.py runserver");
    }

    void OnApplicationQuit()
    {
        Process.Start("cd cnnsvmServer && python manage.py runserver --shutdown");
    }
}
