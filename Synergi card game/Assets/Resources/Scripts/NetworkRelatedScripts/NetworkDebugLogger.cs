using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Author: Eric Fotang
/// Purpose: Serves as a debug log since I can't see what's going on in Unity's debug log while playing.
/// </summary>
public class NetworkDebugLogger : MonoBehaviour
{

    public string output = "";
    public string stack = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
    }

    private void Update()
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = $"{output}";
    }
}
