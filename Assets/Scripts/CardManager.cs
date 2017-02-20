using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace Assets.Scripts
{


    public class CardManager : MonoBehaviour
    {


        public Sprite[] sprites;
        public Card card;

        public enum CardStatus { InDeck, InHand, OnTable, InDiscard };
        public CardStatus cardStatus = CardStatus.InDeck;

        public enum Team { My, AI };
        public Team team = Team.My;

        public bool isPlayable = false;
        public bool isDragable = false;

        public float moveTime = 0.001f;
        private float inverseMoveTime;

        private GameObject startPoint;
        private GameObject endPoint;

        private Rigidbody2D rb2D;

        protected void Start()
        {

            //Get a component reference to this object's Rigidbody2D
            rb2D = GetComponent<Rigidbody2D>();

            //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
            inverseMoveTime = 1f / moveTime;

        }


        public void SetCardPosition(CardStatus status)
        {
            var oldstatus = cardStatus;
            cardStatus = status;

            if (oldstatus == CardStatus.InDeck)
            {
                startPoint = CardgameManager.instance.playerDeckCount.gameObject;
                endPoint = CardgameManager.instance.playerHand;

            }
            EventManager.Instance.AddListener<MoveCardEvent>(Move);

        }



        public void PopulateCard(Card card)
        {

            this.card = card;

            var transformer = this.transform;

            //Set Image
            var imageObj = transformer.GetChild(0);
            var imageComponent = imageObj.GetComponent<Image>();
            imageComponent.sprite = sprites[card.SpriteIcon];

            //Set Card Title
            var cardTitle = transformer.GetChild(1);
            var titleComponent = cardTitle.GetComponent<Text>();
            titleComponent.text = card.CardName;

            //Set Card Description
            var cardDesc = transformer.GetChild(2);
            var DescComponent = cardDesc.GetComponent<Text>();
            DescComponent.text = card.CardText;
        }

        internal void ApplyEffect(CardEffect cardEffect)
        {
            if (CardEffect.Effect.DealDamage.Equals(cardEffect.effect))
            {

                CardgameManager.instance.ApplyDamage(cardEffect.Value, team);

            }

            else if (CardEffect.Effect.Heal.Equals(cardEffect.effect))
            {

                CardgameManager.instance.ApplyHealing(cardEffect.Value, team);

            }


            //if effect is damage
            // deal "value"
            // trigger CardGameManager.DealDamage(Value)

            //else if is heal 

            // heal "value"
            //trigger CardGameManager.Heal(Value)

            //else if DrawCard
            //trigger CardGameManager.DrawCard(Value)


            StartCoroutine(EffectAnimation());



        }



        private IEnumerator EffectAnimation()
        {
            Color whateverColor = Color.black;
            for (var n = 0; n < 3; n++)
            {
                GetComponent<Image>().color = Color.white;
                yield return new WaitForSeconds(.1f);
                GetComponent<Image>().color = whateverColor;
                yield return new WaitForSeconds(.1f);
            }
            GetComponent<Image>().color = Color.white;



        }



        internal void Move(MoveCardEvent move)
        {
            if (move.movingCard == this.gameObject)
            {
                ////Store start position to move from, based on objects current transform position.
                transform.SetParent(startPoint.transform);

                Vector3 start = startPoint.transform.position;



                // Calculate end position based on the direction parameters passed in when calling Move.
               // transform.SetParent(endPoint.transform);
                Vector3 end = endPoint.transform.position;

                // transform.SetParent(startPoint.transform);
                this.transform.position = start;
                //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
                StartCoroutine(SmoothMovement(end));


            }
        }


        protected IEnumerator SmoothMovement(Vector3 end)
        {
            //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
            //Square magnitude is used instead of magnitude because it's computationally cheaper.
            float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //While that distance is greater than a very small amount (Epsilon, almost zero):
            while (sqrRemainingDistance > 0.001f)
            {
                var scaleRate = -0.02f;

                // transform.localScale += Vector3.one * scaleRate;

                //Find a new position proportionally closer to the end, based on the moveTime
                Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

                //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
                rb2D.MovePosition(newPostion);

                //Recalculate the remaining distance after moving.
                sqrRemainingDistance = (transform.position - end).sqrMagnitude;

                //Return and loop until sqrRemainingDistance is close enough to zero to end the function
                yield return null;

            }
            EventManager.Instance.RemoveListener<MoveCardEvent>(Move);
            transform.SetParent(endPoint.transform);
            EventManager.Instance.processingQueue = false;

        }
    }
}

