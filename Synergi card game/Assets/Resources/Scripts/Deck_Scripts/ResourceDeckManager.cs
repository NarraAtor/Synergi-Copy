using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;

public class ResourceDeckManager : MonoBehaviour
{
    //private Stack<CrystalData> resourceDeckArray = new Stack<CrystalData>(10);
    [SerializeField] private GameObject energySupply;
    [SerializeField] private GameObject cardDatabase;

    //Temporary list of hard coded crystals
    //4/30 No longer temporary. Players will add a copy of a crystal to their supply at the start of their turn.
    private List<CrystalData> listOfBasicCrystals;

    //I can't exactly picture how a user will pick their own deck, so I'll just create a prototype-deck with hard coded data.
    //No longer deck, now just an array of crystals.
    private void Start()
    {
        listOfBasicCrystals = new List<CrystalData>();
        listOfBasicCrystals.Add(Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Blue Crystal"));
        listOfBasicCrystals.Add(Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Green Crystal"));
        listOfBasicCrystals.Add(Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Purple Crystal"));
        listOfBasicCrystals.Add(Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Red Crystal"));

        //MakeResourceDeck();

        //Draw();
        //Draw();
        //Draw();
        //Draw();

        AddCrystalToSupply(CardColor.Blue);
        AddCrystalToSupply(CardColor.Green);
        AddCrystalToSupply(CardColor.Purple);
        AddCrystalToSupply(CardColor.Red);
    }

    /// <summary>
    /// Purpose: Makes a 10 card resource deck. Is hard coded for now.
    ///          TODO: Make cards chosen from the card database form the deck instead.
    /// </summary>
    private void MakeResourceDeck()
    {
        for(int i = 0; i < 10; i++)
        {
            //resourceDeckArray.Push(listOfBasicCrystals[i % 4]);
        }
    }

    /// <summary>
    /// Purpose: Draws a card from the resource deck and adds it to the player's Energy Supply.
    /// </summary>
    public void Draw()
    {
        //energySupply.GetComponent<EnergySupplyManager>().Add(resourceDeckArray.Pop());
    }

    public void AddCrystalToSupply(CardColor color)
    {
        switch(color)
        {
            case CardColor.Blue:
                energySupply.GetComponent<EnergySupplyManager>().Add(listOfBasicCrystals[0]);
                break;

            case CardColor.Green:
                energySupply.GetComponent<EnergySupplyManager>().Add(listOfBasicCrystals[1]);
                break;

            case CardColor.Purple:
                energySupply.GetComponent<EnergySupplyManager>().Add(listOfBasicCrystals[2]);
                break;

            case CardColor.Red:
                energySupply.GetComponent<EnergySupplyManager>().Add(listOfBasicCrystals[3]);
                break;
        }
    }
}
