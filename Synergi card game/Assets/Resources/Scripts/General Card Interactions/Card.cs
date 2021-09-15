using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardBase;
using Mirror;
public enum CardColor
{
    Red,
    Blue,
    Green,
    Purple
}
public enum CardType
{
    Being,
    Tactic,
    Deployable,
    Crystal
}
public enum CardPositions
{
    FrontLeft,
    FrontCenter,
    FrontRight,
    MiddleLeft,
    MiddleCenter,
    MiddleRight,
    BackLeft,
    BackCenter,
    BackRight,
    Hand,
    TacticalField,
    Graveyard,
    Abyss
}

//Being removed for now since host is automatically determined to be player1.
//public enum Owner
//{
//    Player1,//The player
//    Player2//The enemy
//}
/// <summary>
/// Purpose: The base card class from which all others are derived from.
///          Although you can technically call this class, it serves as an abstract class for the most part.
///          
/// Restrictions: All game objects that have this script and its children are prefabs. 
///               Therefore, slapping on serialized fields won't work
///               because prefabs can't access scene references in the editor.
///               As such, using GameObject.Find is our best bet. This makes it very important to watch strings.
///               Carefully consider your options before trying to change strings in general.
/// </summary>
public class Card : MonoBehaviour
{
    //For when a card is in hand and clicked by a player.
    //Remove Serialized field later.
    protected Battlefield_Zone_Manager player_Battlefield;
    protected Battlefield_Zone_Manager enemy_Battlefield;
    protected Hand_Manager player_Hand;
    protected GameObject player_Graveyard;
    protected GameObject player_Portrait;
    protected GameObject enemy_Portrait;
    protected GameObject game_Manager;
    protected Turn_Manager turn_Manager;
    protected EnergySupplyManager Player_EnergySupply;
    [SerializeField] protected CardColor cardColor;
    protected CardType cardType;
    protected CardPositions currentPosition;
    protected string cardTitle;
    protected int genericEnergyCost;
    protected int blueEnergyCost;
    protected int greenEnergyCost;
    protected int redEnergyCost;
    protected int purpleEnergyCost;
    protected int convertedEnergyCost;
    protected string abilityText;
    // for when a player has selected a card in the hand
    protected bool isSelected;
    // for preventing a player from using a card when it is not their turn
    protected bool playerActive;
    // a bool that allows the Card class to do all the work for its children
    protected bool cardIsPlayable;
    // a bool that determines whether or not a card is visibile to the opponent
    protected bool isVisible;
    protected GameObject m_CardTitle;
    protected GameObject CardBorder;
    protected GameObject CardArt;
    protected GameObject CardTitleTextbox;
    protected GameObject CardEffectTextbox;
    protected GameObject CardTypeAndSubtypes;
    protected GameObject CardAbility;
    protected GameObject FlavorText;
    protected GameObject CardEnergyCost;
    protected GameObject CardBack;
    protected GameObject CardFront;
    protected List<GameObject> cardUI = new List<GameObject>();

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;
        }
    }

    public CardColor CardColor
    {
        get
        {
            return cardColor;
        }
        set
        {
            cardColor = value;
        }
    }

    public CardType CardType
    {
        get
        {
            return cardType;
        }
        set
        {
            cardType = value;
        }
    }
    public string CardTitle
    {
        get
        {
            return cardTitle;
        }
        set
        {
            cardTitle = value;
        }
    }
    public int GenericEnergyCost
    {
        get
        {
            return genericEnergyCost;
        }
        set
        {
            genericEnergyCost = value;
        }
    }
    public int RedEnergyCost
    {
        get
        {
            return redEnergyCost;
        }
        set
        {
            redEnergyCost = value;
        }
    }
    public int BlueEnergyCost
    {
        get
        {
            return blueEnergyCost;
        }
        set
        {
            blueEnergyCost = value;
        }
    }
    public int GreenEnergyCost
    {
        get
        {
            return greenEnergyCost;
        }
        set
        {
            greenEnergyCost = value;
        }
    }
    public int PurpleEnergyCost
    {
        get
        {
            return purpleEnergyCost;
        }
        set
        {
            purpleEnergyCost = value;
        }
    }
    public string AbilityText
    {
        get
        {
            return abilityText;
        }
        set
        {
            abilityText = value;
        }
    }
    public CardPositions CurrentPosition
    {
        get
        {
            return currentPosition;
        }
        set
        {
            currentPosition = value;
        }
    }
    public bool IsVisible
    {
        get
        {
            return isVisible;
        }
        set
        {
            isVisible = value;
        }
    }

    public Card(CardType cardType, CardColor cardColor,
        int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost,
        string cardTitle, string abilityText)
    {
        this.cardType = cardType;
        this.cardColor = cardColor;
        this.redEnergyCost = redEnergyCost;
        this.blueEnergyCost = blueEnergyCost;
        this.greenEnergyCost = greenEnergyCost;
        this.purpleEnergyCost = purpleEnergyCost;
        this.genericEnergyCost = genericEnergyCost;
        convertedEnergyCost = redEnergyCost + blueEnergyCost + greenEnergyCost + purpleEnergyCost + genericEnergyCost;

        this.cardTitle = cardTitle;
        this.abilityText = abilityText;
    }

    //My DeFacto constructor. Unity's methods for creating a new being are not ideal.
    public void Init(CardType cardType, CardColor cardColor,
        int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost,
        string cardTitle, string abilityText)
    {
        this.cardType = cardType;
        this.cardColor = cardColor;
        this.redEnergyCost = redEnergyCost;
        this.blueEnergyCost = blueEnergyCost;
        this.greenEnergyCost = greenEnergyCost;
        this.purpleEnergyCost = purpleEnergyCost;
        this.genericEnergyCost = genericEnergyCost;
        convertedEnergyCost = redEnergyCost + blueEnergyCost + greenEnergyCost + purpleEnergyCost + genericEnergyCost;

        this.cardTitle = cardTitle;
        this.abilityText = abilityText;
    }

    // Unity keeps on calling the CardType of my cards beings(the default type). I think this is because I'm setting
    // the card type in the constructor instead of start (Unity gets weird when I assign things "before").
    protected virtual void Start()
    {
        //I may have to eventually change these so that its assigned an object in a serialized field instead.
        player_Battlefield = GameObject.Find("Player Battlefield").GetComponent<Battlefield_Zone_Manager>();
        enemy_Battlefield = GameObject.Find("Enemy Battlefield").GetComponent<Battlefield_Zone_Manager>();
        player_Hand = GameObject.Find("Player Hand").GetComponent<Hand_Manager>();
        player_Graveyard = GameObject.Find("Player Graveyard");
        player_Portrait = GameObject.Find("PlayerPortrait");
        enemy_Portrait = GameObject.Find("EnemyPortrait");
        Player_EnergySupply = player_Portrait.GetComponent<EnergySupplyManager>();
        game_Manager = GameObject.Find("GameManager");
        turn_Manager = game_Manager.GetComponent<Turn_Manager>();
        isVisible = true;


        foreach (RectTransform component in this.GetComponentsInChildren<RectTransform>(true))
        {
            //Adds the components to a list for easier access later.
            //cardUI.Add(component.gameObject);

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
            if(component.gameObject.name == "CardFront")
            {
                CardFront = component.gameObject;
            }
            if (component.gameObject.name == "CardBack")
            {
                CardBack = component.gameObject;
            }
        }
    }

    protected virtual void Update()
    {

        bool isBattlefieldCard = false;
        //Determine which cards are and are not visible
        switch (CurrentPosition)
        {
            //If a card is on the battlefield it should be visible
            case CardPositions.FrontLeft:
            case CardPositions.FrontCenter:
            case CardPositions.FrontRight:
            case CardPositions.MiddleLeft:
            case CardPositions.MiddleCenter:
            case CardPositions.MiddleRight:
            case CardPositions.BackLeft:
            case CardPositions.BackCenter:
            case CardPositions.BackRight:
                //TODO: Cloaked mechanic
                //If Cloaked = false, a mechanic I'd like to add in the future
                //IsVisible = true;
                //For now, if a card is in the battlefield, ignore it
                isBattlefieldCard = true;
                return;

            //If the card is in a player's own hand it should be visible by default.
            //Else it should be invisible by default.
            case CardPositions.Hand:
                if(player_Hand.GetComponent<Hand_Manager>().CardsInPlayer_Hand.Contains(this.gameObject))
                {
                    IsVisible = true;
                }
                else
                {
                    //Do a check to see if a card has been revealed here
                    //if(isRevealed)
                    IsVisible = false;
                }
                break;

        }

        if(isBattlefieldCard)
        {
            print($"{CurrentPosition}: Switching visibility");
        }
        if (!IsVisible)
       {
           ShowCardBack();
       }
       else
       {
           ShowCardFront();
       }
    }

    //Helper method for setting this card's UI to the values in data.
    protected virtual void SetCardUI()
    {
        //This line of code is kinda useless since Card's SetUIComponent can't be called when this method
        //is called from a child. The override "hides" the base method.
        //???? I need to debug this later
        //SetUIComponentColor(cardColor);

        CardAbility.GetComponent<TMP_Text>().text = $"{this.abilityText}";
        CardEnergyCost.GetComponent<TMP_Text>().text = $"{this.convertedEnergyCost}";
        m_CardTitle.GetComponent<TMP_Text>().text = $"{this.cardTitle}";
    }

    //Sets the color of this card equal to the cardColor variable.
    //I will eventually make this protected.
    public virtual void SetUIComponentColor(CardColor cardColor)
    {
        switch (cardColor)
        {
            case CardColor.Blue:

                CardBorder.GetComponent<Image>().color = Color.blue;
                break;

            case CardColor.Green:
                CardBorder.GetComponent<Image>().color = Color.green;
                break;

            case CardColor.Red:
                CardBorder.GetComponent<Image>().color = Color.red;
                break;

            case CardColor.Purple:
                CardBorder.GetComponent<Image>().color = Color.magenta;
                break;
        }
    }

    /// <summary>
    /// Purpose: Determines if a card be played. 
    ///          If so, tells the child instance to do its thing via the cardIsPlayable bool.
    ///          A card is considered playable until proven unplayable.
    /// Restrictions:
    /// TODO: Make either this method or another method that sends the card's data to a zoom in of sorts.
    /// </summary>
    public virtual void IsClicked()
    {
        switch (turn_Manager.GetCurrentTurnNetbased())
        {
            case Turn.Self:
                switch (currentPosition)
                {
                    case CardPositions.Hand:

                        playerActive = true;
                        cardIsPlayable = true;
                        //Only one card may be selected at a time. I get a component in each child just so I have an array.
                        for (int i = 0; i < player_Hand.GetComponent<Hand_Manager>().CardsInPlayer_Hand.Count; i++)
                        {
                            player_Hand.GetComponent<Hand_Manager>().CardsInPlayer_Hand[i].GetComponent<Card>().IsSelected = false;
                        }

                        isSelected = true;

                        //Check if it is this player's turn 
                        //(certain keywords/abilities will be a part of this if statement in the future. EX: Flash)
                        if (!playerActive)
                        {
                            cardIsPlayable = false;
                            return;
                        }

                        //Checks if it is the main phase at this moment
                        //Again, certain keywords and abilities will be a part of this if statement in the future.)
                        if (turn_Manager.CurrentPhase != Phases.MainPhase1 &&
                           turn_Manager.CurrentPhase != Phases.MainPhase2)
                        {
                            cardIsPlayable = false;
                            return;
                        }

                        //Checks if the player has enough energy to play this card.
                        //If energyGiven < energyCost, you can't play the card.
                        if (Player_EnergySupply.TotalRedEnergy < RedEnergyCost &&
                           Player_EnergySupply.TotalBlueEnergy < BlueEnergyCost &&
                           Player_EnergySupply.TotalGreenEnergy < GreenEnergyCost &&
                           Player_EnergySupply.TotalPurpleEnergy < PurpleEnergyCost
                           )
                        {
                            cardIsPlayable = false;
                        }
                        break;

                }
                break;
            case Turn.Other:
                switch (currentPosition)
                {
                    case CardPositions.Hand:
                        //isSelected = true;
                        playerActive = false;
                        //print("It's the other player's turn!");
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// Purpose: Shows only the back of a card so that players don't know which one it is.
    /// Restrictions:
    /// 
    /// </summary>
    protected virtual void ShowCardBack()
    {
        foreach(RectTransform component in CardFront.GetComponentInChildren<RectTransform>())
        {
            component.gameObject.SetActive(false);
        }
        //CardFront.SetActive(false);
        CardBack.GetComponent<Image>().enabled = true;
    }

    protected virtual void ShowCardFront()
    {
        foreach (RectTransform component in CardFront.GetComponentInChildren<RectTransform>())
        {
            component.gameObject.SetActive(true);
        }
        //CardFront.SetActive(true);
        CardBack.GetComponent<Image>().enabled = false;
    }

    public override string ToString()
    {
        return $"{cardTitle},{cardType},{cardColor},{redEnergyCost}, {blueEnergyCost}, {greenEnergyCost}, {purpleEnergyCost}, {convertedEnergyCost},{abilityText}";
    }
}
