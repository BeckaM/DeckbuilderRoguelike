using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class CardEffect 
    {

        public enum Effect { DealDamage, DrawCard, ReduceDamage, Heal };
        public Effect effect;

        public int Value;

        public enum Trigger { Instant, Passive, StartOfTurn, EndOfTurn, OnTakeDamage, OnDealDamage, OnDraw, OnPlayCard };
        public Trigger trigger;

    }

}