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
    [SyncVar]
    private int turnNumber;
    public Phases CurrentPhase
    {
        get
        {
            return currentPhase;
        }

        private set
        {
            currentPhase = value;
        }

    }
    public Turn CurrentPlayerTurn
    {
        get
        {
            return currentPlayerTurn;
        }

        private set
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
    [SerializeField] private GameObject energySelector;
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
    private bool turnNumberWasDeclared = false;

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

        energySelector.SetActive(false);


        //CurrentPhase = Phases.Draw;
        CurrentPlayerTurn = Turn.Self;
        turnNumber = 1;
        print($"\n--------------- Turn {turnNumber}----------\n");
    }


    void Update()
    {


        //Resolve indicator glitch
        foreach (GameObject indicator in p1IndicatorArray)
        {
            DeselectTurnIndicator(indicator);
        }
        foreach (GameObject indicator in p2IndicatorArray)
        {
            DeselectTurnIndicator(indicator);
        }

        //Set Turn based on whether this is a host (P1) or a client only (P2)
        if (isServer)
        {
            //The host is always right
            switch (currentPlayerTurn)
            {
                case Turn.Self:
                    ProgressGame(currentPlayerTurn, currentPhase);
                    break;
                case Turn.Other:
                    ProgressGame(currentPlayerTurn, currentPhase);
                    break;
            }
        }
        else if (isClientOnly)
        {
            //The client is always the opposite of whatever the host is
            switch (currentPlayerTurn)
            {
                case Turn.Self:
                    ProgressGame(Turn.Other, currentPhase);
                    break;
                case Turn.Other:
                    ProgressGame(Turn.Self, currentPhase);
                    break;
            }
        }
        //print($"{GlobalCurrentPhase.Value}");

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
    ///          TODO: Change combat system so that attackers are declared and commited one at a time.
    ///          TODO: Consider moving this to a new script
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

    /// <summary>
    /// Purpose: A helper method that progresses the player through the phases and turns.
    ///          Is Being called constantly
    /// Restrictions: Limited to 2 players.
    /// </summary>
    /// <param name="turn">Whose turn it is.</param>
    /// <param name="phase">What phase it is.</param>
    private void ProgressGame(Turn turn, Phases phase)
    {
        //Phase/Turn State Machine
        switch (turn)
        {

            case Turn.Self:
                switch (phase)
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
                            CmdProgressPhase();
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
                            CmdProgressPhase();
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.MainPhase1:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p1Main1PhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CmdProgressPhase();
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
                            CmdProgressPhase();
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.MainPhase2:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p1Main2PhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CmdProgressPhase();
                        }
                        P1ReadyToPass = false;

                        break;
                    case Phases.EndPhase:
                        SelectTurnIndicator(p1EndPhaseIndicator);
                        if (P1ReadyToPass)
                        {
                            CmdProgressTurn();
                            CmdProgressPhase();
                        }
                        P1ReadyToPass = false;
                        canDrawCardsDuringDrawPhase = true;
                        NeedsToInvestInACrystal = true;
                        break;
                }
                break;
            case Turn.Other:
                switch (phase)
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
    /// Purpose: Allows other methods to quickly sort through the logic to figure out which turn it is.
    /// Restrictions: Limited to 2 players.
    /// </summary>
    /// <returns> whether or not it is the player's turn</returns>
    public Turn GetCurrentTurnNetbased()
    {
        //if the client is clicking the card it has to do a swap
        if (isClientOnly)
        {
            switch (currentPlayerTurn)
            {
                case Turn.Self:
                    return Turn.Other;
                case Turn.Other:
                    return Turn.Self;
            }
        }

        //otherwise the server is clicking the card and it can do things normally.
        return currentPlayerTurn;

    }

    /// <summary>
    /// Purpose: Allows both clients and host to manipulate the currentTurn SyncVar
    /// Restrictions: Only works for 2 players.
    /// </summary>
    /// <param name="currentPhase"></param>
    [Command(requiresAuthority = false)]
    public void CmdProgressTurn()
    {
        switch (currentPlayerTurn)
        {
            case Turn.Self:
                currentPlayerTurn = Turn.Other;
                break;
            case Turn.Other:
                currentPlayerTurn = Turn.Self;
                break;
        }

        turnNumber++;
    }
    /// <summary>
    /// Purpose: Allows both clients and host to manipulate the currentPhase SyncVar
    /// Restrictions: Only works for 2 players.
    /// </summary>
    /// <param name="currentPhase"></param>
    [Command(requiresAuthority = false)]
    public void CmdProgressPhase()
    {
        switch (currentPhase)
        {
            case Phases.Draw:
                currentPhase = Phases.ReadyPhase;
                break;
            case Phases.ReadyPhase:
                currentPhase = Phases.MainPhase1;
                break;
            case Phases.MainPhase1:
                currentPhase = Phases.BattlePhase;
                break;
            case Phases.BattlePhase:
                currentPhase = Phases.MainPhase2;
                break;
            case Phases.MainPhase2:
                currentPhase = Phases.EndPhase;
                break;
            case Phases.EndPhase:
                currentPhase = Phases.Draw;
                if (!turnNumberWasDeclared)
                {
                    print($"\n--------------- Turn {turnNumber}----------\n");
                    turnNumberWasDeclared = true;
                }
                break;
        }
    }


}
