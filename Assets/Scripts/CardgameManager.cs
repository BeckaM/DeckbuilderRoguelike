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

 

//  public List<GameObject> MyDeckCards = new List<GameObject>();
public List<GameObject> MyHandCards = new List<GameObject>();
        public List<GameObject> MyTableCards = new List<GameObject>();
        //   public List<GameObject> MyDiscardCards = new List<GameObject>();

        //   public List<GameObject> AIDeckCards = new List<GameObject>();
        public List<GameObject> AIHandCards = new List<GameObject>();
        public List<GameObject> AITableCards = new List<GameObject>();
        // public List<GameObject> AIDiscardCards = new List<GameObject>();

        public EnemyManager enemy;
        public Player player;

        void Awake()
        {
            instance = this;

        }

        void OnEnable()
        {
            EventManager.Instance.AddListener<UpdateDeckTexts>(UpdateDeckDiscardText);
        }

        void OnDisable()
        {
            EventManager.Instance.RemoveListener<UpdateDeckTexts>(UpdateDeckDiscardText);
        }




        // Use this for initialization
        public void Setup()
        {
            //Sets opponent profile images and texts.
            SetOpponents();

            //FindLevelObjects();

            //Draw our starting hand
            DrawStartingHands();


            UpdateGame();
        }

        //private void FindLevelObjects()
        //{
        //    //DungeonCanvas = GameObject.Find("Canvas(Board)");
        //    myDeck = GameObject.Find("MyDeck(Clone").GetComponent<DeckManager>();
        //    AIDeck = GameObject.Find("AIDeck(Clone)").GetComponent<DeckManager>();
        //}



        private void DrawStartingHands()
        {
            for (var i = 0; i < 3; i++)
            {

                GameManager.instance.myDeck.Draw();
                GameManager.instance.AIDeck.Draw();


            }
        }


        //private void PutCardsInLists()
        //{
        //    foreach (GameObject CardObject in GameObject.FindGameObjectsWithTag("Card"))
        //    {
        //        //CardObject.GetComponent<Rigidbody>().isKinematic = true;
        //        CardManager c = CardObject.GetComponent<CardManager>();

        //        if (c.team == CardManager.Team.My)
        //        {
        //            MyDeckCards.Add(CardObject);

        //        }


        //        else
        //        {
        //            AIDeckCards.Add(CardObject);
        //        }
        //    }

        //}

        private void UpdateDeckDiscardText(UpdateDeckTexts updates)
        {
            if (updates.team == CardManager.Team.My)
            {
                playerDeckCount.text = updates.decktext.ToString();
                playerDiscardCount.text = updates.discardtext.ToString();
            }
            else
            {
                monsterDeckCount.text = updates.decktext.ToString();
                monsterDiscardCount.text = updates.discardtext.ToString();
            }
            EventManager.Instance.processingQueue = false;
        }

        private void SetOpponents()
        {

            player.mana = player.maxMana;
            enemy.mana = enemy.maxMana;

            //enemy.UpdateLife();
            monsterPortrait.sprite = enemy.monsterImage;

           // player.UpdateLife();
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
            playerManaText.text = "Mana:" + player.mana + "/" + player.maxMana;
            monsterManaText.text = "Mana" + enemy.mana + "/" + enemy.maxMana;

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

            //MyDeckCards.Clear();
            //MyHandCards.Clear();
            //MyDiscardCards.Clear();
            //MyTableCards.Clear();

            //AIDeckCards.Clear();
            //AIHandCards.Clear();
            //AIDiscardCards.Clear();
            //AITableCards.Clear();

            //DeckManager.instance.Cleanup();
            this.gameObject.SetActive(false);

            GameManager.instance.ReturnFromCardgame(win);
        }



        //Triggered by end turn button.
        public void EndTurn()
        {
            if (turn == Turn.AITurn)
            {
                GameManager.instance.myDeck.Draw();
                turn = Turn.MyTurn;
            }
            else if (turn == Turn.MyTurn)
            {
               GameManager.instance.AIDeck.Draw();
                turn = Turn.AITurn;
            }

            player.mana = player.maxMana;
            enemy.mana = enemy.maxMana;
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

            // StartCoroutine(ResolveCard(card));       

        }

        //private IEnumerator ResolveCard(CardManager card)
        //{
        //    Debug.Log("Playing a new card");
        //    yield return StartCoroutine(ApplyEffects(card));

        //    yield return StartCoroutine(CheckTriggers(CardEffect.Trigger.OnPlayCard, card.team));

        //    yield return StartCoroutine(MoveCard(card));
        //    Debug.Log("Done playing the card");

        //}




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