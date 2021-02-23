using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardBase;

namespace CardBase
{
    [CreateAssetMenu(menuName = "Cards/Deployable")]
    public class DeployableData : CardData
    {
        [SerializeField] protected int durability;
        [SerializeField] protected string subtype;
        public int Durability
        {
            get
            {
                return durability;
            }
            set
            {
                durability = value;
            }
        }

        public string Subtype
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
        public DeployableData(CardColor cardColor, int energyCost, string cardTitle, string abilityText, int durability) :
        base(CardType.Deployable, cardColor, energyCost, cardTitle, abilityText)
        {
            this.durability = durability;
        }
        //My DeFacto constructor. Unity's methods for creating a new card are not ideal.
        public void Init(CardColor cardColor, int energyCost, string cardTitle, string abilityText, int durability)
        {
            base.Init(CardType.Deployable, cardColor, energyCost, cardTitle, abilityText);
            this.durability = durability;
        }
    }

}
