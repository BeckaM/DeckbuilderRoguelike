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

        public enum Effect { DealDamage, DrawCard, ReduceDamage, Heal, AddMaxMana };
        public Effect effect;

        public int value;

        public enum Trigger { Instant, Passive, StartOfTurn, EndOfTurn, OnTakeDamage, OnDealDamage, OnDraw, OnPlayCard };
        public Trigger trigger;

    }

}