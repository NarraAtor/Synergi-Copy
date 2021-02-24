 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardZone : MonoBehaviour
{
    //If a card zone is unoccupied, it can have its data set and is set to active when a being or deployable is deployed in to it.
    protected bool isOccupied;
    protected bool isClickable;

    private GameObject Player_Battlefield;
    private GameObject Player_Hand;
    private List<GameObject> CardUI;
    private GameObject CardBorder;
    private GameObject CardArt;
    private GameObject CardTitleTextbox;
    private GameObject CardEffectTextbox;
    private GameObject m_CardTitle;
    private GameObject CardTypeAndSubtypes;
    private GameObject CardAbility;
    private GameObject FlavorText;
    private GameObject CardEnergyCost;

    private GameObject CardBack;
    private GameObject BeingStatsBorder;
    private GameObject BeingStatsTextbox;
    private GameObject BeingStatsText;
    private GameObject Placement_Text_B;
    private GameObject DeployableDurabilityBorder;
    private GameObject DeployableDurabilityTextbox;
    private GameObject DeployableDurabilityText;
    private GameObject Placement_Text_D;
    private Being BeingScript;
    private Deployable DeployableScript;

    // Start is called before the first frame update
    void Start()
    {
        isOccupied = false;
        isClickable = false;
        Player_Battlefield = GameObject.Find("Player Battlefield");
        Player_Hand = GameObject.Find("Player Hand");
        CardUI = new List<GameObject>();
        BeingScript = this.gameObject.GetComponent<Being>();
        DeployableScript = this.gameObject.GetComponent<Deployable>();
        //Get all the rect transforms in even the inactive components. We're using this to get access to all children.
        //Then, get the game object this component is attached to.
        foreach (RectTransform component in this.GetComponentsInChildren<RectTransform>(true))
        {

            CardUI.Add(component.gameObject);
            //This code allows me to access each component more specifically.
            //A switch would've been better here, oh well.
            if (component.gameObject.name == "CardBorder")
            {
                CardBorder = component.gameObject;
            }
            if (component.gameObject.name == "CardArt")
            {
                CardArt = component.gameObject;
            }
            if (component.gameObject.name == "CardTitleTextbox")
            {
                CardTitleTextbox = component.gameObject;
            }
            if (component.gameObject.name == "CardEffectTextbox")
            {
                CardEffectTextbox = component.gameObject;
            }
            if (component.gameObject.name == "CardTitle")
            {
                m_CardTitle = component.gameObject;
            }
            if (component.gameObject.name == "CardType/Subtypes")
            {
                CardTypeAndSubtypes = component.gameObject;
            }
            if (component.gameObject.name == "CardAbility")
            {
                CardAbility = component.gameObject;
            }
            if (component.gameObject.name == "FlavorText")
            {
                FlavorText = component.gameObject;
            }
            if (component.gameObject.name == "CardEnergyCost")
            {
                CardEnergyCost = component.gameObject;
            }
            if (component.gameObject.name == "BeingStatsBorder")
            {
                BeingStatsBorder = component.gameObject;
            }
            if (component.gameObject.name == "BeingStatsTextbox")
            {
                BeingStatsTextbox = component.gameObject;
            }
            if (component.gameObject.name == "BeingStatsText")
            {
                BeingStatsText = component.gameObject;
            }
            if (component.gameObject.name == "Placement Text B")
            {
                Placement_Text_B = component.gameObject;
            }
            if (component.gameObject.name == "DeployableDurabilityBorder")
            {
                DeployableDurabilityBorder = component.gameObject;
            }
            if (component.gameObject.name == "DeployableDurabilityTextbox")
            {
                DeployableDurabilityTextbox = component.gameObject;
            }
            if (component.gameObject.name == "DeployableDurabilityText")
            {
                DeployableDurabilityText = component.gameObject;
            }
            if (component.gameObject.name == "Placement Text D")
            {
                Placement_Text_D = component.gameObject;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Checks to see if the clicked card zone is unoccupied and is able to be clicked right now.
    //It then checks through the player's hand for the card that is trying to be placed down.
    //Finally, it sends a message to that card to Deploy itself and hides all of the other unoccupied
    //selectable card zones.
    public void IsClicked()
    {
        GameObject card;

        if (!isOccupied && isClickable)
        {
            //Find the selected card in hand
            for (int i = 0; i < Player_Hand.GetComponent<Hand_Manager>().CardsInPlayer_HandProperty.Count; i++)
            {
                card = Player_Hand.GetComponent<Hand_Manager>().CardsInPlayer_HandProperty[i];

                if (card.GetComponent<Card>().IsSelected)
                {
                    if (card.GetComponent<Card>() is Being)
                    {
                        
                        card.SendMessage("DeployBeing", this.gameObject.name);
                    }
                    if (card.GetComponent<Card>() is Deployable)
                    {
                        card.SendMessage("DeployDeployable", this.gameObject.name);
                    }
                }
            }
            isOccupied = true;
            Player_Battlefield.BroadcastMessage("HideDeployableZones");
        }

    }

    //Shows the card zone to the user and makes it selectable
    public void ShowDeployableZone()
    {
        if (!isOccupied)
        {
            isClickable = true;
            this.GetComponentInChildren<Image>().enabled = true;
        }
    }

    //Hides the card zone from the user and makes it selectable
    public void HideDeployableZone()
    {
        if (!isOccupied)
        {
            isClickable = false;
            this.GetComponentInChildren<Image>().enabled = false;
        }
    }

    //Accepts the data from a card trying to deploy itself into this card zone.
    //Uses the data to activate set the data in this card's inactive children.
    //It takes card as a parameter so that it can match the data of the card being sent.
    public void Deploy(Card cardData)
    {
        SetUIComponentsToActive(cardData);
        SetUIComponentColor(cardData.CardColor);
        SendDataToCard(cardData);
    }

    private void SetUIComponentColor(CardColor cardColor)
    {
        switch (cardColor)
        {
            case CardColor.Blue:
                CardBorder.GetComponent<Image>().color = Color.blue;
                BeingStatsBorder.GetComponent<Image>().color = Color.blue;
                break;

            case CardColor.Green:
                CardBorder.GetComponent<Image>().color = Color.green;
                BeingStatsBorder.GetComponent<Image>().color = Color.green;
                break;

            case CardColor.Red:
                CardBorder.GetComponent<Image>().color = Color.red;
                BeingStatsBorder.GetComponent<Image>().color = Color.red;
                break;

            case CardColor.Purple:
                CardBorder.GetComponent<Image>().color = Color.magenta;
                BeingStatsBorder.GetComponent<Image>().color = Color.green;
                break;
        }
    }
    //Helper method for setting all components that relate to the correct card type to active.
    //Since this is only for cards on the battlefield, it will only work for beings and deployables.
    //The cardData's type determines which components will be set to active.
    private void SetUIComponentsToActive(Card cardData)
    {
        //TODO: shift code around so universial components can be adjusted here.
        if (cardData is Being)
        {
            Being currentCard = (Being)cardData;

            foreach (GameObject uicomponent in CardUI)
            {
                uicomponent.SetActive(true);
                if(uicomponent == DeployableDurabilityBorder||
                   uicomponent == DeployableDurabilityTextbox||
                   uicomponent == DeployableDurabilityText||
                   uicomponent == Placement_Text_D)
                {
                    uicomponent.SetActive(false);
                }
                
            }
            //Time to set text values here
            CardTypeAndSubtypes.GetComponent<TMP_Text>().text = $"Being/{currentCard.Species}";
            BeingStatsText.GetComponent<TMP_Text>().text = $"{currentCard.OriginalMaxHealth}/{currentCard.OriginalPower}";
            CardAbility.GetComponent<TMP_Text>().text = $"{currentCard.AbilityText}";
            CardEnergyCost.GetComponent<TMP_Text>().text = $"{cardData.EnergyCost}";
            m_CardTitle.GetComponent<TMP_Text>().text = $"{currentCard.CardTitle}";
        }
        else if (cardData is Deployable)
        {
            Deployable currentCard = (Deployable)cardData;
            foreach (GameObject uicomponent in CardUI)
            {
                uicomponent.SetActive(true);
                if(uicomponent == BeingStatsBorder||
                   uicomponent == BeingStatsTextbox||
                   uicomponent == BeingStatsText||
                   uicomponent == Placement_Text_B)
                {
                    uicomponent.SetActive(false);
                }
                
            }
            //Time to set text values here
            CardTypeAndSubtypes.GetComponent<TMP_Text>().text = $"Deployable/{currentCard.Subtype}";
            DeployableDurabilityText.GetComponent<TMP_Text>().text = $"{currentCard.Durability}";
            CardAbility.GetComponent<TMP_Text>().text = $"{currentCard.AbilityText}";
            CardEnergyCost.GetComponent<TMP_Text>().text = $"{cardData.EnergyCost}";
            m_CardTitle.GetComponent<TMP_Text>().text = $"{currentCard.CardTitle}";
        }
        else
        {
            print("error, tried to deploy a tactic card");
        }
    }

    //helper method for sending data to the being class this card is attached to.
    private void SendDataToCard(Card cardData)
    {
        if (cardData is Being)
        {
            Being currentCard = (Being)cardData;
            BeingScript.SetUIComponentColor(currentCard.CardColor);
            BeingScript.Init(currentCard.CardColor, currentCard.OriginalMaxHealth, currentCard.OriginalPower, currentCard.Species,
            currentCard.AbilityText, currentCard.EnergyCost, currentCard.CardTitle);
            /* BeingScript.CardTitle = currentCard.CardTitle;
            BeingScript.CardType = CardType.Being;
            BeingScript.CardColor = currentCard.CardColor;
            BeingScript.EnergyCost = currentCard.EnergyCost;
            BeingScript.OriginalMaxHealth = currentCard.OriginalMaxHealth;
            BeingScript.OriginalPower = currentCard.OriginalPower;
            BeingScript.CurrentMaxHealth = currentCard.CurrentMaxHealth;
            BeingScript.CurrentHealth = currentCard.CurrentHealth;
            BeingScript.CurrentPower = currentCard.CurrentPower;
            BeingScript.Species = currentCard.Species;
            BeingScript.AbilityText = currentCard.AbilityText; */
            //BeingScript =  currentCard; doesn't change the data on the editor.
            //Use this script to test how data is sent.
            print(BeingScript);
        }
        if (cardData is Deployable)
        {
            Deployable currentCard = (Deployable)cardData;
            DeployableScript.SetUIComponentColor(currentCard.CardColor);
            DeployableScript.CardTitle = currentCard.CardTitle;
            DeployableScript.CardType = CardType.Deployable;
            DeployableScript.CardColor = currentCard.CardColor;
            DeployableScript.EnergyCost = currentCard.EnergyCost;
            DeployableScript.Subtype = currentCard.Subtype;
            DeployableScript.AbilityText = currentCard.AbilityText;
            //Use this script to test how data is sent.
            print(DeployableScript);
        }
    }
}
