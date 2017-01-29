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
       
        public enum Team { My, AI };
        public Team team = Team.My;
        public int Cost;

        public enum CardType { Instant, Aura };
        public CardType cardtype;
        public int Damage;
        public int Heal;
        public int Draw;
        public int Armor;

        public enum Trigger { None, StartofMy, StartofAI, EndofMy, EndofAI, OnDraw, OnMyPlay, OnAIPlay, OnMyDamage, OnAIDamage };
        public Trigger trigger;




    }

    [Serializable]
    public class CardWrapper
    {
        public List<Card> CardItems;
    }
}