using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBase
{
    public enum CardType
    {
        Being,
        Deployable,
        Tactic
    }
    [CreateAssetMenu(menuName = "Cards/Generic Card")]
    public class CardData : ScriptableObject
    {
        protected CardType cardType;
        [SerializeField] protected string cardTitle;
        [SerializeField] protected CardColor cardColor;
        [SerializeField] protected int energyCost;
        [SerializeField] protected string abilityText;
        //public Sprite cardImage;


        public string CardTitle
        {
            get
            {
                return cardTitle;
            }
            set
            {
                cardTitle = value;
            }
        }

        public CardColor CardColorProperty
        {
            get
            {
                return cardColor;
            }
        }

        public int EnergyCost
        {
            get
            {
                return energyCost;
            }
            set
            {
                energyCost = value;
            }
        }

        public string AbilityText
        {
            get
            {
                return abilityText;
            }
            set
            {
                abilityText = value;
            }
        }

        public CardType CardTypeProperty
        {
            get
            {
                return cardType;
            }
        }

        public CardData(CardType cardType, CardColor cardColor, int energyCost, string cardTitle, string abilityText)
        {
            this.cardType = cardType;
            this.cardColor = cardColor;
            this.energyCost = energyCost;
            this.cardTitle = cardTitle;
            this.abilityText = abilityText;
        }

        //My DeFacto constructor. Unity's methods for creating a new being are not ideal.
        public void Init(CardType cardType, CardColor cardColor, int energyCost, string cardTitle, string abilityText)
        {
            this.cardType = cardType;
            this.cardColor = cardColor;
            this.energyCost = energyCost;
            this.cardTitle = cardTitle;
            this.abilityText = abilityText;
        }
    }
}
