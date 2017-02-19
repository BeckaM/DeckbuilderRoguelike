using UnityEngine;
namespace Assets.Scripts {

    public class GameEvent

    {

    }



    public class MoveCardEvent : GameEvent
    {
        public GameObject movingCard { get; private set; }
        public GameObject startPoint { get; private set; }
        public GameObject endPoint { get; private set; }

        public MoveCardEvent(GameObject movingCard, GameObject startPoint, GameObject endPoint)
        {
            this.movingCard = movingCard;
            this.startPoint = startPoint;
            this.endPoint = endPoint;
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
}