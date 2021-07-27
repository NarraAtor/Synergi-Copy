using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Eric Fotang
/// Purpose: Manages the PASS button that each player has. 
///          I intend to use it the same way LOR and MTG does.
/// Restrictions: Must be attached to a pass button to work appropiately.
///               Other scripts set this object's text. 
///               This script just decides what happens when that oocurs.
/// </summary>
public class PassButton : MonoBehaviour
{
    //This should be kept internal since it's only used for this script's game object.
    public enum PassButtonStates
    {
        PASS,
        ATTACK,
        BLOCK,
        BACK
    }

    [SerializeField] private GameObject gameManager;
    private GameObject passButton;
    [SerializeField] private GameObject playerBattlefield;
    private PassButtonStates currentState;
    // Start is called before the first frame update
    void Start()
    {
        passButton = this.gameObject;
        ChangeButtonState(PassButtonStates.PASS);
    }

    // Update is called once per frame
    void Update()
    {
      
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

        switch(currentState)
        {
            case PassButtonStates.PASS:
                {
                    gameManager.GetComponent<Turn_Manager>().P1ReadyToPass = true;
                }
                break;
            case PassButtonStates.ATTACK:
                {
                    gameManager.GetComponent<Turn_Manager>().CommitAttackers();
                }
                break;
            case PassButtonStates.BLOCK:
                {
                    //gameManager.GetComponent<Turn_Manager>().CommitDefenders();
                }
                break;
            case PassButtonStates.BACK:
                {
                    playerBattlefield.GetComponent<Battlefield_Zone_Manager>().HideDeployableZones();
                }
                break;
        }
    }

    /// <summary>
    /// Purpose: Used to change the text of the button.
    /// <paramref name="desiredState"/>the text the passButton's text should be changed to.</param>
    /// </summary>
    public void ChangeButtonState(PassButtonStates desiredState)
    {
        switch (desiredState)
        {
            case PassButtonStates.PASS:
                currentState = PassButtonStates.PASS;
                passButton.GetComponentInChildren<Text>().text = $"PASS";
                break;
            case PassButtonStates.ATTACK:
                currentState = PassButtonStates.ATTACK;
                passButton.GetComponentInChildren<Text>().text = $"ATTACK";
                break;
            case PassButtonStates.BLOCK:
                currentState = PassButtonStates.BLOCK;
                passButton.GetComponentInChildren<Text>().text = $"BLOCK";
                break;
            case PassButtonStates.BACK:
                currentState = PassButtonStates.BACK;
                passButton.GetComponentInChildren<Text>().text = $"BACK";
                break;
        }
    }
}
