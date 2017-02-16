using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.
using UnityEngine.UI;

namespace Assets.Scripts
{

    public class CardgameManager : MonoBehaviour
    {

        public static CardgameManager instance;


        public enum Turn { MyTurn, AITurn };
        public Turn turn = Turn.MyTurn;

        public Image playerPortrait;
        public Text playerLifeText;
        public Text playerManaText;
        public Text playerDeckCount;
        public Text playerDiscardCount;

        public Image monsterPortrait;
        public Text monsterLifeText;
        public Text monsterManaText;
        public Text monsterDeckCount;
        public Text monsterDiscardCount;

        public List<GameObject> MyDeckCards = new List<GameObject>();
        public List<GameObject> MyHandCards = new List<GameObject>();
        public List<GameObject> MyTableCards = new List<GameObject>();
        public List<GameObject> MyDiscardCards = new List<GameObject>();

        public List<GameObject> AIDeckCards = new List<GameObject>();
        public List<GameObject> AIHandCards = new List<GameObject>();
        public List<GameObject> AITableCards = new List<GameObject>();
        public List<GameObject> AIDiscardCards = new List<GameObject>();

        public EnemyManager enemy;
        public Player player;

        void Awake()
        {
            instance = this;

        }


        // Use this for initialization
        void Start()
        {
            //Sets opponent profile images and texts.
            SetOpponents();

            //Organize the cards into the correct lists.
            PutCardsInLists();

            //Draw our starting hand
            DrawStartingHands();

            UpdateGame();
        }

        private void DrawStartingHands()
        {
            for (var i = 0; i < 3; i++)
            {
                DrawCardFromDeck(CardManager.Team.My);
                DrawCardFromDeck(CardManager.Team.AI);
            }
        }

       
        private void PutCardsInLists()
        {
            foreach (GameObject CardObject in GameObject.FindGameObjectsWithTag("Card"))
            {
                //CardObject.GetComponent<Rigidbody>().isKinematic = true;
                CardManager c = CardObject.GetComponent<CardManager>();

                if (c.team == CardManager.Team.My)
                {
                    MyDeckCards.Add(CardObject);

                }


                else
                {
                    AIDeckCards.Add(CardObject);
                }
            }
            
        }

        private void UpdateDeckDiscardText()
        {
            playerDeckCount.text = MyDeckCards.Count.ToString();
            monsterDeckCount.text = AIDeckCards.Count.ToString();
            playerDiscardCount.text = MyDiscardCards.Count.ToString();
            monsterDiscardCount.text = AIDiscardCards.Count.ToString();
        }

        private void SetOpponents()
        {
            enemy.UpdateLife();
            monsterPortrait.sprite = enemy.monsterImage;

            player.UpdateLife();
            playerPortrait.sprite = player.playerImage;
        }

        internal void ApplyDamage(int value, CardManager.Team team)
        {
            if (team == CardManager.Team.My)
            {
                enemy.LoseLife(value);
            }
            else
            {
                player.LoseLife(value);

            }           

        }

        internal void ApplyHealing(int value, CardManager.Team team)
        {
            if (team == CardManager.Team.My)
            {
                player.GainLife(value);
            }
            else
            {
                enemy.GainLife(value);

            }
            
        }

        public void UpdateGame()
        {   
            //Update Mana text in UI.
            playerManaText.text = "Mana:" + player.mana + "/" +  player.maxMana;
            monsterManaText.text = "Mana" + enemy.mana + "/" + enemy.maxMana;
           
            //Update Deck and Discard text in UI.            
            UpdateDeckDiscardText();

            // Set cards as playable and/or draggable.
            SetPlayableDraggable();

            //Check if player or monster has reached 0 life
            CheckWinConditions();
        }

        private void CheckWinConditions()
        {
            bool win;

            if (player.life <= 0)
            {
                win = false;
                EndGame(win);
            }
            else if (enemy.life <= 0)
            {
                win = true;
                EndGame(win);
            }
        }

        private void SetPlayableDraggable()
        {
            if (turn == Turn.MyTurn)
            {
                foreach (GameObject Card in MyHandCards)
                {

                    CardManager c = Card.GetComponent<CardManager>();
                    c.isDragable = true;
                    if (c.card.Cost <= player.mana)
                    {
                        c.isPlayable = true;

                    }
                    else
                    {
                        c.isPlayable = false;
                    }
                }
            }
            else
            {
                foreach (GameObject Card in AIHandCards)
                {
                    CardManager c = Card.GetComponent<CardManager>();
                    if (c.card.Cost <= enemy.mana)
                    {
                        c.isPlayable = true;

                    }
                    else
                    {
                        c.isPlayable = false;
                    }
                }
            }
        }

        private void EndGame(bool win)
        {
            this.gameObject.SetActive(false);
            GameManager.instance.ReturnFromCardgame(win);
        }



        //Triggered by end turn button.
        public void EndTurn()
        {
            if (turn == Turn.AITurn)
            {
                DrawCardFromDeck(CardManager.Team.My);
                turn = Turn.MyTurn;
            }
            else if (turn == Turn.MyTurn)
            {
                DrawCardFromDeck(CardManager.Team.AI);
                turn = Turn.AITurn;
            }

            player.mana = player.maxMana;
            enemy.mana = enemy.maxMana;
            UpdateGame();
        }

        //Draw card function, Team indicates who draws.
        public void DrawCardFromDeck(CardManager.Team team)
        {

            if (team == CardManager.Team.My && MyDeckCards.Count != 0 && MyHandCards.Count < 10)
            {
                int random = Random.Range(0, MyDeckCards.Count);
                GameObject tempCard = MyDeckCards[random];


                var hand = GameObject.Find("Hand").transform;
                tempCard.transform.SetParent(hand);
                tempCard.GetComponent<CardManager>().SetCardStatus(CardManager.CardStatus.InHand);

                MyDeckCards.Remove(tempCard);
                MyHandCards.Add(tempCard);
                
            }

            if (team == CardManager.Team.AI && AIDeckCards.Count != 0 && AIHandCards.Count < 10)
            {
                int random = Random.Range(0, AIDeckCards.Count);
                GameObject tempCard = AIDeckCards[random];

                var AIhand = GameObject.Find("AIHand").transform;
                tempCard.transform.SetParent(AIhand);
                tempCard.GetComponent<CardManager>().SetCardStatus(CardManager.CardStatus.InHand);

                AIDeckCards.Remove(tempCard);
                AIHandCards.Add(tempCard);
                
            }
            UpdateGame();
        }

        public void PlaceCard(CardManager card)
        {

            //Pay the mana cost.
            if(card.team == CardManager.Team.My)
            {
                player.mana = player.mana - card.card.Cost;
            }
            else
            {
                enemy.mana = enemy.mana - card.card.Cost;
            }           

            //PlaySound(cardDrop);          
            
            //Apply the cards effects if they are instant.
            foreach (CardEffect effect in card.card.Effects)
            {
                if (effect.trigger == CardEffect.Trigger.Instant)
                {
                    card.ApplyEffect(effect);
                }
            }

            //Move the card to it's onwers table section if it has a duration, otherwise discard it. 
            MyHandCards.Remove(card.gameObject);
            if (card.team == CardManager.Team.My)               
            {

                if (card.card.CardDuration > 0)
                {
                    card.SetCardStatus(CardManager.CardStatus.OnTable);
                    MyTableCards.Add(card.gameObject);
                }
                else 
                {
                    card.SetCardStatus(CardManager.CardStatus.InDiscard);
                    MyDiscardCards.Add(card.gameObject);
                    playerDiscardCount.text = MyDiscardCards.Count.ToString();
                }
            }
            else
            {

                if (card.card.CardDuration > 0)
                {
                    card.SetCardStatus(CardManager.CardStatus.OnTable);
                    AITableCards.Add(card.gameObject);
                }
                else
                {
                    card.SetCardStatus(CardManager.CardStatus.InDiscard);
                    AIDiscardCards.Add(card.gameObject);
                    monsterDiscardCount.text = AIDiscardCards.Count.ToString();
                }
            }

            //Check for cards in play that trigger on playing a card.
            //   CheckTriggers(CardEffect.Trigger.OnPlayCard, card.team);
                        
            UpdateGame();


        }

        private void CheckTriggers(CardEffect.Trigger triggertype, CardManager.Team team)
        {
            throw new NotImplementedException();
        }
    }
}