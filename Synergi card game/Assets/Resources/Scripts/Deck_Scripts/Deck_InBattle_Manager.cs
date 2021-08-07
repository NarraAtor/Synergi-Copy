using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;
using Mirror;

/// <summary>
/// Purpose: Manages the deck while each player is in battle.
/// Restrictions: None
/// </summary>
public class Deck_InBattle_Manager : NetworkBehaviour
{

    private Stack<CardData> deckArray = new Stack<CardData>(40);
    //To send cards to when drawing
    [SerializeField] private GameObject playerHand;
    //To send cards to when milling
    [SerializeField] private GameObject playerGraveyard;

    [SerializeField] private GameObject cardDatabase;
    //Waits for client to connect before starting.
    [SyncVar]
    private bool gameHasStarted;
    [SyncVar(hook = nameof(DrawStartingHand))]
    private bool playersDrewCards;

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
    public void Start()
    {
        gameHasStarted = false;
        playersDrewCards = false;
        MakeDeck();

        //For testing purposes only, eventually I'll be using pre-created SOs from a card database.
        //LightningBolt = TacticData.CreateInstance<TacticData>();
        //LightningBolt.Init(CardColor.Green, 2, "LightningBolt", "Deal 2 damage to an enemy", TacticSubtypes.Instant);

        //DeckArray.Push(LightningBolt);

    }

    private void Update()
    {
        //Resolves the error where commands are sent to the server before client connects.
        //Also resolves client's null reference error when drawing a card from their own deck.
        if (playersDrewCards)
        {
            return;
        }

        if (isClientOnly)
        {
            if (NetworkClient.ready)
            {
                if (!gameHasStarted)
                {
                    CmdStartGame();
                }
            }
        }
        else if (isServer)
        {
            if (gameHasStarted)
            {
                playersDrewCards = true;
            }
        }

        print(gameHasStarted);
    }

    //Draw a card from the top of the deck.
    public void Draw()
    {

        if(isServer)
        {
            print($"Server called Draw();");
        }
        if(isClientOnly)
        {
            print($"Client called Draw();");
        }
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

    /// <summary>
    /// Purpose: Starts the game after the client has properly connected.
    ///          Helps deal with the weird null reference error occuring in the clientRPC
    ///          AddToCard method in Hand_Manager.
    /// </summary>
    [Command(requiresAuthority = false)]
    private void CmdStartGame()
    {
        gameHasStarted = true;
    }

    /// <summary>
    /// Purpose: Draws each player's starting hand. 
    ///         
    /// </summary>
    private void DrawStartingHand(bool oldValue, bool newValue)
    {
        if(newValue)
        {
            Draw();
        }
    }
}
