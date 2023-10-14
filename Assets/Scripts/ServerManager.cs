using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
public class ServerManager : MonoBehaviour
{
    private Process powershellProcess;
    public static ServerManager serverInstance { get; private set; }
    private void Awake()
    {
        if (serverInstance == null)
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
        //var ps1File = @"F:\Feivel\Programming\Unity\insultMachine\JudgeServer\server.ps1";

        //var scriptArguments = "-ExecutionPolicy Bypass -File \"" + ps1File + "\"";
        //var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments);
        //processStartInfo.RedirectStandardOutput = true;
        //processStartInfo.RedirectStandardError = true;

        //using var process = new Process();
        //process.StartInfo = processStartInfo;
        //process.Start();
        //string output = process.StandardOutput.ReadToEnd();
        //string error = process.StandardError.ReadToEnd();
        //UnityEngine.Debug.Log(output); // I am invoked using ProcessStartInfoClass!
    }

    private void OnApplicationQuit()
    {
        //// Terminate the PowerShell process when the game is closed
        //if (powershellProcess != null && !powershellProcess.HasExited)
        //{
        //    powershellProcess.Kill();
        //}
    }
}
