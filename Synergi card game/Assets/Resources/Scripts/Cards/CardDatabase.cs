using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;
//This is a script for managing all of the scriptable object cards I make.
public class CardDatabase : MonoBehaviour
{
    private BeingData[] listOfBeings;
    private TacticData[] listOfTactics;
    private DeployableData[] listOfDeployables;
    //private List<CardData> listOfCards = new List<CardData>(3);

    private void Start()
    {
        listOfBeings = Resources.LoadAll<BeingData>("Scripts/Cards/List of Cards/Beings");
        listOfTactics = Resources.LoadAll<TacticData>("Scripts/Cards/List of Cards/Tactics");
        listOfDeployables = Resources.LoadAll<DeployableData>("Scripts/Cards/List of Cards/Deployables");
        
    }

    /// <summary>
    /// Purpose: Creates a stack of cards based on what cards the user decided to include in their deck.
    ///          TODO: Allow user to pick which cards go in their deck.
    /// Restrictions: Relies on the editor to have these scripts.
    /// Returns: A stack of CardData objects.
    /// </summary>
    public Stack<CardData> CreateDeck()
    {
        Stack<CardData> deck = new Stack<CardData>();
        for(int i = 0; i < 13; i++)
        {
            deck.Push(listOfBeings[0]);
            deck.Push(listOfTactics[0]);
            deck.Push(listOfDeployables[0]);
        }

        deck.Push(listOfBeings[0]);

        return deck;
    }
}
