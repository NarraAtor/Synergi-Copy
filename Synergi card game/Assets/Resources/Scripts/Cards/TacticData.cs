using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;
namespace CardBase
{
    [CreateAssetMenu(menuName = "Cards/Tactic")]
    public class TacticData : CardData
    {
        //I don't think the SO needs data on other game objects
        //[SerializeField] protected GameObject TacticGrid;
        [SerializeField] protected TacticSubtypes subtype;
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

        public TacticData(CardColor cardColor, 
                          int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost, 
                          string cardTitle, string abilityText, TacticSubtypes subtype) :
        base(CardType.Tactic, cardColor, redEnergyCost, blueEnergyCost, greenEnergyCost, purpleEnergyCost, genericEnergyCost, cardTitle, abilityText)
        {
            this.subtype = subtype;
        }

        //My DeFacto constructor. Unity's methods for creating a new card are not ideal.
        public void Init(CardColor cardColor, 
                         int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost, 
                         string cardTitle, string abilityText, TacticSubtypes subtype)
        {
            base.Init(CardType.Tactic, cardColor, redEnergyCost, blueEnergyCost, greenEnergyCost, purpleEnergyCost, genericEnergyCost, cardTitle, abilityText);
            this.subtype = subtype;
        }
    }
}

