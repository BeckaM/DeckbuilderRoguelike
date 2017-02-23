using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{


    public class CardManager : MonoBehaviour
    {


        public Sprite[] sprites;
        public Card card;
        public GameObject cardDescription;
        public GameObject cardImage;
        public GameObject cardName;
        public GameObject cardEffectText;

        public enum CardStatus { InDeck, InHand, OnTable, InDiscard };
        public CardStatus cardStatus = CardStatus.InDeck;

        public CardgameManager.Team owner;

        public bool showDescription
        {
            get
            {
                if (cardStatus == CardStatus.InHand)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public DeckManager deckManager
        {
            get
            {
                if (owner == CardgameManager.Team.My)
                {
                    return DeckManager.player;
                }
                else
                {
                    return DeckManager.monster;
                }
            }
        }


        public bool isPlayable
        {
            get
            {
                if (CardgameManager.instance.turn == owner && CardgameManager.instance.player.mana >= card.cost && cardStatus == CardStatus.InHand)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isDragable
        {
            get
            {
                if (owner == CardgameManager.Team.My && cardStatus == CardStatus.InHand)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public float moveTime = 1f;
        private float inverseMoveTime;

        public GameObject startPoint;
        public GameObject endPoint;

        //private Vector3 startSize;
        //private Vector3 endSize;


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
            cardStatus = status;
            EventManager.Instance.AddListener<MoveCard_GUI>(Move);
        }

        public void PopulateCard(Card card)
        {

            this.card = card;

            var transformer = this.transform;

            //Set Image            
            var imageComponent = cardImage.GetComponent<Image>();
            imageComponent.sprite = sprites[card.spriteIcon];

            //Set Card Title

            var titleComponent = cardName.GetComponent<Text>();
            titleComponent.text = card.cardName;

            //Set Card Description
            var cardtext = cardDescription.GetComponent<Text>();
            cardtext.text = card.cardText;

            //Set Card Background.
            var background = GetComponent<Image>();
            background.color = card.backgroundColor;

        }

        internal void ApplyEffect(CardEffect cardEffect)
        {
            if (CardEffect.Effect.DealDamage.Equals(cardEffect.effect))
            {
                EventManager.Instance.AddListener<DealDamage_GUI>(DealDamageAnimation);
                EventManager.Instance.QueueAnimation(new DealDamage_GUI(cardEffect.value, owner, this));

                CardgameManager.instance.ApplyDamage(cardEffect.value, owner);


                EventManager.Instance.QueueTrigger(new DealDamage_Trigger(owner));

            }

            else if (CardEffect.Effect.Heal.Equals(cardEffect.effect))
            {

                EventManager.Instance.QueueAnimation(new Heal_GUI(cardEffect.value, owner, this));

                CardgameManager.instance.ApplyHealing(cardEffect.value, owner);


                EventManager.Instance.QueueTrigger(new Heal_Trigger(owner));

            }


            //if effect is damage
            // deal "value"
            // trigger CardGameManager.DealDamage(Value)

            //else if is heal 

            // heal "value"
            //trigger CardGameManager.Heal(Value)

            //else if DrawCard
            //trigger CardGameManager.DrawCard(Value)           
        }



        private void DealDamageAnimation(DealDamage_GUI damage)
        {
            var text = cardEffectText.GetComponent<Text>();


            text.text = "" + damage.damage + " Damage!";

            StartCoroutine(EffectText(Color.red));

        }

        private IEnumerator EffectText(Color color)
        {
            var text = cardEffectText.GetComponent<Text>();
            text.color = color;
            cardEffectText.SetActive(true);

            for (var n = 0; n < 3; n++)
            {
               text.color = Color.white;
                yield return new WaitForSeconds(.1f);
                text.color = color;
                yield return new WaitForSeconds(.1f);
            }

            cardEffectText.SetActive(false);

            //Color whateverColor = Color.black;
            //for (var n = 0; n < 3; n++)
            //{
            //    GetComponent<Image>().color = Color.white;
            //    yield return new WaitForSeconds(.1f);
            //    GetComponent<Image>().color = whateverColor;
            //    yield return new WaitForSeconds(.1f);
            //}
            //GetComponent<Image>().color = Color.white;
            EventManager.Instance.processingQueue = false;
        }

        internal void Move(MoveCard_GUI move)
        {
            if (move.movingCard == this)
            {
                Debug.Log("Card " + card.cardName + " recieved a move trigger");

                StartCoroutine(SmoothMovement());


            }
        }

        protected IEnumerator SmoothMovement()
        {
            Debug.Log("Starting move card animation");
            Vector3 endpos = new Vector3();
            // Vector3 endsize = new Vector3();

            transform.SetParent(startPoint.transform);
            this.transform.localScale = Vector3.one;
            yield return new WaitForEndOfFrame();
            Vector3 startpos = new Vector3();
            startpos = this.transform.position;

            //if we have no cards at this position, make a test place and get the new position from that.
            if (endPoint.transform.childCount == 0)
            {
                this.GetComponent<CanvasGroup>().alpha = (0f);
                transform.SetParent(endPoint.transform);
                //this.transform.localScale = Vector3.one;
                //transform.SetParent(endPoint.transform);
                yield return new WaitForEndOfFrame();
                //endsize = this.transform.localScale;
                endpos = this.transform.position;
            }

            //If we already have a card at that position, place it where the last card is.
            else
            {

                this.GetComponent<CanvasGroup>().alpha = (0f);
                var lastChild = endPoint.transform.GetChild(endPoint.transform.childCount - 1);
                endpos = lastChild.transform.position;
                //  endsize = lastChild.transform.localScale;

            }
            this.transform.SetParent(startPoint.transform);
            yield return new WaitForEndOfFrame();


            GetComponent<CanvasGroup>().alpha = (1f);
            //  yield return new WaitForSeconds(1f);

            //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
            //Square magnitude is used instead of magnitude because it's computationally cheaper.
            float sqrRemainingDistance = (transform.position - endpos).sqrMagnitude;

            //While that distance is greater than a very small amount (Epsilon, almost zero):
            while (sqrRemainingDistance > 0.001f)
            {
                //    var scaleRate = 0.02f;

                //    if (startSize.x < endsize.x && transform.localScale.x < endsize.x)
                //    {
                //        transform.localScale += endsize * scaleRate;
                //    }
                //    else if (startSize.x > endsize.x && transform.localScale.x > endsize.x)
                //    {

                //        transform.localScale -= endsize * scaleRate;

                //    }

                //Find a new position proportionally closer to the end, based on the moveTime
                Vector3 newPostion = Vector3.MoveTowards(rb2D.position, endpos, inverseMoveTime * Time.deltaTime);



                //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
                rb2D.MovePosition(newPostion);

                //Recalculate the remaining distance after moving.
                sqrRemainingDistance = (transform.position - endpos).sqrMagnitude;

                //Return and loop until sqrRemainingDistance is close enough to zero to end the function
                yield return null;

            }


            //this.transform.localScale = endsize;
            EventManager.Instance.RemoveListener<MoveCard_GUI>(Move);
            //yield return new WaitForSeconds(3f);

            if (cardStatus == CardStatus.InDiscard)
            {
                transform.SetParent(deckManager.transform);
            }
            else
            {
                transform.SetParent(endPoint.transform);
            }

            if (showDescription)
            {
                cardDescription.SetActive(true);
            }
            else
            {
                cardDescription.SetActive(false);
            }

            yield return new WaitForSeconds(0.3f);
            EventManager.Instance.processingQueue = false;



        }
    }
}

