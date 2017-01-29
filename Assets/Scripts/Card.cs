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
        public int Damage;
        public enum Team { My, AI };
        public Team team = Team.My;
    }

    [Serializable]
    public class CardWrapper
    {
        public List<Card> CardItems;
    }
}