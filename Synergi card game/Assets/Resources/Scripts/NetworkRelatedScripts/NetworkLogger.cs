using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Purpose: Prints all messages that are sent to the editor's console.
///          I'm doing this because I can't see the console while in a build.
/// Restrictions: Can only be used on the NetworkLogger Game Object.
/// </summary>
public class NetworkLogger : MonoBehaviour
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
        this.gameObject.GetComponent<TextMeshProUGUI>().text = output;
    }
}