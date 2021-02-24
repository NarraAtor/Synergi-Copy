using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;

public class Hand_Manager : MonoBehaviour
{
    //This list is just to have some way to grab the data from each child in the Player's hand.
    private RectTransform[] ChildrenOfPlayer_Hand;
    //The list of game objects in the Player's hand.
    private List<GameObject> CardsInPlayer_Hand;
    [SerializeField] private GameObject BeingPrefab;
    [SerializeField] private GameObject DeployablePrefab;
    [SerializeField] private GameObject TacticPrefab;


    public List<GameObject> CardsInPlayer_HandProperty
    {
        get
        {
            return CardsInPlayer_Hand;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ChildrenOfPlayer_Hand = this.GetComponentsInChildren<RectTransform>();
        CardsInPlayer_Hand = new List<GameObject>();
        foreach(RectTransform child in ChildrenOfPlayer_Hand)
        {
            if(child.tag == "Hand")
            {
                CardsInPlayer_Hand.Add(child.gameObject);
            }
        }
    }

    /// <summary>
    /// Purpose: Adds a card to the hand based on the type of CardData it is.
    /// </summary>
    /// <param name="card"></param>
    public void AddCardToHand(CardData card)
    {
        if(card is BeingData)
        {
            BeingData beingData = (BeingData)card;
            //addedCard.CardColor = beingData.CardColorProperty;
            GameObject addedCard = Instantiate(BeingPrefab, this.gameObject.transform);
            addedCard.tag = "Hand";
            addedCard.GetComponent<Being>().Init(beingData.CardColorProperty, beingData.OriginalMaxHealth, 
            beingData.OriginalPower,beingData.Species, beingData.AbilityText, beingData.EnergyCost, beingData.CardTitle);
            CardsInPlayer_Hand.Add(addedCard.gameObject);
        }
         if(card is TacticData)
        {
            TacticData tacticData = (TacticData)card;
            GameObject addedCard = Instantiate(TacticPrefab, this.gameObject.transform);
            addedCard.tag = "Hand";
            addedCard.GetComponent<Tactic>().Init(tacticData.CardColorProperty, tacticData.EnergyCost, 
            tacticData.CardTitle, tacticData.AbilityText, tacticData.Subtype);
            CardsInPlayer_Hand.Add(addedCard.gameObject);
        }
        
        if(card is DeployableData)
        {
            DeployableData deployableData = (DeployableData)card;
            GameObject addedCard = Instantiate(DeployablePrefab, this.gameObject.transform);
            addedCard.tag = "Hand";
            addedCard.GetComponent<Deployable>().Init(deployableData.CardColorProperty, deployableData.EnergyCost, 
            deployableData.CardTitle, deployableData.AbilityText, deployableData.Durability);
            CardsInPlayer_Hand.Add(addedCard.gameObject);
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
        for(int i = 0; i < CardsInPlayer_Hand.Count; i++)
        {
            if(CardsInPlayer_Hand[i].activeSelf == false)
            {
                flag = i;
                foundCard = true;
            }
        }

        if(foundCard)
        {
            CardsInPlayer_Hand.RemoveAt(flag);
        }

        Destroy(card.gameObject);
    }
}
