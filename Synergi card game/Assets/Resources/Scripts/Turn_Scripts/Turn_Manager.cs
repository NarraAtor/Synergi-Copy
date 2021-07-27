using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public enum Turn
{
    Self,
    Other
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
/// Restrictions: The Host will be treated as player 1. 
///               The client will be treated as player 2.
///               The game will assume 2 players are on the network only.
/// </summary>
public class Turn_Manager : NetworkBehaviour
{
    [SyncVar]
    private Phases currentPhase;
    [SyncVar]
    private Turn currentPlayerTurn;
    public Phases CurrentPhase
    {
        get
        {
            return currentPhase;
        }

        set
        {
            currentPhase = value;
        }

    }
    public Turn CurrentPlayerTurn {
        get
        {
            return currentPlayerTurn;
        }

        set
        {
            currentPlayerTurn = value;
        }
    }

    public bool P1ReadyToPass { get; set; }
    public bool NeedsToInvestInACrystal { get; set; }
    public bool AttackersDeclared { get; set; }
    public bool BlockersDeclared { get; set; }
    //TODO: Change Queue into a List.
    public Queue<Being> AttackerQueue { get; set; }

    //public bool P2ReadyToPass { get; set; }
    [SerializeField] private Deck_InBattle_Manager p1deck;
    private GameObject energySelector;
    [SerializeField] private GameObject p1PhaseIndicator;
    private GameObject p1DrawPhaseIndicator;
    private GameObject p1ReadyPhaseIndicator;
    private GameObject p1Main1PhaseIndicator;
    private GameObject p1BattlePhaseIndicator;
    private GameObject p1Main2PhaseIndicator;
    private GameObject p1EndPhaseIndicator;
    private GameObject[] p1IndicatorArray;
    [SerializeField] private GameObject p2PhaseIndicator;
    private GameObject p2DrawPhaseIndicator;
    private GameObject p2ReadyPhaseIndicator;
    private GameObject p2Main1PhaseIndicator;
    private GameObject p2BattlePhaseIndicator;
    private GameObject p2Main2PhaseIndicator;
    private GameObject p2EndPhaseIndicator;
    private GameObject[] p2IndicatorArray;
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
        NeedsToInvestInACrystal = true;

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

        p1IndicatorArray = new GameObject[]{ p1DrawPhaseIndicator,
                                             p1ReadyPhaseIndicator,
                                             p1Main1PhaseIndicator,
                                             p1BattlePhaseIndicator,
                                             p1Main2PhaseIndicator,
                                             p1EndPhaseIndicator};


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

        p2IndicatorArray = new GameObject[]{ p2DrawPhaseIndicator,
                                             p2ReadyPhaseIndicator,
                                             p2Main1PhaseIndicator,
                                             p2BattlePhaseIndicator,
                                             p2Main2PhaseIndicator,
                                             p2EndPhaseIndicator};

        p1PassButton = GameObject.Find("Player Pass Button");

        energySelector = GameObject.Find("Player Crystal Selector");
        energySelector.SetActive(false);


        //CurrentPhase = Phases.Draw;
        CurrentPlayerTurn = Turn.Self;
    }


    void Update()
    {

        //Set Turn based on whether this is a host (P1) or a client only (P2)
        //Resolve indicator glitch
        foreach (GameObject indicator in p1IndicatorArray)
        {
            DeselectTurnIndicator(indicator);
        }
        foreach (GameObject indicator in p2IndicatorArray)
        {
            DeselectTurnIndicator(indicator);
        }

        //print($"{GlobalCurrentPhase.Value}");
        //Phase/Turn State Machine
        switch (CurrentPlayerTurn)
        {

            case Turn.Self:
                switch (CurrentPhase)
                {
                    //TODO: Make each phase broadcast messages to the entire game.
                    //Alternatively, if I do more research into unity events or use C# events and delegates, use that instead. 
                    case Phases.Draw:

                        SelectTurnIndicator(p1DrawPhaseIndicator);

                        //Gets rid of infinite draw glitch
                        if (canDrawCardsDuringDrawPhase)
                        {
                            p1deck.Draw();
                            canDrawCardsDuringDrawPhase = false;
                        }

                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.ReadyPhase;
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.ReadyPhase:
                        //Call turn effects
                        if (NeedsToInvestInACrystal)
                        {
                            energySelector.SetActive(true);
                            P1ReadyToPass = false;
                        }
                        else
                        {
                            energySelector.SetActive(false);
                        }
                        SelectTurnIndicator(p1ReadyPhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.MainPhase1;
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.MainPhase1:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p1Main1PhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.BattlePhase;
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
                        //else if(BlockersDeclared)
                        //{
                        //
                        //}
                        else if (P1ReadyToPass)
                        {
                            AttackerQueue.Clear();
                            CurrentPhase = Phases.MainPhase2;
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.MainPhase2:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p1Main2PhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CurrentPhase = Phases.EndPhase;
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.EndPhase:
                        SelectTurnIndicator(p1EndPhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            switch (CurrentPlayerTurn)
                            {
                                case Turn.Self:
                                    CurrentPlayerTurn = Turn.Other;
                                    break;
                                case Turn.Other:
                                    CurrentPlayerTurn = Turn.Self;
                                    break;
                            }

                            CurrentPhase = Phases.Draw;
                        }
                        P1ReadyToPass = false;
                        canDrawCardsDuringDrawPhase = true;
                        NeedsToInvestInACrystal = true;
                        break;
                }
                break;
            case Turn.Other:
                switch (CurrentPhase)
                {
                    //TODO: Make each phase broadcast messages to the entire game.
                    case Phases.Draw:
                        SelectTurnIndicator(p2DrawPhaseIndicator);
                        //p2deck.Draw();
                        //if (P1ReadyToPass)
                        //{
                        //    GlobalCurrentPhase.Value = Phases.ReadyPhase;
                        //    DeselectTurnIndicator(p2DrawPhaseIndicator);
                        //}
                        P1ReadyToPass = false;

                        break;
                    case Phases.ReadyPhase:
                        //Call turn effects
                        SelectTurnIndicator(p2ReadyPhaseIndicator);
                        //if (P1ReadyToPass)
                        //{
                        //    GlobalCurrentPhase.Value = Phases.MainPhase1;
                        //    DeselectTurnIndicator(p2ReadyPhaseIndicator);
                        //}
                        P1ReadyToPass = false;

                        break;
                    case Phases.MainPhase1:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p2Main1PhaseIndicator);
                        //if (P1ReadyToPass)
                        //{
                        //    GlobalCurrentPhase.Value = Phases.BattlePhase;
                        //    DeselectTurnIndicator(p2Main1PhaseIndicator);
                        //}
                        P1ReadyToPass = false;

                        break;
                    case Phases.BattlePhase:
                        //TODO: Give the user the chance to attack or end this phase. End on time end.
                        SelectTurnIndicator(p2BattlePhaseIndicator);
                        //if (P1ReadyToPass)
                        //{
                        //    GlobalCurrentPhase.Value = Phases.MainPhase2;
                        //    DeselectTurnIndicator(p2BattlePhaseIndicator);
                        //}
                        P1ReadyToPass = false;

                        break;
                    case Phases.MainPhase2:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p2Main2PhaseIndicator);
                        //if (P1ReadyToPass)
                        //{
                        //    GlobalCurrentPhase.Value = Phases.EndPhase;
                        //    DeselectTurnIndicator(p2Main2PhaseIndicator);
                        //}
                        P1ReadyToPass = false;

                        break;
                    case Phases.EndPhase:
                        SelectTurnIndicator(p2EndPhaseIndicator);
                        //if (P1ReadyToPass)
                        //{
                        //    CurrentPlayerTurn = Turn.Self;
                        //    GlobalCurrentPhase.Value = Phases.Draw;
                        //    DeselectTurnIndicator(p2EndPhaseIndicator);
                        //}
                        P1ReadyToPass = false;
                        canDrawCardsDuringDrawPhase = true;
                        NeedsToInvestInACrystal = true;
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

        while (AttackerQueue.Count > 0)
        {
            AttackerQueue.Peek().CommitAttack();
            AttackerQueue.Dequeue().BattleNumber.SetActive(false);
        }
        p1PassButton.GetComponent<PassButton>().ChangeButtonState(PassButton.PassButtonStates.PASS);
        //p1PassButton.GetComponent<PassButton>().ChangeButtonText(PassButton.PassButtonStates.PASS);
        //TODO: attackerDeclared = true;
        //P1ReadyToPass = true;
    }

    /// <summary>
    /// Purpose: Triggers when a player clicks on a being during their battlephase.
    ///          If the clicked being is already in the queue, clear the entire thing.
    ///          Otherwise, add it to the queue.
    /// </summary>
    /// <param name="being">the being being added to the queue</param>
    public void DeclareAttacker(Being being)
    {
        //TODO:Declare attackers

        //ERROR: Clicking cards that are horizontally adjacent from right to left clears the list.
        //       Error does not occur in literally any other scenario: 
        //       - h. adjacent from left to right works.
        //       - v. adjacent in any otder works.
        //       - d. adjacent in any order works.
        //       The game appears to think the player is clicking the same card. I believe this is a result of using Queue.Contains.
        //       I'll try a different bool.
        if (AttackerQueue.Contains(being))
        {
            while (AttackerQueue.Count > 0)
            {
                AttackerQueue.Dequeue().BattleNumber.SetActive(false);
                p1PassButton.GetComponent<PassButton>().ChangeButtonState(PassButton.PassButtonStates.PASS);
            }
        }
        else
        {
            AttackerQueue.Enqueue(being);
            being.BattleNumber.SetActive(true);
            being.BattleNumber.GetComponent<TextMeshProUGUI>().text = $"{AttackerQueue.Count}";
            p1PassButton.GetComponent<PassButton>().ChangeButtonState(PassButton.PassButtonStates.ATTACK);
        }
    }


}
