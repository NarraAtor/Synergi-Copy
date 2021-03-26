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
    public Turn currentPlayerTurn { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentPhase = Phases.Draw;
        currentPlayerTurn = Turn.P2;
    }

    
}
