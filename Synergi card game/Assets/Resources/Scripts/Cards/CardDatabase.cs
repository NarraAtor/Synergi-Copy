using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;
using Mirror;
//This is a script for managing all of the scriptable object cards I make.
public class CardDatabase : NetworkBehaviour
{
    private BeingData[] listOfBeings;
    private TacticData[] listOfTactics;
    private DeployableData[] listOfDeployables;
    //private List<CardData> listOfCards = new List<CardData>(3);

    public void Start()
    {

    }

    /// <summary>
    /// Purpose: Creates a stack of cards based on what cards the user decided to include in their deck.
    ///          TODO: Allow user to pick which cards go in their deck.
    /// Restrictions: Relies on the editor to have these scripts.
    /// Returns: A stack of CardData objects.
    /// </summary>
    public Stack<CardData> CreateDeck()
    {
        //Intialized here to avoid weird null reference exceptions from using Mirror.
        listOfBeings = Resources.LoadAll<BeingData>("Scripts/Cards/List of Cards/Beings");
        listOfTactics = Resources.LoadAll<TacticData>("Scripts/Cards/List of Cards/Tactics");
        listOfDeployables = Resources.LoadAll<DeployableData>("Scripts/Cards/List of Cards/Deployables");

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

    /// <summary>
    /// Purpose: Finds a card based on its title. 
    ///          Made since CardData can't be sent over the network.
    ///          Consider adding overloads later.
    ///          TODO: Remove sentFromServer bool.
    /// Restrictions: None
    /// </summary>
    /// <param name="cardTitle">The name of the card to find</param>
    /// <returns>the data of the card with the passed in title</returns>
    public CardData FindCard(string cardTitle, bool sentFromServer)
    {
        if(sentFromServer)
        {
            print($"Find Card called: {cardTitle} Server");
        }
        else
        {
            print($"Find Card called: {cardTitle} Client");
        }
        foreach (BeingData beingData in listOfBeings)
        {
            if (cardTitle.Equals(beingData.CardTitle))
            {
                return beingData;
            }
        }
        foreach (TacticData tacticData in listOfTactics)
        {
            if (cardTitle.Equals(tacticData.CardTitle))
            {
                return tacticData;
            }
        }
        foreach (DeployableData deployableData in listOfDeployables)
        {
            if (cardTitle.Equals(deployableData.CardTitle))
            {
                return deployableData;
            }
        }

        print($"unable to find card: {cardTitle}");
        //return null;
        return null;
    }
}
