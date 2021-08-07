using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;
using Mirror;


/// <summary>
/// Purpose: Manages each player's hand and controls visibility over what each player sees in each hand.
/// Restrictions:
/// TODO: Make sure to add a networkobject if this code doesn't work!
/// </summary>
public class Hand_Manager : NetworkBehaviour
{
    [SerializeField] private CardDatabase cardDatabase;
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
        print(isServer + "\n" + isClientOnly);
        if(isServer)
        {
            CmdAddCardToHandServer(card.CardTitle, hand, true);
        }
        else if(isClientOnly)
        {
            CmdAddCardToHandServer(card.CardTitle, hand, false);
        }
    }

    /// <summary>
    /// Purpose: Adds a card to the selected player's hand and does the same on the client.
    /// Restrictions: Only works properly with "this" or "enemyHand" variables
    ///               Mirror can't send my CardData over the network so I'll just send the card title
    ///               over and search for the card locally
    /// </summary>
    /// <param name="cardTitle">the card to add to the hand</param>
    /// <param name="hand">the hand to add the card too</param>
    /// 
    [Command(requiresAuthority = false)]
    public void CmdAddCardToHandServer(string cardTitle, GameObject hand, bool sentFromServer)
    {
        //Find the scriptable object with the matching name locally.
        CardData card = cardDatabase.FindCard(cardTitle, sentFromServer);

        if(sentFromServer)
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
                }
            }
        }
        else
        {
            if (hand.Equals(enemyHand))
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
                }
            }
            else if (hand.Equals(this.gameObject))
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
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
                    RpcAddCardToHandClient(cardTitle, hand, sentFromServer);
                }
            }
        }
    }

    /// <summary>
    /// Purpose: Sends a message to player 2 (the client) to add a card to their hand.
    /// Restrictions: 
    /// </summary>
    /// <param name="cardTitle">the name of the card to add</param>
    /// <param name="hand">the hand to add the card to</param>
    /// <param name="sentFromServer">whether this was called from the server or the client</param>
    [ClientRpc(includeOwner = false)]
    public void RpcAddCardToHandClient(string cardTitle, GameObject hand, bool sentFromServer)
    {
        //Find the scriptable object with the matching name locally.
        CardData card = cardDatabase.FindCard(cardTitle, sentFromServer);

        if (sentFromServer)
        {
            if (hand.Equals(enemyHand))
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
                    print(addedCard);
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
            else if (hand.Equals(this.gameObject))
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
        else
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




}

