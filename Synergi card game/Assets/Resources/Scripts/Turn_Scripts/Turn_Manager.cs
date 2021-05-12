using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Phases CurrentPhase {get; set;}
    public Turn CurrentPlayerTurn { get; set; }
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


    // Start is called before the first frame update
    void Start()
    {
        //Find each phase indicator
        foreach(Transform child in p1PhaseIndicator.GetComponentInChildren<Transform>())
        {
            switch(child.gameObject.name)
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
                        p1deck.Draw();
                        CurrentPhase = Phases.ReadyPhase;
                        DeselectTurnIndicator(p1DrawPhaseIndicator);
                        break;
                    case Phases.ReadyPhase:
                        //Call turn effects
                        SelectTurnIndicator(p1ReadyPhaseIndicator);
                        CurrentPhase = Phases.MainPhase1;
                        DeselectTurnIndicator(p1ReadyPhaseIndicator);
                        break;
                    case Phases.MainPhase1:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p1Main1PhaseIndicator);
                        CurrentPhase = Phases.BattlePhase;
                        DeselectTurnIndicator(p1Main1PhaseIndicator);
                        break;
                    case Phases.BattlePhase:
                        //TODO: Give the user the chance to attack or end this phase. End on time end.
                        SelectTurnIndicator(p1BattlePhaseIndicator);
                        CurrentPhase = Phases.MainPhase2;
                        DeselectTurnIndicator(p1BattlePhaseIndicator);
                        break;
                    case Phases.MainPhase2:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p1Main2PhaseIndicator);
                        CurrentPhase = Phases.EndPhase;
                        DeselectTurnIndicator(p1Main2PhaseIndicator);
                        break;
                    case Phases.EndPhase:
                        SelectTurnIndicator(p1EndPhaseIndicator);
                        CurrentPlayerTurn = Turn.P2;
                        CurrentPhase = Phases.Draw;
                        DeselectTurnIndicator(p1EndPhaseIndicator);
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
                        CurrentPhase = Phases.ReadyPhase;
                        DeselectTurnIndicator(p2DrawPhaseIndicator);
                        break;
                    case Phases.ReadyPhase:
                        //Call turn effects
                        SelectTurnIndicator(p2ReadyPhaseIndicator);
                        CurrentPhase = Phases.MainPhase1;
                        DeselectTurnIndicator(p2ReadyPhaseIndicator);
                        break;
                    case Phases.MainPhase1:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p2Main1PhaseIndicator);
                        CurrentPhase = Phases.BattlePhase;
                        DeselectTurnIndicator(p2Main1PhaseIndicator);
                        break;
                    case Phases.BattlePhase:
                        //TODO: Give the user the chance to attack or end this phase. End on time end.
                        SelectTurnIndicator(p2BattlePhaseIndicator);
                        CurrentPhase = Phases.MainPhase2;
                        DeselectTurnIndicator(p2BattlePhaseIndicator);
                        break;
                    case Phases.MainPhase2:
                        //TODO: End on user input or time end
                        SelectTurnIndicator(p2Main2PhaseIndicator);
                        CurrentPhase = Phases.EndPhase;
                        DeselectTurnIndicator(p2Main2PhaseIndicator);
                        break;
                    case Phases.EndPhase:
                        //SelectTurnIndicator(p2EndPhaseIndicator);
                        //Commented to prevent a stack overflow.
                        //CurrentPlayerTurn = Turn.P1;
                        //CurrentPhase = Phases.Draw;
                        //DeselectTurnIndicator(p2EndPhaseIndicator);
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
}
