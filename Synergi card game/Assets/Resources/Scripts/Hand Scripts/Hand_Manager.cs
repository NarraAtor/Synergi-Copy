using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using MLAPI.NetworkVariable.Collections;

/// <summary>
/// Purpose: Manages each player's hand and controls visibility over what each player sees in each hand.
/// Restrictions:
/// TODO: Make sure to add a networkobject if this code doesn't work!
/// </summary>
public class Hand_Manager : NetworkBehaviour
{
    //This list is just to have some way to grab the data from each child in the Player's hand.
    private RectTransform[] childrenOfPlayer_Hand;
    //The list of game objects in the Player's hand.
    private List<GameObject> cardsInPlayer_Hand;

    //This list is just to have some way to grab the data from each child in the Enemy's hand.
    private RectTransform[] childrenOfEnemy_Hand;
    //The list of game objects in the Enemy's hand.
    private List<GameObject> cardsInEnemy_Hand;

    [SerializeField] private GameObject beingPrefab;
    [SerializeField] private GameObject deployablePrefab;
    [SerializeField] private GameObject tacticPrefab;
    //The opponent's hand locally.
    [SerializeField] private GameObject enemyHand;
    private bool networkIsConnected;

    public List<GameObject> CardsInPlayer_Hand
    {
        get
        {
            return cardsInPlayer_Hand;
        }
    }
    public List<GameObject> CardsInEnemy_Hand
    {
        get
        {
            return cardsInEnemy_Hand;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Adds all the cards in the hand at the start of the game to the HandManager's List.
        childrenOfPlayer_Hand = this.GetComponentsInChildren<RectTransform>();
        cardsInPlayer_Hand = new List<GameObject>();
        foreach (RectTransform child in childrenOfPlayer_Hand)
        {
            if (child.gameObject.GetComponent<Card>() != null)
            {
                cardsInPlayer_Hand.Add(child.gameObject);
            }
        }

        childrenOfEnemy_Hand = enemyHand.GetComponentsInChildren<RectTransform>();
        cardsInEnemy_Hand = new List<GameObject>();
        foreach (RectTransform child in childrenOfEnemy_Hand)
        {
            if (child.gameObject.GetComponent<Card>() != null)
            {
                cardsInPlayer_Hand.Add(child.gameObject);
            }
        }
        networkIsConnected = false;
    }

    public override void NetworkStart()
    {
        base.NetworkStart();
        networkIsConnected = true;
    }

    void Update()
    {



        //string listOfCardsInHand = "";
        //foreach(GameObject card in Player1Hand.Value)
        //{
        //    listOfCardsInHand += card + " ,";
        //}
        //print($"{listOfCardsInHand}");
    }

    /// <summary>
    /// Purpose: Sends a request to the server to add a card to a hand
    /// Restrictions: Only works properly with "this" or "enemyHand" variables
    /// </summary>
    /// <param name="card">the card to add to the hand</param>
    /// <param name="hand">the hand to add the card too</param>
    public void AddCardToHand(CardData card, GameObject hand)
    {
        if(IsHost)
        {
            GlobalAddCardToHandServerRPC(card, hand, true);
        }
        else if(IsClient)
        {
            GlobalAddCardToHandServerRPC(card, hand, false);
        }
    }

    /// <summary>
    /// Purpose: Actually adds a card to the hand based on the type of CardData it is.
    ///          The main reason I overloaded this method is because I wanted to find a straightforward
    ///          way to differentiate the server call from the actual method without too much code.
    ///          This whole thing is a workaround since I have no idea how creating my own NetworkList is supposed
    ///          to work and the default NetworkList has weird internal errors with GameObjects.
    ///          (Maybe it's just broken?)
    /// Restrictions: Only works properly with "this" or "enemyHand" variables
    /// </summary>
    /// <param name="card">the card to add to the hand</param>
    /// <param name="hand">the hand to add the card too</param>
    /// <param name="sentFromPlayer1">whether or not this card was sent from the host; 
    ///                               only changes the signature here</param>
    public void AddCardToHand(CardData card, GameObject hand, bool sentFromPlayer1)
    {
        if (hand.Equals(this.gameObject))
        {
            if (card is BeingData)
            {
                BeingData beingData = (BeingData)card;
                //addedCard.CardColor = beingData.CardColorProperty;
                GameObject addedCard = Instantiate(beingPrefab, this.gameObject.transform);
                addedCard.GetComponent<Being>().CurrentPosition = CardPositions.Hand;
                addedCard.GetComponent<Being>().Init(beingData.CardColorProperty, beingData.OriginalMaxHealth,
                beingData.OriginalPower, beingData.Species, beingData.AbilityText,
                beingData.RedEnergyCost, beingData.BlueEnergyCost, beingData.GreenEnergyCost, beingData.PurpleEnergyCost, beingData.GenericEnergyCost,
                beingData.CardTitle);
                cardsInPlayer_Hand.Add(addedCard.gameObject);

            }
            if (card is TacticData)
            {
                TacticData tacticData = (TacticData)card;
                GameObject addedCard = Instantiate(tacticPrefab, this.gameObject.transform);
                addedCard.GetComponent<Tactic>().CurrentPosition = CardPositions.Hand;
                addedCard.GetComponent<Tactic>().Init(tacticData.CardColorProperty,
                                                      tacticData.RedEnergyCost,
                                                      tacticData.BlueEnergyCost,
                                                      tacticData.GreenEnergyCost,
                                                      tacticData.PurpleEnergyCost,
                                                      tacticData.GenericEnergyCost,
                                                      tacticData.CardTitle,
                                                      tacticData.AbilityText,
                                                      tacticData.Subtype);
                cardsInPlayer_Hand.Add(addedCard.gameObject);
            }

            if (card is DeployableData)
            {
                DeployableData deployableData = (DeployableData)card;
                GameObject addedCard = Instantiate(deployablePrefab, this.gameObject.transform);
                addedCard.GetComponent<Deployable>().CurrentPosition = CardPositions.Hand;
                addedCard.GetComponent<Deployable>().Init(deployableData.CardColorProperty,
                                                          deployableData.RedEnergyCost,
                                                          deployableData.BlueEnergyCost,
                                                          deployableData.GreenEnergyCost,
                                                          deployableData.PurpleEnergyCost,
                                                          deployableData.GenericEnergyCost,
                                                          deployableData.CardTitle,
                                                          deployableData.AbilityText,
                                                          deployableData.Durability,
                                                          deployableData.Subtype);
                cardsInPlayer_Hand.Add(addedCard.gameObject);
            }
        }
        else if (hand.Equals(enemyHand))
        {
            if (card is BeingData)
            {
                BeingData beingData = (BeingData)card;
                //addedCard.CardColor = beingData.CardColorProperty;
                GameObject addedCard = Instantiate(beingPrefab, enemyHand.transform);
                addedCard.GetComponent<Being>().CurrentPosition = CardPositions.Hand;
                addedCard.GetComponent<Being>().Init(beingData.CardColorProperty, beingData.OriginalMaxHealth,
                beingData.OriginalPower, beingData.Species, beingData.AbilityText,
                beingData.RedEnergyCost, beingData.BlueEnergyCost, beingData.GreenEnergyCost, beingData.PurpleEnergyCost, beingData.GenericEnergyCost,
                beingData.CardTitle);
                cardsInEnemy_Hand.Add(addedCard.gameObject);
            }
            if (card is TacticData)
            {
                TacticData tacticData = (TacticData)card;
                GameObject addedCard = Instantiate(tacticPrefab, enemyHand.transform);
                addedCard.GetComponent<Tactic>().CurrentPosition = CardPositions.Hand;
                addedCard.GetComponent<Tactic>().Init(tacticData.CardColorProperty,
                                                      tacticData.RedEnergyCost,
                                                      tacticData.BlueEnergyCost,
                                                      tacticData.GreenEnergyCost,
                                                      tacticData.PurpleEnergyCost,
                                                      tacticData.GenericEnergyCost,
                                                      tacticData.CardTitle,
                                                      tacticData.AbilityText,
                                                      tacticData.Subtype);
                cardsInEnemy_Hand.Add(addedCard.gameObject);
            }

            if (card is DeployableData)
            {
                DeployableData deployableData = (DeployableData)card;
                GameObject addedCard = Instantiate(deployablePrefab, enemyHand.transform);
                addedCard.GetComponent<Deployable>().CurrentPosition = CardPositions.Hand;
                addedCard.GetComponent<Deployable>().Init(deployableData.CardColorProperty,
                                                          deployableData.RedEnergyCost,
                                                          deployableData.BlueEnergyCost,
                                                          deployableData.GreenEnergyCost,
                                                          deployableData.PurpleEnergyCost,
                                                          deployableData.GenericEnergyCost,
                                                          deployableData.CardTitle,
                                                          deployableData.AbilityText,
                                                          deployableData.Durability,
                                                          deployableData.Subtype);
                cardsInEnemy_Hand.Add(addedCard.gameObject);
            }
        }
    }

    /// <summary>
    /// Purpose: Removes a card from the CardInPlayer_Hand list (such as when it is deployed).
    /// </summary>
    public void GetRidOfDestroyedCards(Card card)
    {
        card.gameObject.SetActive(false);
        int flag = -1;
        bool foundCard = false;
        for (int i = 0; i < cardsInPlayer_Hand.Count; i++)
        {
            if (cardsInPlayer_Hand[i].activeSelf == false)
            {
                flag = i;
                foundCard = true;
            }
        }

        if (foundCard)
        {
            cardsInPlayer_Hand.RemoveAt(flag);
        }

        Destroy(card.gameObject);
    }

    /// <summary>
    /// Purpose: Tells all clients to add a card to a hand.
    /// Restricitons: hand must be this or enemyHand.
    /// </summary>
    /// <param name="card">the card to add</param>
    /// <param name="hand">the hand to add the card too</param>
    /// <param name="sentFromPlayer1">whether or not this card was sent from the host</param>
    [ServerRpc]
    private void GlobalAddCardToHandServerRPC(CardData card, GameObject hand, bool sentFromPlayer1)
    {
        GlobalAddCardToHandClientRpc(card, hand, sentFromPlayer1);
    }

    /// <summary>
    /// Purpose: Tells each client add a card to their hand.
    /// Restrictions: hand must be this or enemyHand.
    /// </summary>
    /// <param name="card"></param>
    /// <param name="hand"></param>
    /// <param name="sentFromPlayer1">whether or not this card was sent from the host</param>
    [ClientRpc]
    private void GlobalAddCardToHandClientRpc(CardData card, GameObject hand, bool sentFromPlayer1)
    {
        if(IsHost)
        {
            if(sentFromPlayer1)
            {
                AddCardToHand(card, hand, sentFromPlayer1);
            }
            else
            {
                //if this wasn't sent from the host, swap hands accordingly.
                if(hand.Equals(this.gameObject))
                {
                    AddCardToHand(card, enemyHand, sentFromPlayer1);
                }
                else if(hand.Equals(enemyHand))
                {
                    AddCardToHand(card, this.gameObject, sentFromPlayer1);
                }
            }
        }
        else if(IsClient)
        {
            if (sentFromPlayer1)
            {
                //if this was sent from the host, swap hands accordingly.
                if (hand.Equals(this.gameObject))
                {
                    AddCardToHand(card, enemyHand, sentFromPlayer1);
                }
                else if (hand.Equals(enemyHand))
                {
                    AddCardToHand(card, this.gameObject, sentFromPlayer1);
                }
            }
            else
            {
                AddCardToHand(card, hand, sentFromPlayer1);
            }
        }
    }
}
