using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Eric Fotang
/// Purpose: Manages the PASS button that each player has. 
///          I intend to use it the same way LOR and MTG does
/// Restrictions: Must be attached to a pass button to work appropiately.
/// </summary>
public class PassButton : MonoBehaviour
{
    private GameObject gameManager;
    private GameObject passButton;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        passButton = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Make text change if and only if beings are in the process of declaring attacks/blocks.
        //TODO: Make text change depending if the player is declaring attackers or blockers.
        if(gameManager.GetComponent<Turn_Manager>().CurrentPhase == Phases.BattlePhase)
        {
            //Test
            passButton.GetComponentInChildren<Text>().text = $"ATTACK";
        }
        else
        {
            passButton.GetComponentInChildren<Text>().text = $"PASS";

        }
    }

    /// <summary>
    /// Purpose: Normally passes the turn/phase.
    ///          If there are beings in the process of being declared attackers/defenders, change text accordingly.
    /// Restrictions:
    /// TODO: Change when declaring attackers.
    /// TODO: Change when declaring defenders.
    /// </summary>
    public void IsClicked()
    {
        if(passButton.GetComponentInChildren<Text>().text == $"PASS")
        {
            gameManager.GetComponent<Turn_Manager>().P1ReadyToPass = true;
        }
        else if(passButton.GetComponentInChildren<Text>().text == $"ATTACK")
        {
            gameManager.GetComponent<Turn_Manager>().CommitAttackers();
        }
        else if (passButton.GetComponentInChildren<Text>().text == $"BLOCK")
        {
            gameManager.GetComponent<Turn_Manager>().CommitAttackers();
        }
    }
}
