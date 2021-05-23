using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum TacticSubtypes
{
    Normal,
    Lasting,
    Instant,
    Equip
}
public class Tactic : Card
{
    protected GameObject TacticGrid;
    protected TacticSubtypes subtype;

    public TacticSubtypes Subtype
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

    public Tactic(CardColor cardColor, 
                  int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost, 
                  string cardTitle, string abilityText, TacticSubtypes subtype) :
    base(CardType.Tactic, cardColor, redEnergyCost, blueEnergyCost, greenEnergyCost, purpleEnergyCost, genericEnergyCost, cardTitle, abilityText)
    {
        this.subtype = subtype;
    }

    public void Init(CardColor cardColor, 
                     int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost,
                     string cardTitle, string abilityText, TacticSubtypes subtype)
    {
        base.Init(CardType.Tactic, cardColor, redEnergyCost, blueEnergyCost, greenEnergyCost, purpleEnergyCost, genericEnergyCost, cardTitle, abilityText);
        this.subtype = subtype;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        //This is a line of code to deal with the CardType error
        CardType = CardType.Tactic;

        base.Start();
        TacticGrid = GameObject.Find("Player Tactical Field");
        SetCardUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //When a tactic is clicked, the system will ask for confirmation if you want to play this card. 
    //If the user confirms, it will go to a side zone next to the battlefield to do its effects.
    //Then if its not a lasting card it will be sent to the graveyard.
    //For now, lets just send it to the graveyard.
    //TODO:
    //Step 4, have the card go to the graveyard if its not a lasting card.
    //Note: You will probably have to create subclasses with more specific behaviors. If so, here's an alternate road map.
    //Alt 1, have the card print that its a tactic.
    //Alt 2, have the card print text only if this card is in the hand (indicated via tags).
    //Alt 3, have selecting the card send it to a data array list of tactics.
    public override void IsClicked()
    {
        base.IsClicked();

        switch(currentPosition)
        {
            case CardPositions.Hand:

                //Checks if the player has enough energy to play this card.
                //If energyGiven >= energyCost, you can play the card.
                if (cardIsPlayable)
                {
                    Utilize();
                }
                break;

            case CardPositions.TacticalField:
                //DestroyTactic is being called for testing purposes
                DestroyTactic();
                break;
        }
    }

    //Void for when a tactic is played.
    public virtual void Utilize()
    {
        TacticGrid.GetComponent<Tactic_Zone_Manager>().SendMessage("Add", this.GetComponent<Tactic>());
        Player_Hand.GetComponent<Hand_Manager>().GetRidOfDestroyedCards(this);
    }

    public virtual void DestroyTactic()
    {
        this.tag = "Graveyard";
        Player_Graveyard.GetComponent<Graveyard_Manager>().SendMessage("AddCardToGraveyard", this.GetComponent<Tactic>());
        TacticGrid.GetComponent<Tactic_Zone_Manager>().SendMessage("Remove", this.GetComponent<Tactic>());
    }

    //Helper method for setting this card's UI to the values in data.
    protected override void SetCardUI()
    {
        base.SetCardUI();
        SetUIComponentColor(cardColor);
    }

    //Sets the color of this card equal to the cardColor variable.
    public override void SetUIComponentColor(CardColor cardColor)
    {
        base.SetUIComponentColor(cardColor);
        //I'll keep this code commented out until tactics show the need for further color based changes.
        /* switch (cardColor)
        {
            case CardColor.Blue:
                beingStatsBorder.GetComponent<Image>().color = Color.blue;
                break;

            case CardColor.Green:
                beingStatsBorder.GetComponent<Image>().color = Color.green;
                break;

            case CardColor.Red:
                beingStatsBorder.GetComponent<Image>().color = Color.red;
                break;

            case CardColor.Purple:
                beingStatsBorder.GetComponent<Image>().color = Color.green;
                break;
        } */
    }


    public override string ToString()
    {
        return base.ToString(); //TODO: Add tactic subtype (normal, lasting, equip);
    }
}
