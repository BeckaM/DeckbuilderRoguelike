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
        public string CardName;
        public string CardText;
        public int SpriteIcon;

        public int Cost;

        //Permanent = -1, Until end of turn = 0. 
        public int CardDuration;

        public List<CardEffect> Effects;

        public void test() { }

    }

    [Serializable]
    public class CardWrapper
    {
        public List<Card> CardItems;
    }
}