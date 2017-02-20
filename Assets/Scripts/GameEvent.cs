using UnityEngine;
namespace Assets.Scripts {

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
}