using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts
{
    public class GameEvent

    {

    }

    public class MoveCard_GUI : GameEvent
    {
        public CardManager movingCard { get; private set; }
        public GameObject start { get; private set; }
        public GameObject end { get; private set; }

        public MoveCard_GUI(CardManager movingCard, GameObject start, GameObject end)
        {
            this.movingCard = movingCard;
            this.start = start;
            this.end = end;
        }
    }

    public class EndGame_GUI : GameEvent
    {
        public bool playerWon { get; private set; }
        public List<Card> cardRewards { get; private set; }
        public int goldReward { get; private set; }

        public EndGame_GUI(bool playerWon, List<Card> cardRewards, int goldReward)
        {
            this.playerWon = playerWon;
            this.cardRewards = cardRewards;
            this.goldReward = goldReward;
        }
    }

    //public class UpdateDeckTexts_GUI : GameEvent
    //{
    //    public int decktext { get; private set; }
    //    public int discardtext { get; private set; }
    //    public CardgameManager.Team team { get; private set; }

    //    public UpdateDeckTexts_GUI(int decktext, int discardtext, CardgameManager.Team team)
    //    {
    //        this.decktext = decktext;
    //        this.discardtext = discardtext;
    //        this.team = team;

    //    }
    //}

    public class UpdateMana_GUI : GameEvent
    {
        public int mana { get; private set; }
        public int maxMana { get; private set; }
        public CardgameManager.Team team { get; private set; }

        public UpdateMana_GUI(int mana, int maxMana, CardgameManager.Team team)
        {
            this.mana = mana;
            this.maxMana = maxMana;
            this.team = team;

        }
    }


    public class UpdateLife_GUI : GameEvent
    {
        public int life { get; private set; }
        public int maxLife { get; private set; }
        public CardgameManager.Team team { get; private set; }

        public UpdateLife_GUI(int life, int maxLife, CardgameManager.Team team)
        {
            this.life = life;
            this.maxLife = maxLife;
            this.team = team;

        }
    }

    public class UpdateArmor_GUI : GameEvent
    {
        public int armor { get; private set; }        
        public CardgameManager.Team team { get; private set; }

        public UpdateArmor_GUI(int armor, CardgameManager.Team team)
        {
            this.armor = armor;            
            this.team = team;

        }
    }

    public class UpdateDamageIncrease_GUI : GameEvent
    {
        public int boost { get; private set; }
        public CardgameManager.Team team { get; private set; }

        public UpdateDamageIncrease_GUI(int boost, CardgameManager.Team team)
        {
            this.boost = boost;
            this.team = team;

        }
    }

    public class ApplyDamage_GUI : GameEvent
    {
        public int damage { get; private set; }       
        public CardgameManager.Team team { get; private set; }

        public ApplyDamage_GUI(int damage, CardgameManager.Team team)
        {
            this.damage = damage;           
            this.team = team;
        }
    }


    public class CardEffect_GUI : GameEvent
    {
        public int value { get; private set; }
        public CardEffect.Effect type { get; private set; }
        public CardgameManager.Team team { get; private set; }
        public CardManager card { get; private set; }

        public CardEffect_GUI(int value, CardgameManager.Team team, CardManager card, CardEffect.Effect type)
        {

            this.card = card;
            this.value = value;
            this.team = team;
            this.type = type;

        }
    }

    public class DestroyCard_GUI : GameEvent
    {
        public CardManager card { get; private set; }

        public DestroyCard_GUI(CardManager card)
        {
            this.card = card;
        }
    }

    public class TableCard_Trigger : GameEvent
    {
        public CardgameManager.Team team { get; private set; }
        public CardEffect.Trigger effect { get; private set; }

        public TableCard_Trigger(CardgameManager.Team team, CardEffect.Trigger effect)
        {
            this.team = team;
            this.effect = effect;

        }
    }

}
