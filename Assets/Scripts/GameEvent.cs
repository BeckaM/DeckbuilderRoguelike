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
    public class UpdateManaText_GUI : GameEvent
    {
        public int mana { get; private set; }        
        public CardgameManager.Team team { get; private set; }


        public UpdateManaText_GUI(int mana, CardgameManager.Team team)
        {
            this.mana = mana;            
            this.team = team;

        }
    }

}