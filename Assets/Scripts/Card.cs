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
        public enum Type {MonsterCard, ClassCard, Consumable }
        public Type type;
        public int level;
        public string cardName;
        public string cardText;
        public int spriteIcon;
        public Color backgroundColor;

        public int cost;

        //Permanent = -1, Until end of turn = 0. 
        public int cardDuration;

        public List<CardEffect> effects;
                
    }

    [Serializable]
    public class CardWrapper
    {
        public List<Card> cardItems;
    }
}