using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CardBase;
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
public class Card : MonoBehaviour
{
    //For when a card is in hand and clicked by a player.
    //Remove Serialized field later.
    protected GameObject Player_Battlefield;
    protected GameObject Player_Hand;
    protected GameObject Player_Graveyard;
    protected GameObject Player_Portrait;
    protected Turn_Manager Turn_Manager;
    protected EnergySupplyManager Player_EnergySupply;
    [SerializeField] protected CardColor cardColor;
    protected CardType cardType;
    protected string cardTitle;
    protected int genericEnergyCost;
    protected int blueEnergyCost;
    protected int greenEnergyCost;
    protected int redEnergyCost;
    protected int purpleEnergyCost;
    protected int convertedEnergyCost;
    protected string abilityText;
    protected bool isSelected;
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
        Player_Battlefield = GameObject.Find("Player Battlefield");
        Player_Hand = GameObject.Find("Player Hand");
        Player_Graveyard = GameObject.Find("Player Graveyard");
        Player_Portrait = GameObject.Find("PlayerPortrait");
        Player_EnergySupply = Player_Portrait.GetComponent<EnergySupplyManager>();
        Turn_Manager = GameObject.Find("GameManager").GetComponent<Turn_Manager>();


        //CardUI = new List<GameObject>();
        foreach (RectTransform component in this.GetComponentsInChildren<RectTransform>(true))
        {
            //CardUI.Add(component.gameObject);
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
        }
    }

    //Helper method for setting this card's UI to the values in data.
    protected virtual void SetCardUI()
    {
        //This line of code is kinda useless since Card's SetUIComponent can't be called when this method
        //is called from a child. The override "hides" the base method.
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


    public virtual void IsClicked()
    {
        switch (Turn_Manager.currentPlayerTurn)
        {
            case Turn.P1:
                switch (this.tag)
                {
                    case "Hand":
                        //isSelected = true;
                        print("This card has been clicked");
                        break;
                }
                break;
            case Turn.P2:
                {
                    print("It's the other player's turn!");
                    return;
                }
        }
        
    }

    public override string ToString()
    {
        return $"{cardTitle},{cardType},{cardColor},{redEnergyCost}, {blueEnergyCost}, {greenEnergyCost}, {purpleEnergyCost}, {convertedEnergyCost},{abilityText}";
    }
}
