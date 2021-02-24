using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;

namespace CardBase
{
    [CreateAssetMenu(menuName = "Cards/Crystal")]
    public class CrystalData : CardData
    {
        [SerializeField] protected int redEnergyGiven { get; set; }
        [SerializeField] protected int blueEnergyGiven { get; set; }
        [SerializeField] protected int greenEnergyGiven { get; set; }
        [SerializeField] protected int purpleEnergyGiven { get; set; }
        [SerializeField] protected int genericEnergyGiven { get; set; }
        // Start is called before the first frame update
        void Start()
        {

        }

        public CrystalData(CardColor cardColor, string cardTitle)
            : base(CardType.Crystal, cardColor, cardTitle)
        {
            this.Init(cardColor, cardTitle);
        }

        //My DeFacto constructor. Unity's methods for creating a new being are not ideal.
        public void Init(CardColor cardColor, string cardTitle)
        {
            base.Init(CardType.Crystal, cardColor, energyCost, cardTitle, abilityText);
        }

    }
}

