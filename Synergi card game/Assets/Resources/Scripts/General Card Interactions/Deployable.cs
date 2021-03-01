using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Deployable : Card
{
    //TODO: change public primitives back to protected
    public int durability;
    public string subtype;
    protected GameObject DeployableDurabilityBorder;
    protected GameObject DeployableDurabilityTextbox;
    protected GameObject DeployableDurabilityText;
    protected GameObject Placement_Text_D;
    public int Durability
    {
        get
        {
            return durability;
        }
        set
        {
            durability = value;
        }
    }

    public string Subtype
    {
        get
        {
            return subtype;
        }
        set
        {
            subtype = value;
        }
    }
    public Deployable(CardColor cardColor, int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost,
                      string cardTitle, string abilityText, int durability) :
                      base(CardType.Deployable, cardColor, redEnergyCost, blueEnergyCost, greenEnergyCost, purpleEnergyCost, genericEnergyCost, cardTitle, abilityText)
    {
        this.durability = durability;
    }

    //My DeFacto constructor. Unity's methods for creating a new card are not ideal.
    public void Init(CardColor cardColor, int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost,
                     string cardTitle, string abilityText, int durability)
    {
        this.durability = durability;
        base.Init(CardType.Deployable, cardColor, redEnergyCost, blueEnergyCost, greenEnergyCost, purpleEnergyCost, genericEnergyCost, cardTitle, abilityText);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        //This is a line of code to deal with the CardType error
        cardType = CardType.Deployable;

        base.Start();
        foreach (RectTransform component in this.GetComponentsInChildren<RectTransform>(true))
        {
            //CardUI.Add(component.gameObject);
            //This code allows me to access each component more specifically.
            //A switch would've been better here, oh well.
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
        SetCardUI();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //Deployables should follow the same road map as beings deployment wise. Look at that for reference.
    public override void IsClicked()
    {
        switch(this.tag)
        {
            case "Hand":
                //Only one card may be selected at a time. I get a component in each child just so I have an array.
                foreach (GameObject card in Player_Hand.GetComponent<Hand_Manager>().CardsInPlayer_HandProperty)
                {
                    card.GetComponent<Card>().IsSelected = false;
                }
                isSelected = true;
                Player_Battlefield.SendMessage("ShowDeployableZones");
                break;
        }
    }

    public virtual void DeployDeployable(string nameOfCardZone)
    {
        GameObject.Find(nameOfCardZone).GetComponent<CardZone>().SendMessage("Deploy", this.GetComponent<Deployable>());
        Player_Hand.GetComponent<Hand_Manager>().GetRidOfDestroyedCards(this);
    }

    //Helper method for setting this card's UI to the values in data.
    protected override void SetCardUI()
    {
        base.SetCardUI();
        SetUIComponentColor(cardColor);
        CardTypeAndSubtypes.GetComponent<TMP_Text>().text = $"Deployable/{this.subtype}";
        DeployableDurabilityText.GetComponent<TMP_Text>().text = $"{this.durability}";
    }

    //Sets the color of this card equal to the cardColor variable.
    public override void SetUIComponentColor(CardColor cardColor)
    {
        base.SetUIComponentColor(cardColor);
        switch (cardColor)
        {
            case CardColor.Blue:
                DeployableDurabilityBorder.GetComponent<Image>().color = Color.blue;
                break;

            case CardColor.Green:
                DeployableDurabilityBorder.GetComponent<Image>().color = Color.green;
                break;

            case CardColor.Red:
                DeployableDurabilityBorder.GetComponent<Image>().color = Color.red;
                break;

            case CardColor.Purple:
                DeployableDurabilityBorder.GetComponent<Image>().color = Color.green;
                break;
        }
    }
    public override string ToString()
    {
        return base.ToString() + $"{durability},{subtype}";
    }
}
