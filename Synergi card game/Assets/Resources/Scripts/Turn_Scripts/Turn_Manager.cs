﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Turn
{
    P1,
    P2
}
public enum Phases
{
    Draw,
    ReadyPhase,
    MainPhase1,
    BattlePhase,
    MainPhase2,
    EndPhase
}
/// <summary>
/// Purpose: Handles the player phases and turns.
/// </summary>
public class Turn_Manager : MonoBehaviour
{
    public Phases CurrentPhase { get; set; }
    public Turn CurrentPlayerTurn { get; set; }
    public bool P1ReadyToPass { get; set; }
    public bool AttackersDeclared { get; set; }
    public bool BlockersDeclared { get; set; }
    public Queue<Being> AttackerQueue { get; set; }

    //public bool P2ReadyToPass { get; set; }
    [SerializeField] private Deck_InBattle_Manager p1deck;
    [SerializeField] private GameObject p1PhaseIndicator;
    private GameObject p1DrawPhaseIndicator;
    private GameObject p1ReadyPhaseIndicator;
    private GameObject p1Main1PhaseIndicator;
    private GameObject p1BattlePhaseIndicator;
    private GameObject p1Main2PhaseIndicator;
    private GameObject p1EndPhaseIndicator;
    [SerializeField] private GameObject p2PhaseIndicator;
    private GameObject p2DrawPhaseIndicator;
    private GameObject p2ReadyPhaseIndicator;
    private GameObject p2Main1PhaseIndicator;
    private GameObject p2BattlePhaseIndicator;
    private GameObject p2Main2PhaseIndicator;
    private GameObject p2EndPhaseIndicator;
    //[SerializeField] private Deck_InBattle_Manager p2deck;
    //[SerializeField] private GameObject p2PhaseIndicator;
    [SerializeField] private GameObject p1PassButton;
    //[SerializeField] private GameObject p2PassButton;
    private bool canDrawCardsDuringDrawPhase;


    // Start is called before the first frame update
    void Start()
    {
        //Setting the phase passer to false means the phase only changes if and only if the player 
        P1ReadyToPass = false;

        canDrawCardsDuringDrawPhase = false;
        AttackerQueue = new Queue<Being>();

        //Find each phase indicator
        foreach (Transform child in p1PhaseIndicator.GetComponentInChildren<Transform>())
        {
            switch (child.gameObject.name)
            {
                case "Draw Phase Indicator":
                    p1DrawPhaseIndicator = child.gameObject;
                    break;
                case "Ready Phase Indicator":
                    p1ReadyPhaseIndicator = child.gameObject;
                    break;
                case "Main 1 Phase Indicator":
                    p1Main1PhaseIndicator = child.gameObject;
                    break;
                case "Battle Phase Indicator":
                    p1BattlePhaseIndicator = child.gameObject;
                    break;
                case "Main 2 Phase Indicator":
                    p1Main2PhaseIndicator = child.gameObject;
                    break;
                case "End Phase Indicator":
                    p1EndPhaseIndicator = child.gameObject;
                    break;
            }
        }

        foreach (Transform child in p2PhaseIndicator.GetComponentInChildren<Transform>())
        {
            switch (child.gameObject.name)
            {
                case "Draw Phase Indicator":
                    p2DrawPhaseIndicator = child.gameObject;
                    break;
                case "Ready Phase Indicator":
                    p2ReadyPhaseIndicator = child.gameObject;
                    break;
                case "Main 1 Phase Indicator":
                    p2Main1PhaseIndicator = child.gameObject;
                    break;
                case "Battle Phase Indicator":
                    p2BattlePhaseIndicator = child.gameObject;
                    break;
                case "Main 2 Phase Indicator":
                    p2Main2PhaseIndicator = child.gameObject;
                    break;
                case "End Phase Indicator":
                    p2EndPhaseIndicator = child.gameObject;
                    break;
            }
        }

        //Find the pass buttons.
        p1PassButton = GameObject.Find("Player Pass Button");

        CurrentPhase = Phases.Draw;
        CurrentPlayerTurn = Turn.P1;
    }

    void Update()
    {
        //Phase/Turn State Machine
        switch (CurrentPlayerTurn)
        {
            case Turn.P1:
                switch (CurrentPhase)
                {
                    //TODO: Make each phase broadcast messages to the entire game.
                    case Phases.Draw:
                        
                        SelectTurnIndicator(p1DrawPhaseIndicator);

                        //Gets rid of infinite draw glitch
                        if(canDrawCardsDuringDrawPhase)
                        {
                            p1deck.Draw();
                            canDrawCardsDuringDrawPhase = false;
                        }

                        if (P1ReadyToPass)
                        {

                            CurrentPhase = Phases.ReadyPhase;
                            DeselectTurnIndicator(p1DrawPhaseIndicator);
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.ReadyPhase:
                        //Call turn effects
                        SelectTurnIndicator(p1ReadyPhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.MainPhase1;
                            DeselectTurnIndicator(p1ReadyPhaseIndicator);
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.MainPhase1:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p1Main1PhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.BattlePhase;
                            DeselectTurnIndicator(p1Main1PhaseIndicator);
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.BattlePhase:
                        //TODO: Give the user the chance to attack or end this phase. End on time end.
                        SelectTurnIndicator(p1BattlePhaseIndicator);
                        AttackersDeclared = false;
                        BlockersDeclared = false;

                        //Test
                        if (AttackersDeclared)
                        {

                        }
                        else if(BlockersDeclared)
                        {

                        }
                        else if (P1ReadyToPass)
                        {
                            AttackerQueue.Clear();
                            CurrentPhase = Phases.MainPhase2;
                            DeselectTurnIndicator(p1BattlePhaseIndicator);
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.MainPhase2:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p1Main2PhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.EndPhase;
                            DeselectTurnIndicator(p1Main2PhaseIndicator);
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.EndPhase:
                        SelectTurnIndicator(p1EndPhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPlayerTurn = Turn.P2;
                            CurrentPhase = Phases.Draw;
                            DeselectTurnIndicator(p1EndPhaseIndicator);
                        }
                        P1ReadyToPass = false;
                        canDrawCardsDuringDrawPhase = true;
                        break;
                }
                break;
            case Turn.P2:
                switch (CurrentPhase)
                {
                    //TODO: Make each phase broadcast messages to the entire game.
                    case Phases.Draw:
                        SelectTurnIndicator(p2DrawPhaseIndicator);
                        //p2deck.Draw();
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.ReadyPhase;
                            DeselectTurnIndicator(p2DrawPhaseIndicator);
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.ReadyPhase:
                        //Call turn effects
                        SelectTurnIndicator(p2ReadyPhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.MainPhase1;
                            DeselectTurnIndicator(p2ReadyPhaseIndicator);
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.MainPhase1:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p2Main1PhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.BattlePhase;
                            DeselectTurnIndicator(p2Main1PhaseIndicator);
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.BattlePhase:
                        //TODO: Give the user the chance to attack or end this phase. End on time end.
                        SelectTurnIndicator(p2BattlePhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.MainPhase2;
                            DeselectTurnIndicator(p2BattlePhaseIndicator);
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.MainPhase2:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p2Main2PhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.EndPhase;
                            DeselectTurnIndicator(p2Main2PhaseIndicator);
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.EndPhase:
                        SelectTurnIndicator(p2EndPhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPlayerTurn = Turn.P1;
                            CurrentPhase = Phases.Draw;
                            DeselectTurnIndicator(p2EndPhaseIndicator);
                        }
                        P1ReadyToPass = false;
                        canDrawCardsDuringDrawPhase = true;
                        break;
                }
                break;
        }

    }

    /// <summary>
    /// Purpose: Helper Method: Highlights a turn indicator.
    /// Restrictions: Only meant to work with the turn indicator variables.
    /// </summary>
    /// <param name="gameObject">the turn indicator to highlight</param>
    private void SelectTurnIndicator(GameObject gameObject)
    {
        //print($"{gameObject.name}, {CurrentPlayerTurn}");
        gameObject.GetComponentInChildren<Image>().color = Color.white;
        gameObject.GetComponentInChildren<Text>().color = Color.black;
    }

    /// <summary>
    /// Purpose: Helper Method: UnHighlights a turn indicator.
    /// Restrictions: Only meant to work with the turn indicator variables.
    /// </summary>
    /// <param name="gameObject">the turn indicator to highlight</param>
    private void DeselectTurnIndicator(GameObject gameObject)
    {
        //print($"{gameObject.name}, {CurrentPlayerTurn} Deselected");
        gameObject.GetComponentInChildren<Image>().color = Color.black;
        gameObject.GetComponentInChildren<Text>().color = Color.white;
    }

    /// <summary>
    /// Purpose: Confirms that all currently selected attackers are attacking.
    ///          This will call each card's attack method in order of selection.
    /// </summary>
    public void CommitAttackers()
    {
        //TEST
        print($"Attackers Confirmed!");
        //while(AttackerQueue.Count > 0)
        //{
        //    //attackerQueue.Dequeue().CommitAttack();
        //}
    }

    /// <summary>
    /// Purpose: Triggers when a player clicks on a being during their battlephase.
    ///          If the clicked being is already in the queue, clear the entire thing.
    ///          Otherwise, add it to the queue.
    /// </summary>
    /// <param name="being"></param>
    public void DeclareAttacker(Being being)
    {
        //TODO:Declare attackers
        
        if (AttackerQueue.Contains(being))
        {
            while(AttackerQueue.Count > 0)
            {
                AttackerQueue.Dequeue().BattleNumber.SetActive(false);
            }
        }
        else
        {
            AttackerQueue.Enqueue(being);
            being.BattleNumber.SetActive(true);
            being.BattleNumber.GetComponent<TextMeshProUGUI>().text = $"{AttackerQueue.Count}";
        }
    }


}
