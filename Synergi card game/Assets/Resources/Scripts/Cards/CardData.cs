using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


namespace CardBase
{
    public enum CardType
    {
        Being,
        Deployable,
        Tactic,
        Crystal
    }
    [CreateAssetMenu(menuName = "Cards/Generic Card")]
    public class CardData : ScriptableObject
    {
        protected CardType cardType;
        [SerializeField] protected string cardTitle;
        [SerializeField] protected CardColor cardColor;
        [SerializeField] protected int genericEnergyCost;
        [SerializeField] protected int blueEnergyCost;
        [SerializeField] protected int greenEnergyCost;
        [SerializeField] protected int redEnergyCost;
        [SerializeField] protected int purpleEnergyCost;
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

        public int GenericEnergyCost
        {
            get
            {
                return genericEnergyCost;
            }
            set
            {
                genericEnergyCost = value;
            }
        }
        public int RedEnergyCost
        {
            get
            {
                return redEnergyCost;
            }
            set
            {
                redEnergyCost = value;
            }
        }
        public int BlueEnergyCost
        {
            get
            {
                return blueEnergyCost;
            }
            set
            {
                blueEnergyCost = value;
            }
        }
        public int GreenEnergyCost
        {
            get
            {
                return greenEnergyCost;
            }
            set
            {
                greenEnergyCost = value;
            }
        }
        public int PurpleEnergyCost
        {
            get
            {
                return purpleEnergyCost;
            }
            set
            {
                purpleEnergyCost = value;
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

        public CardData(CardType cardType, CardColor cardColor,
                        int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost,
                        string cardTitle, string abilityText)
        {
            this.cardType = cardType;
            this.cardColor = cardColor;
            this.redEnergyCost = redEnergyCost;
            this.blueEnergyCost = blueEnergyCost;
            this.greenEnergyCost = greenEnergyCost;
            this.purpleEnergyCost = purpleEnergyCost;
            this.genericEnergyCost = genericEnergyCost;
            this.cardTitle = cardTitle;
            this.abilityText = abilityText;
        }

        //A constructor for the crystals
        public CardData(CardType cardType, CardColor cardColor, string cardTitle)
        {
            this.cardType = cardType;
            this.cardColor = cardColor;
            this.cardTitle = cardTitle;
        }

        //My DeFacto constructor. Unity's methods for creating a new being are not ideal.
        public void Init(CardType cardType, CardColor cardColor,
                         int redEnergyCost, int blueEnergyCost, int greenEnergyCost, int purpleEnergyCost, int genericEnergyCost, 
                         string cardTitle, string abilityText)
        {
            this.cardType = cardType;
            this.cardColor = cardColor;
            this.redEnergyCost = redEnergyCost;
            this.blueEnergyCost = blueEnergyCost;
            this.greenEnergyCost = greenEnergyCost;
            this.purpleEnergyCost = purpleEnergyCost;
            this.genericEnergyCost = genericEnergyCost;
            this.cardTitle = cardTitle;
            this.abilityText = abilityText;
        }

        //My DeFacto constructor. for crystals
        public void Init(CardType cardType, CardColor cardColor, string cardTitle)
        {
            this.cardType = cardType;
            this.cardColor = cardColor;
            this.cardTitle = cardTitle;
            this.redEnergyCost = 0;
            this.blueEnergyCost = 0;
            this.greenEnergyCost = 0;
            this.purpleEnergyCost = 0;
            this.genericEnergyCost = 0;
            abilityText = "";
        }
    }
}
