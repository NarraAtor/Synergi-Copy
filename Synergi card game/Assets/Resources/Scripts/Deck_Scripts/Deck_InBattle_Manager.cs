using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
public class Deck_InBattle_Manager : NetworkBehaviour
{
    
    private Stack<CardData> deckArray = new Stack<CardData>(40);
    //To send cards to when drawing
    [SerializeField] private  GameObject playerHand;
    //To send cards to when milling
    [SerializeField] private  GameObject playerGraveyard;

    [SerializeField] private GameObject cardDatabase;

    //test object
    //public static TacticData LightningBolt;

    //This will be used for instantiating a deck and with special card effects (think scrying (MTG) and "Nab"-ing (LOR))
    public Stack<CardData> DeckArray
    {
        get
        {
            return deckArray;
        }
        set
        {
            deckArray = value;
        }
    }

    //I can't exactly picture how a user will pick their own deck, so I'll just create a prototype-deck with hard coded data.
    // Start is called before the first frame update
    void Start()
    {
        
        MakeDeck();

        //For testing purposes only, eventually I'll be using pre-created SOs from a card database.
        //LightningBolt = TacticData.CreateInstance<TacticData>();
        //LightningBolt.Init(CardColor.Green, 2, "LightningBolt", "Deal 2 damage to an enemy", TacticSubtypes.Instant);

        //DeckArray.Push(LightningBolt);

        Draw();
        Draw();
        Draw();
    }

    //Draw a card from the top of the deck.
    public void Draw()
    {
        playerHand.GetComponent<Hand_Manager>().AddCardToHand(deckArray.Pop(), playerHand);
    }

    //Places all of the cards in the DeckArray Stack.
    //For now, these will be hard coded with 14 beings, 13 deployables, 13 tactics
    private void MakeDeck()
    {
        deckArray = cardDatabase.GetComponent<CardDatabase>().CreateDeck();
    }

    /// <summary>
    /// Purpose: Shuffles the decks, changing the position of all cards in the deck randomly.
    /// </summary>
    private void Shuffle()
    {

    }
}
