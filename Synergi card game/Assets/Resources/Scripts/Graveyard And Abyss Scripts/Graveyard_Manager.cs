using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Graveyard_Manager : MonoBehaviour
{
    private List<Card> GraveyardArray;
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
    private Tactic TacticScript;
    // Start is called before the first frame update
    void Start()
    {
        GraveyardArray = new List<Card>();
        Player_Battlefield = GameObject.Find("Player Battlefield");
        Player_Hand = GameObject.Find("Player Hand");
        CardUI = new List<GameObject>();
        BeingScript = this.gameObject.GetComponentInChildren<Being>();
        DeployableScript = this.gameObject.GetComponentInChildren<Deployable>();
        TacticScript = this.gameObject.GetComponentInChildren<Tactic>();

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

    //Adds a new card to the graveyard and replaces the top card image (or if there's no cards at all the nothingness) with the new card's image.
    public void AddCardToGraveyard(Card card)
    {
        card.gameObject.tag = "Graveyard";
        GraveyardArray.Add(card);
        SetUIComponentsToActive(card);
        SendDataToCard(card);
    }

     //Helper method for setting all components that relate to the correct card type to active.
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
            CardEnergyCost.GetComponent<TMP_Text>().text = $"Red:{cardData.RedEnergyCost} Blue:{cardData.BlueEnergyCost} Green:{cardData.GreenEnergyCost} Purple:{cardData.PurpleEnergyCost}";
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
            CardEnergyCost.GetComponent<TMP_Text>().text = $"Red:{cardData.RedEnergyCost} Blue:{cardData.BlueEnergyCost} Green:{cardData.GreenEnergyCost} Purple:{cardData.PurpleEnergyCost}";
            m_CardTitle.GetComponent<TMP_Text>().text = $"{currentCard.CardTitle}";
        }
        else if(cardData is Tactic)
        {
            Tactic currentCard = (Tactic)cardData;
            foreach (GameObject uicomponent in CardUI)
            {
                uicomponent.SetActive(true);
                if(uicomponent == BeingStatsBorder||
                   uicomponent == BeingStatsTextbox||
                   uicomponent == BeingStatsText||
                   uicomponent == Placement_Text_B||
                   uicomponent == DeployableDurabilityBorder||
                   uicomponent == DeployableDurabilityTextbox||
                   uicomponent == DeployableDurabilityText||
                   uicomponent == Placement_Text_D)
                {
                    uicomponent.SetActive(false);
                }
                
            }
            //Time to set text values here
            CardTypeAndSubtypes.GetComponent<TMP_Text>().text = $"Tactic/{currentCard.Subtype}";
            CardAbility.GetComponent<TMP_Text>().text = $"{currentCard.AbilityText}";
            CardEnergyCost.GetComponent<TMP_Text>().text = $"Red:{cardData.RedEnergyCost} Blue:{cardData.BlueEnergyCost} Green:{cardData.GreenEnergyCost} Purple:{cardData.PurpleEnergyCost}";
            m_CardTitle.GetComponent<TMP_Text>().text = $"{currentCard.CardTitle}";
        }
        else
        {
            print("error, didn't receive a valid card type");
        }
    }

    //helper method for sending data to the being class this card is attached to.
    private void SendDataToCard(Card cardData)
    {
        if (cardData is Being)
        {
            Being currentCard = (Being)cardData;
            BeingScript.SetUIComponentColor(currentCard.CardColor);
            BeingScript.Init(currentCard.CardColor,
                             currentCard.OriginalMaxHealth,
                             currentCard.OriginalPower,
                             currentCard.Species,
                             currentCard.AbilityText,
                             currentCard.RedEnergyCost,
                             currentCard.BlueEnergyCost,
                             currentCard.GreenEnergyCost,
                             currentCard.PurpleEnergyCost,
                             currentCard.GenericEnergyCost,
                             currentCard.CardTitle);
            
            //BeingScript =  currentCard; doesn't change the data on the editor.
            //Use this script to test how data is sent.
            //print(BeingScript);
        }
        if (cardData is Deployable)
        {
            Deployable currentCard = (Deployable)cardData;
            DeployableScript.SetUIComponentColor(currentCard.CardColor);
            DeployableScript.Init(currentCard.CardColor,
                                  currentCard.RedEnergyCost,
                                  currentCard.BlueEnergyCost,
                                  currentCard.GreenEnergyCost,
                                  currentCard.PurpleEnergyCost,
                                  currentCard.GenericEnergyCost,
                                  currentCard.CardTitle,
                                  currentCard.AbilityText,
                                  currentCard.Durability);
            //Use this script to test how data is sent.
            //print(DeployableScript);
        }
        if (cardData is Tactic)
        {
            Tactic currentCard = (Tactic)cardData;
            TacticScript.SetUIComponentColor(currentCard.CardColor);
            TacticScript.Init(currentCard.CardColor,
                              currentCard.RedEnergyCost,
                              currentCard.BlueEnergyCost,
                              currentCard.GreenEnergyCost,
                              currentCard.PurpleEnergyCost,
                              currentCard.GenericEnergyCost,
                              currentCard.CardTitle,
                              currentCard.AbilityText,
                              currentCard.Subtype);
            //Use this script to test how data is sent.
            //print(TacticScript);
        }
    }
}
