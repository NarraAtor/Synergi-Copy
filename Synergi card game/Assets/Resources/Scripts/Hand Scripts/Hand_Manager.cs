using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

/// <summary>
/// Purpose: Manages each player's hand and controls visiibility over what each player sees in each hand.
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

    //The host's hand on the network
    public NetworkVariable<List<GameObject>> Player1Hand = new NetworkVariable<List<GameObject>>(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    //The client's hand on the network.
    public NetworkVariable<List<GameObject>> Player2Hand = new NetworkVariable<List<GameObject>>(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });


    public List<GameObject> CardsInPlayer_Hand
    {
        get
        {
            return cardsInPlayer_Hand;
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
        if(IsHost)
        {
            Player1Hand.Value = cardsInPlayer_Hand;
            Player2Hand.Value = cardsInEnemy_Hand;

        }
        else if(IsClient)
        {
            Player2Hand.Value = cardsInPlayer_Hand;
            Player1Hand.Value = cardsInEnemy_Hand;
        }

        string listOfCardsInHand = "";
        foreach(GameObject card in Player1Hand.Value)
        {
            listOfCardsInHand += card + " ,";
        }
        print($"{listOfCardsInHand}");
    }

    /// <summary>
    /// Purpose: Adds a card to the hand based on the type of CardData it is.
    /// Restrictions: Only works properly with "this" or "enemyHand" variables
    /// </summary>
    /// <param name="card">the card to add to the hand</param>
    public void AddCardToHand(CardData card, GameObject hand)
    {
        if(hand.Equals(this.gameObject))
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
        else if(hand.Equals(enemyHand))
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
    ///          I didn't destroy the card here becuase I figured that if there were 2 copies 
    ///          of the same card in hand, glitches would occur.
    ///          This method simply gets rid of destroyed cards in the list.
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
