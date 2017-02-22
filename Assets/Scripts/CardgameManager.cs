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


        public enum Team { My, AI };
        public Team turn = Team.My;

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

        public GameObject playerDeck;
        public GameObject monsterDeck;

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
            EventManager.Instance.AddListener<UpdateManaText_GUI>(GUIUpdateMana);
            EventManager.Instance.AddListener<UpdateDeckTexts_GUI>(UpdateDeckDiscardText);
        }

       

        void OnDisable()
        {
            EventManager.Instance.RemoveListener<UpdateManaText_GUI>(GUIUpdateMana);
            EventManager.Instance.RemoveListener<UpdateDeckTexts_GUI>(UpdateDeckDiscardText);

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

                DeckManager.player.Draw();
                DeckManager.monster.Draw();


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

        private void UpdateDeckDiscardText(UpdateDeckTexts_GUI updates)
        {
            if (updates.team == Team.My)
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

        private void GUIUpdateMana(UpdateManaText_GUI e)
        {
            //Update Mana text in UI.
            playerManaText.text = "Mana:" + player.mana + "/" + player.maxMana;
            monsterManaText.text = "Mana" + enemy.mana + "/" + enemy.maxMana;
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

        internal void ApplyDamage(int value, Team team)
        {
            if (team == Team.My)
            {
                enemy.LoseLife(value);
            }
            else
            {
                player.LoseLife(value);

            }

        }

        internal void ApplyHealing(int value, Team team)
        {
            if (team == Team.My)
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
            

            // Set cards as playable and/or draggable.
            //SetPlayableDraggable();

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

        //private void SetPlayableDraggable()
        //{
        //    if (turn == Team.My)
        //    {
        //        foreach (GameObject Card in MyHandCards)
        //        {

        //            CardManager c = Card.GetComponent<CardManager>();
        //            c.isDragable = true;
        //            if (c.card.cost <= player.mana)
        //            {
        //                c.isPlayable = true;

        //            }
        //            else
        //            {
        //                c.isPlayable = false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (GameObject Card in AIHandCards)
        //        {
        //            CardManager c = Card.GetComponent<CardManager>();
        //            if (c.card.cost <= enemy.mana)
        //            {
        //                c.isPlayable = true;

        //            }
        //            else
        //            {
        //                c.isPlayable = false;
        //            }
        //        }
        //    }
        //}

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
            if (turn == Team.AI)
            {
                DeckManager.player.Draw();
                turn = Team.My;
            }
            else if (turn == Team.My)
            {
                DeckManager.monster.Draw();
                turn = Team.AI;
            }

            player.mana = player.maxMana;
            enemy.mana = enemy.maxMana;
            UpdateGame();
        }


        public void PlaceCard(CardManager card)
        {

            //Pay the mana cost.
            Debug.Log("Playing a new card: " + card.card.cardName);
            if (card.owner == Team.My)
            {
                player.mana = player.mana - card.card.cost;
                EventManager.Instance.QueueAnimation(new UpdateManaText_GUI(player.mana, card.owner));
            }
            else
            {
                enemy.mana = enemy.mana - card.card.cost;
                EventManager.Instance.QueueAnimation(new UpdateManaText_GUI(enemy.mana, card.owner));
            }



            //Check for triggers from playing a card.
            Debug.Log("Start checking for triggers on play card");
            EventManager.Instance.QueueTrigger(new PlayCard_Trigger(card.owner));
            Debug.Log("Done checking for triggers");

            //Apply the cards effects if they are instant.
            Debug.Log("Start Applying Card effects");

            foreach (CardEffect effect in card.card.effects)
            {
                if (effect.trigger == CardEffect.Trigger.Instant)
                {
                    card.ApplyEffect(effect);

                }
            }
            Debug.Log("Done applying Card effects");


            //Set the new card position.
            if (card.card.cardDuration == 0)
            {
                card.SetCardPosition(CardManager.CardStatus.InDiscard);
            }
            else
            {
                card.SetCardPosition(CardManager.CardStatus.OnTable);
            }

            //Queue up a move card event to move the card to it's final destination.
            EventManager.Instance.QueueAnimation(new MoveCard_GUI(card));

        }

        //private IEnumerator ResolveCard(CardManager card)
        //{
        //    Debug.Log("Playing a new card");
        //    yield return StartCoroutine(ApplyEffects(card));

        //    yield return StartCoroutine(CheckTriggers(CardEffect.Trigger.OnPlayCard, card.team));

        //    yield return StartCoroutine(MoveCard(card));
        //    Debug.Log("Done playing the card");

        //}


        //private IEnumerator CheckTriggers(CardEffect.Trigger triggertype, CardManager.Team team)
        //{
        //    Debug.Log("Start checking for triggers");
        //    yield return new WaitForSeconds(3F);
        //    Debug.Log("Stop checking for triggers");

        //}
    }
}