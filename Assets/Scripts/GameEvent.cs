using UnityEngine;
namespace Assets.Scripts
{

    public class GameEvent

    {

    }



    public class MoveCardEvent : GameEvent
    {
        public GameObject movingCard { get; private set; }

        public MoveCardEvent(GameObject movingCard)
        {
            this.movingCard = movingCard;

        }
    }

    public class DrawCardEvent : GameEvent
    {
        public CardManager.Team team { get; private set; }

        public DrawCardEvent(CardManager.Team team)
        {
            this.team = team;

        }
    }

    public class UpdateDeckTexts : GameEvent
    {
        public int decktext { get; private set; }
        public int discardtext { get; private set; }
        public CardManager.Team team { get; private set; }


        public UpdateDeckTexts(int decktext, int discardtext, CardManager.Team team)
        {
            this.decktext = decktext;
            this.discardtext = discardtext;
            this.team = team;

        }
    }

}