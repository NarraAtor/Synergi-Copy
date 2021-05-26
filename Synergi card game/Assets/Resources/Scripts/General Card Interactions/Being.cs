using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Being : Card
{
//TODO: make fields protected
    protected int originalMaxHealth;
    protected int originalPower;
    protected int currentMaxHealth;
    protected int currentHealth;
    protected int currentPower;
    protected string species;
    protected GameObject beingStatsBorder;
    protected GameObject beingStatsTextbox;
    protected GameObject beingStatsText;
    protected GameObject placement_Text_B;
    protected GameObject battleNumber;
    public int OriginalMaxHealth
    {
        get
        {
            return originalMaxHealth;
        }
        set
        {
            originalMaxHealth = value;
        }
    }
    public int OriginalPower
    {
        get
        {
            return originalPower;
        }
        set
        {
            originalPower = value;
        }
    }
    public int CurrentMaxHealth
    {
        get
        {
            return currentMaxHealth;
        }
        set
        {
            currentMaxHealth = value;
        }
    }
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;
        }
    }
    public int CurrentPower
    {
        get
        {
            return currentPower;
        }
        set
        {
            currentPower = value;
        }
    }

    public string Species
    {
        get
        {
            return species;
        }
        set
        {
            species = value;
        }
    }

    public GameObject BattleNumber
    {
        get
        {
            return battleNumber;
        }
    }
    public Being(CardColor cardColor, int originalMaxHealth, int originalPower,
    string species, string abilityText,
    int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost, string cardTitle)
    : base(CardType.Being, cardColor, redEnergyCost, blueEnergyCost, greenEnergyCost, purpleEnergyCost, genericEnergyCost, cardTitle, abilityText)
    {
        this.Init(cardColor, originalMaxHealth, originalPower, species, abilityText, redEnergyCost, blueEnergyCost, greenEnergyCost, purpleEnergyCost, genericEnergyCost, cardTitle);
    }

    //My DeFacto constructor. Unity's methods for creating a new being are not ideal.
    public void Init(CardColor cardColor, int originalMaxHealth, int originalPower,
    string species, string abilityText,
    int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost,
    string cardTitle)
    {
        this.originalMaxHealth = originalMaxHealth;
        this.currentMaxHealth = originalMaxHealth;
        this.currentHealth = originalMaxHealth;
        this.originalPower = originalPower;
        this.currentPower = originalPower;
        this.species = species;
        base.Init(CardType.Being, cardColor, redEnergyCost, blueEnergyCost, greenEnergyCost, purpleEnergyCost, genericEnergyCost, cardTitle, abilityText);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        //This is a line of code to deal with the CardType error
        CardType = CardType.Being;
        base.Start();
        foreach (RectTransform component in this.GetComponentsInChildren<RectTransform>(true))
        {
            //CardUI.Add(component.gameObject);
            //This code allows me to access each component more specifically.
            switch(component.gameObject.name)
            {
                case "BeingStatsBorder":
                    beingStatsBorder = component.gameObject;
                    break;
                case "BeingStatsTextbox":
                    beingStatsTextbox = component.gameObject;
                    break;
                case "BeingStatsText":
                    beingStatsText = component.gameObject;
                    break;
                case "Placement Text B":
                    placement_Text_B = component.gameObject;
                    break;
                case "BattleNumber":
                    battleNumber = component.gameObject;
                    break;
            }
        }
        SetCardUI();
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    //When a being is clicked while in the hand, it will prompt the user to select a card zone to deploy the being or hit back.
    //TODO: Make a back button;
    public override void IsClicked()
    {
        base.IsClicked();
        switch (currentPosition)
        {
            case CardPositions.Hand:
                {
                    
                    if (cardIsPlayable)
                    {
                        Player_Battlefield.SendMessage("ShowDeployableZones");
                    }

                }
                break;

                
            case CardPositions.FrontLeft:
            case CardPositions.MiddleLeft:
            case CardPositions.BackLeft:
                {
                    print($"Attacked down left lane! {currentPosition}");
                    //attack declaration system is below
                    //may be moved to turn manager

                    if (Turn_Manager.CurrentPhase == Phases.BattlePhase && Turn_Manager.CurrentPlayerTurn == Turn.P1)
                    {
                        Turn_Manager.DeclareAttacker(this);
                        

                        //Deal Damage to the enemy
                        //Enemy_Portrait.GetComponent<LifeManager>().DamagePlayer(DamageTypes.Battle, CurrentPower);
                    }
                }
                break;
            case CardPositions.FrontCenter:
            case CardPositions.MiddleCenter:
            case CardPositions.BackCenter:
                {
                    print($"Attacked down center lane!  {currentPosition}");
                    //attack declaration system is below
                    //may be moved to turn manager

                    if (Turn_Manager.CurrentPhase == Phases.BattlePhase && Turn_Manager.CurrentPlayerTurn == Turn.P1)
                    {
                        Turn_Manager.DeclareAttacker(this);


                        //Deal Damage to the enemy
                        //Enemy_Portrait.GetComponent<LifeManager>().DamagePlayer(DamageTypes.Battle, CurrentPower);
                    }
                }
                break;
            case CardPositions.FrontRight:
            case CardPositions.MiddleRight:
            case CardPositions.BackRight:
                {
                    print($"Attacked down right lane! {currentPosition}");
                    //attack declaration system is below
                    //may be moved to turn manager

                    if (Turn_Manager.CurrentPhase == Phases.BattlePhase && Turn_Manager.CurrentPlayerTurn == Turn.P1)
                    {
                        Turn_Manager.DeclareAttacker(this);


                        //Deal Damage to the enemy
                        //Enemy_Portrait.GetComponent<LifeManager>().DamagePlayer(DamageTypes.Battle, CurrentPower);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Purpose: "Plays" this card onto the battlefield by destroying this card and creating a new being with this card's info.
    /// </summary>
    /// <param name="nameOfCardZone"></param>
    public virtual void DeployBeing(string nameOfCardZone)
    {
        GameObject.Find(nameOfCardZone).GetComponent<CardZone>().SendMessage("Deploy", this.GetComponent<Being>());
        Player_Hand.GetComponent<Hand_Manager>().GetRidOfDestroyedCards(this);
    }

    /// <summary>
    /// Purpose: Confirms that this card is attacking and deals damage to the opposing player.
    ///          TODO: Have this confirm attackers then go into declare defenders.
    /// </summary>
    public virtual void CommitAttack()
    {
        Enemy_Portrait.GetComponent<LifeManager>().DamagePlayer(DamageTypes.Battle, CurrentPower);
    }

    //Helper method for setting this card's UI to the values in data.
    protected override void SetCardUI()
    {
        base.SetCardUI();
        SetUIComponentColor(cardColor);
        CardTypeAndSubtypes.GetComponent<TMP_Text>().text = $"Being/{this.Species}";
        beingStatsText.GetComponent<TMP_Text>().text = $"{this.OriginalMaxHealth}/{this.OriginalPower}";
    }

    //Sets the color of this card equal to the cardColor variable.
    public override void SetUIComponentColor(CardColor cardColor)
    {
        base.SetUIComponentColor(cardColor);
        switch (cardColor)
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
        }
    }


    public override string ToString()
    {
        return base.ToString() + $", {originalMaxHealth}, {originalPower}, {currentMaxHealth}, {currentHealth}, {currentPower},{species}";
    }


}
