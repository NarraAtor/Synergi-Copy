using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardBase;

public enum Energy
{
    Red,
    Blue,
    Green,
    Purple
}
public class EnergySupplyManager : MonoBehaviour
{
    //Will be used later in the same matter as MtG's Mana pool.
    //private Energy[] energySupply;

    // public Energy[] EnergySupply
    // {
    //     get
    //     {
    //         return energySupply;
    //     }
    // }

    [SerializeField] private GameObject crystalUI;
    private List<Image> crystals = new List<Image>();
    private CrystalData[] crystalStorage = new CrystalData[10];
    public int TotalRedEnergy
    {
        get
        {
            int totalRedEnergy = 0;
            for(int i = 0; i < crystalStorage.Length; i++)
            {
                if(crystalStorage[i] is null)
                {
                    continue;
                }
                totalRedEnergy += crystalStorage[i].RedEnergyGiven;
            }

            return totalRedEnergy;
        }
    }
    public int TotalBlueEnergy
    {
        get
        {
            int totalBlueEnergy = 0;
            for (int i = 0; i < crystalStorage.Length; i++)
            {
                if (crystalStorage[i] is null)
                {
                    continue;
                }
                totalBlueEnergy += crystalStorage[i].BlueEnergyGiven;
            }

            return totalBlueEnergy;
        }
    }
    public int TotalGreenEnergy
    {
        get
        {
            int totalGreenEnergy = 0;
            for (int i = 0; i < crystalStorage.Length; i++)
            {
                if (crystalStorage[i] is null)
                {
                    continue;
                }
                totalGreenEnergy += crystalStorage[i].GreenEnergyGiven;
            }

            return totalGreenEnergy;
        }
    }
    public int TotalPurpleEnergy
    {
        get
        {
            int totalPurpleEnergy = 0;
            for (int i = 0; i < crystalStorage.Length; i++)
            {
                if (crystalStorage[i] is null)
                {
                    continue;
                }
                totalPurpleEnergy += crystalStorage[i].RedEnergyGiven;
            }

            return totalPurpleEnergy;
        }
    }

    public CrystalData[] CrystalStorage
    {
        get
        {
            return crystalStorage;
        }
    }



    //To be used later when its possible to ramp past 10 energy.
    // private List<Energy> bonusEnergySupply = new List<Energy>();

    void Init()
    {
        //energySupply = new Energy[10];

        foreach(RectTransform child in crystalUI.GetComponentInChildren<RectTransform>())
        {
            crystals.Add(child.GetComponent<Image>());
            child.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Purpose: Adds a crystal to the array of crystals.
    /// </summary>
    public void Add(CrystalData card)
    {
        //ResourceDeckManager calls this method in its start method so  I'll put some of the code that
        //would've gone in start in here.
        if(crystals.Count == 0)
        {
            Init();
        }


        for (int i = 0; i < crystalStorage.Length; i++)
        {
            if(crystalStorage[i] is null)
            {
                crystalStorage[i] = card;
                crystals[i].gameObject.SetActive(true);
                switch (card.CardColorProperty)
                {
                    case CardColor.Blue:
                        crystals[i].color = Color.cyan;
                        break;

                    case CardColor.Green:
                        crystals[i].color = Color.green;
                        break;

                    case CardColor.Purple:
                        crystals[i].color = Color.magenta;
                        break;

                    case CardColor.Red:
                        crystals[i].color = Color.red;
                        break;
                }
                
                break;
            }
        }
    }
}
