using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.
using UnityEngine.UI;
using System.Collections;

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

        public GameObject playerDiscard;
        public GameObject monsterDiscard;

        public GameObject playerTable;
        public GameObject monsterTable;

        public GameObject playerHand;
        public GameObject monsterHand;

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
        public void Setup()
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

            player.mana = player.maxMana;
            enemy.mana = enemy.maxMana;

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

            MyDeckCards.Clear();
            MyHandCards.Clear();
            MyDiscardCards.Clear();
            MyTableCards.Clear();

            AIDeckCards.Clear();
            AIHandCards.Clear();
            AIDiscardCards.Clear();
            AITableCards.Clear();

            DeckManager.instance.Cleanup();
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


                
                tempCard.transform.SetParent(playerHand.transform);
                tempCard.GetComponent<CardManager>().SetCardStatus(CardManager.CardStatus.InHand);

                MyDeckCards.Remove(tempCard);
                MyHandCards.Add(tempCard);
                
            }

            if (team == CardManager.Team.AI && AIDeckCards.Count != 0 && AIHandCards.Count < 10)
            {
                int random = Random.Range(0, AIDeckCards.Count);
                GameObject tempCard = AIDeckCards[random];
                               
                tempCard.transform.SetParent(monsterHand.transform);
                tempCard.GetComponent<CardManager>().SetCardStatus(CardManager.CardStatus.InHand);

                AIDeckCards.Remove(tempCard);
                AIHandCards.Add(tempCard);
                
            }
            UpdateGame();
        }

        public void PlaceCard(CardManager card)
        {

            //Pay the mana cost.
            if (card.team == CardManager.Team.My)
            {
                player.mana = player.mana - card.card.Cost;
            }
            else
            {
                enemy.mana = enemy.mana - card.card.Cost;
            }

            UpdateGame();

            //PlaySound(cardDrop);          

            StartCoroutine(ResolveCard(card));       
                       
        }

        private IEnumerator ResolveCard(CardManager card)
        {
            Debug.Log("Playing a new card");
            yield return StartCoroutine(ApplyEffects(card));
                       
            yield return StartCoroutine(CheckTriggers(CardEffect.Trigger.OnPlayCard, card.team));
                        
            yield return StartCoroutine(MoveCard(card));
            Debug.Log("Done playing the card");

        }

        private IEnumerator MoveCard(CardManager card)
        {
            Debug.Log("Start moving card");
            //Move the card to it's onwers table section if it has a duration, otherwise discard it. 
            MyHandCards.Remove(card.gameObject);
            if (card.team == CardManager.Team.My)
            {

                if (card.card.CardDuration != 0)
                {
                    card.SetCardStatus(CardManager.CardStatus.OnTable);
                    MyTableCards.Add(card.gameObject);
                    card.Move(playerTable);
                    yield return new WaitForSeconds(1.5f);
                    card.transform.SetParent(playerTable.transform);
                    
                }
                else
                {
                    card.SetCardStatus(CardManager.CardStatus.InDiscard);
                    MyDiscardCards.Add(card.gameObject);
                    card.Move(playerDiscard);
                    yield return new WaitForSeconds(1.5f);
                    playerDiscardCount.text = MyDiscardCards.Count.ToString();                    
                    card.transform.SetParent(DeckManager.instance.playerDiscard.transform);
                    
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
            card.transform.localScale = Vector3.one;
            Debug.Log("Stop moving card");
            UpdateGame();
            
        }

        private IEnumerator ApplyEffects(CardManager card)
        {
            Debug.Log("Start Applying Card effects");
            //Apply the cards effects if they are instant.
            foreach (CardEffect effect in card.card.Effects)
            {
                if (effect.trigger == CardEffect.Trigger.Instant)
                {
                    card.ApplyEffect(effect);
                    
                    yield return new WaitForSeconds(1f);
                }
            }

            Debug.Log("Stop applying Card effects");
        }

        private IEnumerator CheckTriggers(CardEffect.Trigger triggertype, CardManager.Team team)
        {
            Debug.Log("Start checking for triggers");
            yield return new WaitForSeconds(3F);
            Debug.Log("Stop checking for triggers");

        }
    }
}