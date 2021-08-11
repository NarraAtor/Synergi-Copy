﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;
using Mirror;

public class ResourceDeckManager : NetworkBehaviour
{
    //private Stack<CrystalData> resourceDeckArray = new Stack<CrystalData>(10);
    [SerializeField] private GameObject energySupply;
    [SerializeField] private GameObject cardDatabase;

    //Temporary list of hard coded crystals
    //4/30 No longer temporary. Players will add a copy of a crystal to their supply at the start of their turn.
    private CrystalData[] arrayOfBasicCrystals;
    public CrystalData BlueCrystal { get; private set; }
    public CrystalData GreenCrystal { get; private set; }
    public CrystalData PurpleCrystal { get; private set; }
    public CrystalData RedCrystal { get; private set; }

    //I can't exactly picture how a user will pick their own deck, so I'll just create a prototype-deck with hard coded data.
    //No longer a deck, now just an array of crystals.
    private void Start()
    {
        arrayOfBasicCrystals = new CrystalData[] {Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Blue Crystal"),
                                                  Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Green Crystal"),
                                                  Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Purple Crystal"),
                                                  Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Red Crystal")};
        BlueCrystal = arrayOfBasicCrystals[0];
        GreenCrystal = arrayOfBasicCrystals[1];
        PurpleCrystal = arrayOfBasicCrystals[2];
        RedCrystal = arrayOfBasicCrystals[3];

        //MakeResourceDeck();

    }

    /// <summary>
    /// Purpose: Makes a 10 card resource deck. Is hard coded for now.
    ///          TODO: Make cards chosen from the card database form the deck instead.
    /// </summary>
    //private void MakeResourceDeck()
    //{
    //    for(int i = 0; i < 10; i++)
    //    {
    //        //resourceDeckArray.Push(arrayOfBasicCrystals[i % 4]);
    //    }
    //}

    /// <summary>
    /// Purpose: Draws a card from the resource deck and adds it to the player's Energy Supply.
    /// </summary>
    //public void Draw()
    //{
    //    //energySupply.GetComponent<EnergySupplyManager>().Add(resourceDeckArray.Pop());
    //}
    
    /// <summary>
    /// Purpose: Adds a crystal from the array of crystals to the player's energy supply.
    ///          TODO: Figure out why I'm not using this right now.
    ///          Redacted for now,does not seems to server any functional purpose.
    /// Restrictions:
    /// </summary>
    /// <param name="color">the color of energy to add</param>
    public void AddCrystalToSupply(CardColor color)
    {
        switch(color)
        {
            case CardColor.Blue:
                energySupply.GetComponent<EnergySupplyManager>().Add(arrayOfBasicCrystals[0]);
                break;

            case CardColor.Green:
                energySupply.GetComponent<EnergySupplyManager>().Add(arrayOfBasicCrystals[1]);
                break;

            case CardColor.Purple:
                energySupply.GetComponent<EnergySupplyManager>().Add(arrayOfBasicCrystals[2]);
                break;

            case CardColor.Red:
                energySupply.GetComponent<EnergySupplyManager>().Add(arrayOfBasicCrystals[3]);
                break;
        }
    }

    /// <summary>
    /// Purpose: Allows me to pick a crystal from another class without having to make a switch again.
    /// Restrictions:
    /// </summary>
    /// <param name="cardColor"></param>
    /// <returns>A crystal from the arrayOfCrystals</returns>
    public CrystalData GetACrystalBasedOnColor(CardColor cardColor)
    {
        switch (cardColor)
        {
            case CardColor.Red:
                return RedCrystal;
            case CardColor.Blue:
                return BlueCrystal;
            case CardColor.Green:
                return GreenCrystal;
            case CardColor.Purple:
                return PurpleCrystal;
        }

        print($"Failed to return a crystal.");
        return null;
    }
}
