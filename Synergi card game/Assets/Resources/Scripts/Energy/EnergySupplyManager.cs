using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardBase;
using Mirror;

public enum Energy
{
    Red,
    Blue,
    Green,
    Purple
}
/// <summary>
/// Author: Eric Fotang
/// Purpose: Manages this player's energy, this game's version of mana.
///          Each client has 2 versions of this, one for the player and one for the enemy.
/// Restrictions:
/// </summary>
public class EnergySupplyManager : NetworkBehaviour
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

    //This will be empty for the enemy portrait
    [SerializeField] private GameObject enemyPortrait;
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
            //child.gameObject.SetActive(false);
        }

        //Mess with the order of the crystals so that progression goes from left to right, then down.
        List<Image> temporaryCrystalList = new List<Image>();
        temporaryCrystalList.Add(crystals[0]);
        temporaryCrystalList.Add(crystals[2]);
        temporaryCrystalList.Add(crystals[4]);
        temporaryCrystalList.Add(crystals[6]);
        temporaryCrystalList.Add(crystals[8]);
        temporaryCrystalList.Add(crystals[1]);
        temporaryCrystalList.Add(crystals[3]);
        temporaryCrystalList.Add(crystals[5]);
        temporaryCrystalList.Add(crystals[7]);
        temporaryCrystalList.Add(crystals[9]);
        crystals = temporaryCrystalList;
    }

    private void Update()
    {
        
    }
    /// <summary>
    /// Purpose: Adds a crystal to the array of crystals and broadcaasts the added crystal across the network.
    /// </summary>
    public void Add(CrystalData card)
    {
        //ResourceDeckManager calls this method in its start method so I'll put some of the code that
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
                //crystals[i].gameObject.SetActive(true);
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

        //Send a message to the other player to do show the same info
        if(isServer)
        {
            CmdAdd(card.CardColorProperty, true);
        }
        else if(isClientOnly)
        {
            CmdAdd(card.CardColorProperty, false);
        }
    }

    public void Add(CardColor cardColor)
    {
        if (crystals.Count == 0)
        {
            Init();
        }


        for (int i = 0; i < crystalStorage.Length; i++)
        {
            if (crystalStorage[i] is null)
            {
                crystalStorage[i] = card;
                //crystals[i].gameObject.SetActive(true);
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
    /// <summary>
    /// Purpose: Tells the server to tell clients to add a crystal to their crystal supply.
    /// Restrictions:
    /// </summary>
    /// <param name="crystalCardColor">the crystal to add to the player's energy supply.</param>
    /// <param name="sentFromServer">whether or not this was sent from the server.</param>
    [Command(requiresAuthority = false)]
    private void CmdAdd(CardColor crystalCardColor, bool sentFromServer)
    {
        RpcAdd(crystalCardColor, sentFromServer);
    }
    /// <summary>
    /// Purpose: Sends a message to all clients to update the opposing resource system UI.
    /// Restrictions: Must be use in a command to reach all clients 
    ///               (doesn't naturally work from client to server).
    /// </summary>
    /// <param name="crystalCardColor">the crystal to add to the user's energy supply.</param>
    /// <param name="sentFromServer">whether or not this method was called from the server</param>
    [ClientRpc(includeOwner = true)]
    private void RpcAdd(CardColor crystalCardColor, bool sentFromServer)
    {
        if(sentFromServer && isServer ||
           !sentFromServer && isClientOnly)
        {
            return;
        }
        print($"Client RPC called. Sent from server: {sentFromServer}");
        enemyPortrait.GetComponent<EnergySupplyManager>().Add(crystalCardColor);
    }
}
