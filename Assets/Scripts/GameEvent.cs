using UnityEngine;
namespace Assets.Scripts
{

    public class GameEvent

    {

    }



    public class MoveCard_AnimEvent : GameEvent
    {
        public GameObject movingCard { get; private set; }

        public MoveCard_AnimEvent(GameObject movingCard)
        {
            this.movingCard = movingCard;

        }
    }

    public class DrawCard_TriggEvent : GameEvent
    {
        public CardgameManager.Team team { get; private set; }

        public DrawCard_TriggEvent(CardgameManager.Team team)
        {
            this.team = team;

        }
    }

    public class UpdateDeckTexts_AnimEvent : GameEvent
    {
        public int decktext { get; private set; }
        public int discardtext { get; private set; }
        public CardgameManager.Team team { get; private set; }


        public UpdateDeckTexts_AnimEvent(int decktext, int discardtext, CardgameManager.Team team)
        {
            this.decktext = decktext;
            this.discardtext = discardtext;
            this.team = team;

        }
    }

}