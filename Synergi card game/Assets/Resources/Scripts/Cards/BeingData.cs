using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBase
{
    [CreateAssetMenu(menuName = "Cards/Being")]
    public class BeingData : CardData
    {
    [SerializeField] protected int originalMaxHealth;
    [SerializeField] protected int originalPower;
    [SerializeField] protected int currentMaxHealth;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int currentPower;
    [SerializeField] protected string species;

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

    public BeingData(CardColor cardColor, int originalMaxHealth, int originalPower,
    string species, string abilityText, int energyCost, string cardTitle)
    : base(CardType.Being, cardColor, energyCost, cardTitle, abilityText)
    {
        this.Init(cardColor, originalMaxHealth, originalPower, species, abilityText, energyCost, cardTitle);
    }

    //My DeFacto constructor. Unity's methods for creating a new being are not ideal.
    public void Init(CardColor cardColor, int originalMaxHealth, int originalPower,
    string species, string abilityText, int energyCost, string cardTitle)
    {
        base.Init(CardType.Being, cardColor, energyCost, cardTitle, abilityText);
        this.originalMaxHealth = originalMaxHealth;
        this.originalPower = originalPower;
        this.currentMaxHealth = originalPower;
        this.currentHealth = currentMaxHealth;
        this.currentPower = originalPower;
        this.species = species;
    }

    }

}
