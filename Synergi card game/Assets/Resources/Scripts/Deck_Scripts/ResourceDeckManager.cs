using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;

public class ResourceDeckManager : MonoBehaviour
{
    private Stack<CrystalData> resourceDeckArray = new Stack<CrystalData>(10);
    [SerializeField] private GameObject energySupply;
    [SerializeField] private GameObject cardDatabase;

    //Temporary list of hard coded crystals
    private List<CrystalData> listOfBasicCrystals;

    //I can't exactly picture how a user will pick their own deck, so I'll just create a prototype-deck with hard coded data.
    private void Start()
    {
        listOfBasicCrystals = new List<CrystalData>();
        listOfBasicCrystals.Add(Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Blue Crystal"));
        listOfBasicCrystals.Add(Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Green Crystal"));
        listOfBasicCrystals.Add(Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Purple Crystal"));
        listOfBasicCrystals.Add(Resources.Load<CrystalData>("Scripts/Cards/List of Cards/Crystals/Red Crystal"));

        MakeResourceDeck();

        Draw();
        Draw();
        Draw();
        Draw();
    }

    /// <summary>
    /// Purpose: Makes a 10 card resource deck. Is hard coded for now.
    ///          TODO: Make cards chosen from the card database form the deck instead.
    /// </summary>
    private void MakeResourceDeck()
    {
        for(int i = 0; i < 10; i++)
        {
            resourceDeckArray.Push(listOfBasicCrystals[i % 4]);
        }
    }

    /// <summary>
    /// Purpose: Draws a card from the resource deck and adds it to the player's Energy Supply.
    /// </summary>
    public void Draw()
    {
        energySupply.GetComponent<EnergySupplyManager>().Add(resourceDeckArray.Pop());
    }
}
