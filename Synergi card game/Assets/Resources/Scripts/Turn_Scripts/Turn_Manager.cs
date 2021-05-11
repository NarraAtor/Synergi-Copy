using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //[SerializeField] private Deck_InBattle_Manager p2deck;
    //[SerializeField] private GameObject p2PhaseIndicator;


    // Start is called before the first frame update
    void Start()
    {
        CurrentPhase = Phases.Draw;
        CurrentPlayerTurn = Turn.P1;
    }

    void Update()
    {
        //Phase State Machine
        switch (CurrentPhase)
        {
            case Phases.Draw:
                //p1PhaseIndicator.
                p1deck.Draw();
                CurrentPhase = Phases.ReadyPhase;
                break;
            case Phases.ReadyPhase:
                //Call turn effects
                CurrentPhase = Phases.MainPhase1;
                break;
            case Phases.MainPhase1:
                //TODO: End on user input or time end
                CurrentPhase = Phases.BattlePhase;
                break;
            case Phases.BattlePhase:
                //TODO: Give the user the chance to attack or end this phase. End on time end.
                CurrentPhase = Phases.MainPhase2;
                break;
            case Phases.MainPhase2:
                //TODO: End on user input or time end
                CurrentPhase = Phases.EndPhase;
                break;
            case Phases.EndPhase:
                switch (CurrentPlayerTurn)
                {
                    case Turn.P1:
                        //TODO: CurrentPlayerTurn = Turn.P2
                        break;
                    case Turn.P2:
                        CurrentPlayerTurn = Turn.P1;
                        break;
                }
                break;
        }
    }


}
