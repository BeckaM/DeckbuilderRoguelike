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
        public int duration;

        public enum CardStatus { InDeck, InHand, OnTable, InDiscard };
        public CardStatus cardStatus = CardStatus.InDeck;

        public CardgameManager.Team owner;
                
        public DeckManager deckManager
        {
            get
            {
                if (owner == CardgameManager.Team.Me)
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
                if (owner == CardgameManager.Team.Me && cardStatus == CardStatus.InHand)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public GameObject table
        {
            get
            {
                if (owner == CardgameManager.Team.Me )
                {
                    return CardgameManager.instance.playerTable;
                }
                else
                {
                    return CardgameManager.instance.monsterTable;
                }
            }
        }

        public GameObject discard
        {
            get
            {
                if (owner == CardgameManager.Team.Me)
                {
                    return CardgameManager.instance.playerDiscard;
                }
                else
                {
                    return CardgameManager.instance.monsterDiscard;
                }
            }
        }

        public float moveTime = 1f;
        private float inverseMoveTime;

        //public GameObject startPoint;
        //public GameObject endPoint;

        //private Vector3 startSize;
        //private Vector3 endSize;
        public int effectCounter;
        public int moveCounter;

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
            moveCounter++;

            if (status == CardStatus.OnTable)
            {
                EventManager.Instance.AddListener<TableCard_Trigger>(CardTrigger);
            }

        }

        private void CardTrigger(TableCard_Trigger trigger)
        {
            foreach (CardEffect cardeffect in card.effects)
            {
                if (trigger.effect == cardeffect.trigger && trigger.team == cardeffect.triggeredBy)
                {
                    
                    ApplyEffect(cardeffect);
                }
            }

            if(trigger.effect == CardEffect.Trigger.EndOfTurn &&  trigger.team == owner)
            {
                Debug.Log(card.cardName);
                duration--;
               // card.cardName = card.cardName + "second";
                if( duration == 0)
                {
                    ExpireCard();
                }
            }

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
            duration = card.cardDuration;

        }

        internal void ApplyEffect(CardEffect cardEffect)
        {
            EventManager.Instance.AddListener<CardEffect_GUI>(CardEffectAnimation);
            effectCounter++;

            EventManager.Instance.QueueAnimation(new CardEffect_GUI(cardEffect.value, owner, this, cardEffect.effect));

            if (CardEffect.Effect.DealDamage.Equals(cardEffect.effect))
            {
                CardgameManager.instance.ApplyDamage(cardEffect.value, owner);

                EventManager.Instance.TriggerEvent(new TableCard_Trigger(owner, CardEffect.Trigger.OnDealDamage));
            }

            else if (CardEffect.Effect.Heal.Equals(cardEffect.effect))
            {
                CardgameManager.instance.ApplyHealing(cardEffect.value, owner);

                EventManager.Instance.TriggerEvent(new TableCard_Trigger(owner, CardEffect.Trigger.OnHeal));
            }

            else if (CardEffect.Effect.AddMaxMana.Equals(cardEffect.effect))
            {
                CardgameManager.instance.IncreaseMaxMana(cardEffect.value, owner);
            }

            else if (CardEffect.Effect.ReduceDamage.Equals(cardEffect.effect))
            {
                CardgameManager.instance.IncreaseDamageReduction(cardEffect.value, owner);
            }
            else
            {
                Debug.LogError("Card Effct not Implemented yet!");
            }
        }

        public void CardEffectAnimation(CardEffect_GUI e)
        {
            if (e.card == this)
            {
                if (e.type == CardEffect.Effect.AddMaxMana)
                {
                    var text = cardEffectText.GetComponent<Text>();

                    text.text = "Max Mana Increased +" + e.value + "!";

                    StartCoroutine(EffectText(Color.blue));
                }
                else if (e.type == CardEffect.Effect.DealDamage)
                {
                    var text = cardEffectText.GetComponent<Text>();

                    text.text = "" + e.value + " Damage!";

                    StartCoroutine(EffectText(Color.red));
                }
                else if (e.type == CardEffect.Effect.Heal)
                {
                    var text = cardEffectText.GetComponent<Text>();

                    text.text = "" + e.value + " Life Restored!";

                    StartCoroutine(EffectText(Color.green));
                }
                else if (e.type == CardEffect.Effect.ReduceDamage)
                {
                    var text = cardEffectText.GetComponent<Text>();

                    text.text = "+" + e.value + " Damage Reduction!";

                    StartCoroutine(EffectText(Color.grey));
                }
            }
        }

        private IEnumerator EffectText(Color color)
        {
            var text = cardEffectText.GetComponent<Text>();
            text.color = color;
            cardEffectText.SetActive(true);

            for (var n = 0; n < 5; n++)
            {
                text.color = Color.white;
                yield return new WaitForSeconds(.1f);
                text.color = color;
                yield return new WaitForSeconds(.1f);
            }

            cardEffectText.SetActive(false);

            effectCounter--;
            if (effectCounter == 0)
            {
                EventManager.Instance.RemoveListener<CardEffect_GUI>(CardEffectAnimation);
            }
            EventManager.Instance.processingQueue = false;
        }

        internal void Move(MoveCard_GUI move)
        {
            if (move.movingCard == this)
            {
                Debug.Log("Card " + card.cardName + " recieved a move trigger");
                                
                StartCoroutine(SmoothMovement(move.start, move.end));


            }
        }

        protected IEnumerator SmoothMovement(GameObject start, GameObject end)
        {
            Debug.Log("Starting move card animation");
            Vector3 endpos = new Vector3();
            // Vector3 endsize = new Vector3();

            transform.SetParent(start.transform);
            this.transform.localScale = Vector3.one;
            yield return new WaitForEndOfFrame();
            Vector3 startpos = new Vector3();
            startpos = this.transform.position;

            //if we have no cards at this position, make a test place and get the new position from that.
            if (end.transform.childCount == 0)
            {
                this.GetComponent<CanvasGroup>().alpha = (0f);
                transform.SetParent(end.transform);
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
                var lastChild = end.transform.GetChild(end.transform.childCount - 1);
                endpos = lastChild.transform.position;
                //  endsize = lastChild.transform.localScale;

            }
            this.transform.SetParent(start.transform);
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


            moveCounter--;
            if (moveCounter == 0)
            {
                EventManager.Instance.RemoveListener<MoveCard_GUI>(Move);
            }
           
            //this.transform.localScale = endsize;
            
            //yield return new WaitForSeconds(3f);

            if (end == discard)
            {
                transform.SetParent(deckManager.transform);
            }
            else
            {
                transform.SetParent(end.transform);
            }

            if (end == table)
            {
                cardDescription.SetActive(false);
            }
            else
            {
                cardDescription.SetActive(true);
            }

            yield return new WaitForSeconds(0.3f);
            EventManager.Instance.processingQueue = false;



        }

        public void ExpireCard()
        {
            SetCardPosition(CardStatus.InDiscard);
            //startPoint = table;
            //endPoint = discard;
            EventManager.Instance.RemoveListener<TableCard_Trigger>(CardTrigger);

            foreach (CardEffect cardeffect in card.effects)
            {
                if (cardeffect.trigger == CardEffect.Trigger.Passive)
                {
                    RemoveEffect(cardeffect);
                }
            }


            EventManager.Instance.QueueAnimation(new MoveCard_GUI(this, table, discard));
            moveCounter++;

        }
        internal void RemoveEffect(CardEffect cardEffect)
        {
            EventManager.Instance.AddListener<CardEffect_GUI>(CardEffectAnimation);
            effectCounter++;

            EventManager.Instance.QueueAnimation(new CardEffect_GUI(cardEffect.value, owner, this, cardEffect.effect));
            cardEffect.value = cardEffect.value * -1;
                       
            if (CardEffect.Effect.AddMaxMana.Equals(cardEffect.effect))
            {
                
                CardgameManager.instance.IncreaseMaxMana(cardEffect.value, owner);
            }

            else if (CardEffect.Effect.ReduceDamage.Equals(cardEffect.effect))
            {
                CardgameManager.instance.IncreaseDamageReduction(cardEffect.value, owner);
            }
            else
            {
                Debug.LogError("Card Effect not Implemented yet!");
            }
        }
    }
    

}

