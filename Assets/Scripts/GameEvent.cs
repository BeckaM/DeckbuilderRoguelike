using UnityEngine;
namespace Assets.Scripts
{
    public class GameEvent

    {

    }

    public class MoveCard_GUI : GameEvent
    {
        public CardManager movingCard { get; private set; }

        public MoveCard_GUI(CardManager movingCard)
        {
            this.movingCard = movingCard;

        }
    }

    public class DrawCard_Trigger : GameEvent
    {
        public CardgameManager.Team team { get; private set; }

        public DrawCard_Trigger(CardgameManager.Team team)
        {
            this.team = team;

        }
    }

    public class PlayCard_Trigger : GameEvent
    {
        public CardgameManager.Team team { get; private set; }

        public PlayCard_Trigger(CardgameManager.Team team)
        {
            this.team = team;

        }
    }

    public class UpdateDeckTexts_GUI : GameEvent
    {
        public int decktext { get; private set; }
        public int discardtext { get; private set; }
        public CardgameManager.Team team { get; private set; }


        public UpdateDeckTexts_GUI(int decktext, int discardtext, CardgameManager.Team team)
        {
            this.decktext = decktext;
            this.discardtext = discardtext;
            this.team = team;

        }
    }
   
    public class DealDamage_Trigger : GameEvent
    {
        public CardgameManager.Team team { get; private set; }


        public DealDamage_Trigger(CardgameManager.Team team)
        {
            this.team = team;

        }
    }
    
    public class Heal_Trigger : GameEvent
    {
        public CardgameManager.Team team { get; private set; }


        public Heal_Trigger(CardgameManager.Team team)
        {
            this.team = team;

        }
    }
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
        public CardgameManager.Team team { get; private set; }


        public UpdateLife_GUI(int health, CardgameManager.Team team)
        {
            this.life = health;
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

}
