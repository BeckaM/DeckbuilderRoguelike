using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Card
    {
        public string cardName;

        //Card Stats
        public enum Type {ManaSource, BasicAttack, Utility, Consumable}
        public Type type;        
        public PlayerClass.ClassName classOwner;
                
        public int level;
        public int cost;

        public List<CardEffect> effects;

        public int cardDuration;      //-1 = permanent, 0 = instant 

        //Card Appearance
        public string cardText;
        public int spriteIcon;
        public Color spriteColor;
        public Color spriteHighlightColor;
        public Color spriteGlowColor;
        public Color spriteBackgroundColor;
        public Color backgroundGlowColor;

        public Color backgroundColor;
    }


    [Serializable]
    public class CardWrapper
    {
        public List<Card> cardItems;
    }
}