using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

/// <summary>
/// Purpose: Prints all messages that are sent to the editor's console.
///          I'm doing this because I can't see the console while in a build.
/// Restrictions: Can only be used on the NetworkLogger Game Object.
/// </summary>
public class NetworkLogger : NetworkBehaviour
{
    public string output = "";
    public string stack = "";
    public TextMeshProUGUI textMeshProUGUI;
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
        CmdUnitedPrint(output, isServer);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            textMeshProUGUI.pageToDisplay++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            textMeshProUGUI.pageToDisplay--;
        }
    }

    /// <summary>
    /// Purpose: Ensures that I can see what is happening between the server and the client rather 
    ///          than having the messages chopped off.
    ///          The command that sends the message to all clients including the host.
    /// Restriction:
    /// </summary>
    /// <param name="output">the string to print</param>
    [Command(requiresAuthority = false)]
    private void CmdUnitedPrint(string output, bool sentFromServer)
    {
        RpcUnitedPrint(output, sentFromServer);
    }

    /// <summary>
    /// Purpose: Ensures that I can see what is happening between the server and the client rather 
    ///          than having the messages chopped off.
    ///          The command that sends the message to all clients including the host.
    /// Restriction:
    /// </summary>
    /// <param name="output">the string to print</param>
    [ClientRpc(includeOwner = true)]
    private void RpcUnitedPrint(string output, bool sentFromServer)
    {
        if (sentFromServer)
        {
            textMeshProUGUI.text += "Server: ";
        }
        else
        {
            textMeshProUGUI.text += "Client: ";
        }
        textMeshProUGUI.text += output + "\n";
    }
}
