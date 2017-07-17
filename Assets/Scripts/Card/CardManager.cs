using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace Assets.Scripts
{

    public class CardManager : MonoBehaviour
    {
        public Card card;

        public CardBottomPanel bottomPanel;
        public CardImagePanel imagePanel;

        public Image cardPanel;
        public GameObject descriptionPanel;
        public TMP_Text descriptionText;

        public TMP_Text cardName;
        public GameObject cardEffectText;

        public int duration;

        public enum CardStatus { InDeck, InHand, OnTable, InDiscard };
        public CardStatus cardStatus = CardStatus.InDeck;

        public CardgameManager.Team owner;

        public CardgameManager.Team Opponent
        {
            get
            {
                if (owner == CardgameManager.Team.Me)
                {
                    return CardgameManager.Team.Opponent;
                }
                else
                {
                    return CardgameManager.Team.Me;
                }
            }
        }

        public int OwnerMana
        {
            get
            {
                if (owner == CardgameManager.Team.Me)
                {
                    return CardgameManager.instance.player.mana;
                }
                else
                {
                    return CardgameManager.instance.enemy.mana;
                }
            }
        }

        public DeckManager DeckManager
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

        public DeckManager OpponentDeckManager
        {
            get
            {
                if (owner == CardgameManager.Team.Me)
                {
                    return DeckManager.monster;
                }
                else
                {
                    return DeckManager.player;
                }
            }
        }

        public bool IsPlayable
        {
            get
            {
                if (CardgameManager.instance.turn == owner && OwnerMana >= card.cost && cardStatus == CardStatus.InHand)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsDragable
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

        public GameObject Table
        {
            get
            {
                if (owner == CardgameManager.Team.Me)
                {
                    return CardgameManager.instance.playerTable;
                }
                else
                {
                    return CardgameManager.instance.monsterTable;
                }
            }
        }

        public GameObject Discard
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

        public float adjustedMoveTime;

        public int effectCounter;
        public int moveCounter;

        private Rigidbody2D rb2D;

        protected void Start()
        {

            //Get a component reference to this object's Rigidbody2D
            rb2D = GetComponent<Rigidbody2D>();

            //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
            // adjustedMoveTime = Screen.width;
            adjustedMoveTime = 50;
        }


        public void SetCardPosition(CardStatus status)
        {
            cardStatus = status;           

            if (status == CardStatus.OnTable)
            {               
                EventManager.Instance.AddListener<TableCard_Trigger>(CardTrigger);
            }
            else if (status == CardStatus.InDiscard)
            {
                DeckManager.cardsInDiscard.Add(this);              
            }
            else if (status == CardStatus.InHand)
            {               
                DeckManager.cardsInHand.Add(this);
            }
            else if (status == CardStatus.InDeck)
            {               
                DeckManager.cardsInDeck.Add(this);                
            }
        }

        //Cards on the table listen to table card triggers. 
        private void CardTrigger(TableCard_Trigger trigger)
        {
            //Loop through the cards effects and apply them if they match the trigger.
            foreach (CardEffect cardeffect in card.effects)
            {
                //If the trigger is End Of Turn, apply end of turn effect. 
                if (trigger.effect == CardEffect.Trigger.EndOfTurn && cardeffect.trigger == CardEffect.Trigger.EndOfTurn)
                {
                    if (trigger.team == owner)
                    {
                        ApplyEffect(cardeffect);
                    }
                }
                   else if (trigger.effect == cardeffect.trigger && trigger.team == cardeffect.triggeredBy)
                {
                    ApplyEffect(cardeffect);
                }
            }

            //If the trigger is EndOfTurn, all cards on the table lose one duration and expire if needed.
            if (trigger.effect == CardEffect.Trigger.EndOfTurn)
            {
                //Debug.Log(card.cardName);
                duration--;
                
                if (duration == 0)
                {
                    ExpireCard();
                }
            }
        }


        public void PopulateCard(Card card)
        {
            this.card = card;

            //Set Card Background pane.
            cardPanel.color = card.backgroundColor;

            //Set Card Title          
            cardName.text = card.cardName;

            //Set Card Description           
            descriptionText.text = card.cardText;

            duration = card.cardDuration;

            imagePanel.PopulateCardImage(card);
            bottomPanel.PopulateBottomPanel(card);
        }


        public void ResetCard(GameObject end)
        {
            duration = card.cardDuration;

            if (end == Table)
            {
                bottomPanel.ShowBottomPanel(false);
              //  imagePanel.ResetPanel();
            }
            else if (end == Discard)
            {
                bottomPanel.ShowBottomPanel(true);
                //imagePanel.ResetPanel();
                GetComponent<RectTransform>().sizeDelta = new Vector2(250, 320);

                GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, DeckManager.discardOffset);
                DeckManager.discardOffset -= 2;
            }
            else if (end == DeckManager.Deck)
            {
                bottomPanel.ShowBottomPanel(true);
               // imagePanel.ResetPanel();
                GetComponent<RectTransform>().sizeDelta = new Vector2(250, 320);

                GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, DeckManager.deckOffset);
                DeckManager.deckOffset -= 2;
            }
        }
                
        
        internal void ApplyEffect(CardEffect cardEffect)
        {
            EventManager.Instance.AddListener<CardEffect_GUI>(CardEffectAnimation);
            effectCounter++;

            EventManager.Instance.QueueAnimation(new CardEffect_GUI(cardEffect.value, owner, this, cardEffect.effect));

            switch (cardEffect.effect)
            {
                case CardEffect.Effect.DealDamage:
                    {
                        CardgameManager.instance.ApplyDamage(cardEffect.value, cardEffect.ignoresArmor, owner);
                        EventManager.Instance.TriggerEvent(new TableCard_Trigger(owner, CardEffect.Trigger.OnDealDamage));
                        break;
                    }
                case CardEffect.Effect.Heal:
                    {
                        CardgameManager.instance.ApplyHealing(cardEffect.value, owner);
                        EventManager.Instance.TriggerEvent(new TableCard_Trigger(owner, CardEffect.Trigger.OnHeal));
                        break;
                    }
                case CardEffect.Effect.AddMaxMana:
                    {
                        CardgameManager.instance.IncreaseMaxMana(cardEffect.value, owner);
                        break;
                    }
                case CardEffect.Effect.ReduceDamage:
                    {
                        CardgameManager.instance.IncreaseDamageReduction(cardEffect.value, owner);
                        break;
                    }
                case CardEffect.Effect.IncreaseDamage:
                    {
                        CardgameManager.instance.IncreaseDamage(cardEffect.value, owner);
                        break;
                    }
                case CardEffect.Effect.DrawCard:
                    {
                        DeckManager.Draw();
                        break;
                    }
                case CardEffect.Effect.DiscardCard:
                    {
                        OpponentDeckManager.DiscardRandomCard();
                        break;
                    }
                case CardEffect.Effect.SelfDamage:
                    {
                        CardgameManager.instance.ApplyDamage(cardEffect.value, cardEffect.ignoresArmor, Opponent);
                        EventManager.Instance.TriggerEvent(new TableCard_Trigger(owner, CardEffect.Trigger.OnDealDamage));
                        break;
                    }
                case CardEffect.Effect.SelfDiscard:
                    {
                        DeckManager.DiscardRandomCard();
                        break;
                    }
                default:
                    {
                        Debug.LogError("Card effect not implemented yet!");
                        break;
                    }
            }
        }


        public void CardEffectAnimation(CardEffect_GUI e)
        {
            if (e.card == this)
            {
                var text = cardEffectText.GetComponent<TMP_Text>();
                switch (e.type)
                {
                    case CardEffect.Effect.AddMaxMana:
                        {
                            text.text = "Max Mana Increased +" + e.value + "!";
                            StartCoroutine(EffectText(Color.blue));
                            break;
                        }
                    case CardEffect.Effect.DealDamage:
                        {
                            text.text = "" + e.value + " Damage!";
                            StartCoroutine(EffectText(Color.red));
                            break;
                        }
                    case CardEffect.Effect.Heal:
                        {
                            text.text = "" + e.value + " Life Restored!";
                            StartCoroutine(EffectText(Color.green));
                            break;
                        }
                    case CardEffect.Effect.ReduceDamage:
                        {
                            text.text = "+" + e.value + " Damage Reduction!";
                            StartCoroutine(EffectText(Color.grey));
                            break;
                        }
                    case CardEffect.Effect.IncreaseDamage:
                        {
                            text.text = "+" + e.value + " Damage Boost!";
                            StartCoroutine(EffectText(Color.yellow));
                            break;
                        }
                    case CardEffect.Effect.SelfDamage:
                        {
                            text.text = "" + e.value + " Self Damage!";
                            StartCoroutine(EffectText(Color.red));
                            break;
                        }
                    case CardEffect.Effect.DiscardCard:
                        {
                            text.text = "Discard " + e.value + " Card!";
                            StartCoroutine(EffectText(Color.red));
                            break;
                        }
                    default:
                        {
                            Debug.LogError("No Animation for this effect");
                            EventManager.Instance.processingQueue = false;
                            break;
                        }
                }
            }
        }


        private IEnumerator EffectText(Color color)
        {
            var text = cardEffectText.GetComponent<TMP_Text>();
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


            //if we have no cards at this position, make a test place and get the new position from that.
            if (end.transform.childCount == 0)
            {
                this.GetComponent<CanvasGroup>().alpha = (0f);
                transform.SetParent(end.transform, false);
                GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
                yield return new WaitForEndOfFrame();
                endpos = this.transform.position;
            }

            //If we already have a card at that position, place it where the last card is.
            else
            {
                this.GetComponent<CanvasGroup>().alpha = (0f);
                var lastChild = end.transform.GetChild(end.transform.childCount - 1);
                endpos = lastChild.transform.position;
            }
            this.transform.SetParent(start.transform, false);
            yield return new WaitForSeconds(0.1f);

            GetComponent<CanvasGroup>().alpha = (1f);

            //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
            //Square magnitude is used instead of magnitude because it's computationally cheaper.
            float sqrRemainingDistance = (transform.position - endpos).sqrMagnitude;

            //While that distance is greater than a very small amount (Epsilon, almost zero):
            while (sqrRemainingDistance > 0.001f)
            {
                //Find a new position proportionally closer to the end, based on the moveTime
                Vector3 newPostion = Vector3.MoveTowards(transform.position, endpos, adjustedMoveTime * Time.deltaTime);

                //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
                transform.position = newPostion;

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

            transform.SetParent(end.transform, false);
            GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
            if (start == Discard)
            {
                DeckManager.discardOffset += 2;
            }
            else if (start == DeckManager.Deck)
            {
                DeckManager.deckOffset += 2;
            }
            ResetCard(end);
            yield return new WaitForSeconds(0.3f);
            EventManager.Instance.processingQueue = false;
        }


        internal void DestroyCardGUI(DestroyCard_GUI e)
        {
            if (e.card == this)
            {
                var text = cardEffectText.GetComponent<TMP_Text>();

                text.text = "Card Consumed!";

                StartCoroutine(EffectText(Color.red));

                Invoke("DestroyCard", 1.5f);
            }
        }

        private void DestroyCard()
        {
            DeckManager.player.DestroyCard(this);
        }


        //Card expires if it runs out of duration. Checked at the end of turn, both players and AIs.
        public void ExpireCard()
        {
            SetCardPosition(CardStatus.InDiscard);           
            EventManager.Instance.RemoveListener<TableCard_Trigger>(CardTrigger);

            foreach (CardEffect cardeffect in card.effects)
            {
                if (cardeffect.trigger == CardEffect.Trigger.Passive)
                {
                    RemoveEffect(cardeffect);
                }
                else if (cardeffect.trigger == CardEffect.Trigger.OnExpire)
                {
                    ApplyEffect(cardeffect);
                }
            }
            EventManager.Instance.AddListener<MoveCard_GUI>(Move);
            EventManager.Instance.QueueAnimation(new MoveCard_GUI(this, Table, Discard));
            moveCounter++;
        }


        internal void RemoveEffect(CardEffect cardEffect)
        {
            //EventManager.Instance.AddListener<CardEffect_GUI>(CardEffectAnimation);
            //effectCounter++;

            var revert = cardEffect.value * -1;
            //EventManager.Instance.QueueAnimation(new CardEffect_GUI(revert, owner, this, cardEffect.effect));

            if (CardEffect.Effect.AddMaxMana.Equals(cardEffect.effect))
            {
                CardgameManager.instance.IncreaseMaxMana(revert, owner);
            }

            else if (CardEffect.Effect.ReduceDamage.Equals(cardEffect.effect))
            {
                CardgameManager.instance.IncreaseDamageReduction(revert, owner);
            }
            else if (CardEffect.Effect.IncreaseDamage.Equals(cardEffect.effect))
            {
                CardgameManager.instance.IncreaseDamage(revert, owner);
            }
            else
            {
                Debug.LogError("Card Effect not Implemented yet!");
            }
        }
    }
}

