using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;


namespace CardBase
{
    /// <summary>
    /// Purpose: A class of SOs devoted to crystals. Crystals are basically this game's version of lands.
    ///          Since these cards are a resource card, they will all cost 0 energy. Some may have abilities in the future,
    ///          but for now they'll have no abilities either.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Crystal")]
    public class CrystalData : CardData
    {
        [SerializeField] protected int redEnergyGiven;
        [SerializeField] protected int blueEnergyGiven;
        [SerializeField] protected int greenEnergyGiven;
        [SerializeField] protected int purpleEnergyGiven;
        [SerializeField] protected int genericEnergyGiven;

        public int RedEnergyGiven
        {
            get
            {
                return redEnergyGiven;
            }
        }
        public int BlueEnergyGiven
        {
            get
            {
                return blueEnergyGiven;
            }
        }
        public int GreenEnergyGiven
        {
            get
            {
                return greenEnergyGiven;
            }
        }
        public int PurpleEnergyGiven
        {
            get
            {
                return purpleEnergyGiven;
            }
        }
        public int GenericEnergyGiven
        {
            get
            {
                return genericEnergyGiven;
            }
        }

        public CrystalData(CardColor cardColor, string cardTitle)
            : base(CardType.Crystal, cardColor, cardTitle)
        {
            this.Init(cardColor, cardTitle);
        }

        //My DeFacto constructor. Unity's methods for creating a new being are not ideal.
        public void Init(CardColor cardColor, string cardTitle)
        {
            base.Init(CardType.Crystal, cardColor, cardTitle);
        }

    }
}

