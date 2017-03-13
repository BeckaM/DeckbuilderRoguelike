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
        public enum Type { MonsterCard, ClassCard, Consumable }
        public Type type;
        public int level;
        public int cost;

        public List<CardEffect> effects;

        //Permanent = -1, Until end of turn = 0. 
        public int cardDuration;


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