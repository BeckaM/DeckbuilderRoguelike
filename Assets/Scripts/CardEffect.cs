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

        public enum Effect { DealDamage, Heal, DrawCard, DiscardCard, ReduceDamage, IncreaseDamage, AddMaxMana, SelfDamage, SelfDiscard };
        public Effect effect;
        public int value;

        public enum Trigger { Instant, Passive, StartOfTurn, EndOfTurn, OnTakeDamage, OnDealDamage, OnDraw, OnPlayCard, OnHeal, OnExpire };
        public Trigger trigger;
        public CardgameManager.Team triggeredBy;

    }

}