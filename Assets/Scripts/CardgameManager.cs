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


        public enum Team { Me, Opponent };
        public Team turn = Team.Me;

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

        public GameObject tabletop;

        public Image monsterPortrait;
        public Text monsterLifeText;
        public Text monsterManaText;
        public Text monsterDeckCount;
        public Text monsterDiscardCount;      
                      
        public EnemyManager enemy;
        public Player player;

        void Awake()
        {
            instance = this;

        }

        void OnEnable()
        {
            EventManager.Instance.AddListener<UpdateMana_GUI>(GUIUpdateMana);
            EventManager.Instance.AddListener<UpdateLife_GUI>(GUIUpdateLife);
            EventManager.Instance.AddListener<UpdateDeckTexts_GUI>(GUIUpdateDeckDiscardText);
        }



        void OnDisable()
        {
            EventManager.Instance.RemoveListener<UpdateMana_GUI>(GUIUpdateMana);
            EventManager.Instance.RemoveListener<UpdateLife_GUI>(GUIUpdateLife);
            EventManager.Instance.RemoveListener<UpdateDeckTexts_GUI>(GUIUpdateDeckDiscardText);

        }




        // Use this for initialization
        public void Setup()
        {
            //Sets opponent profile images and texts.
            SetOpponents();

            //Update life and mana in the GUI.
            EventManager.Instance.QueueAnimation(new UpdateLife_GUI(player.life, player.maxLife, Team.Me));
            EventManager.Instance.QueueAnimation(new UpdateLife_GUI(enemy.life, enemy.maxLife, Team.Opponent));
            EventManager.Instance.QueueAnimation(new UpdateMana_GUI(player.mana, player.maxMana, Team.Me));
            EventManager.Instance.QueueAnimation(new UpdateMana_GUI(enemy.mana, enemy.maxMana, Team.Opponent));

            //Draw our starting hand
            DrawStartingHands();

        }

        private void DrawStartingHands()
        {
            for (var i = 0; i < 3; i++)
            {

                DeckManager.player.Draw();
                DeckManager.monster.Draw();

            }
        }

        private void GUIUpdateDeckDiscardText(UpdateDeckTexts_GUI updates)
        {
            if (updates.team == Team.Me)
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

        private void GUIUpdateMana(UpdateMana_GUI e)
        {
            //Update Mana text in UI.
            if (e.team == Team.Me)
            {
                playerManaText.text = "Mana:" + e.mana + "/" + player.maxMana;
            }
            else
            {
                monsterManaText.text = "Mana:" + e.mana + "/" + enemy.maxMana;
            }

            EventManager.Instance.processingQueue = false;
        }

        private void GUIUpdateLife(UpdateLife_GUI e)
        {
            //Update Mana text in UI.
            if (e.team == Team.Me)
            {
                playerLifeText.text = "Life:" + e.life + "/" + player.maxLife;
            }
            else
            {
                monsterLifeText.text = "Life:" + e.life + "/" + enemy.maxLife;
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

        internal void IncreaseMaxMana(int value, Team team)
        {
            if (team == Team.Me)
            {
                player.maxMana += value;

                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(player.mana, player.maxMana, Team.Me));
            }
            else
            {
                enemy.maxMana += value;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(enemy.mana, enemy.maxMana, Team.Opponent));

            }

        }
        internal void IncreaseDamageReduction(int value, Team team)
        {
            if (team == Team.Me)
            {
                player.ward += value;

                //   EventManager.Instance.QueueAnimation(new UpdateMana_GUI(player.mana, player.maxMana, Team.My));
            }
            else
            {
                enemy.ward += value;
                // EventManager.Instance.QueueAnimation(new UpdateMana_GUI(enemy.mana, enemy.maxMana, Team.AI));

            }

        }
        internal void ApplyDamage(int value, Team team)
        {
            if (team == Team.Me)
            {
                enemy.life -= value;

                EventManager.Instance.QueueAnimation(new UpdateLife_GUI(enemy.life, enemy.maxLife, Team.Opponent));
            }
            else
            {
                player.life -= value;
                EventManager.Instance.QueueAnimation(new UpdateLife_GUI(player.life, player.maxLife, Team.Me));

            }

        }

        internal void ApplyHealing(int value, Team team)
        {
            if (team == Team.Me)
            {
                player.life += value;
                if (player.life > player.maxLife)
                {
                    player.life = player.maxLife;
                }
                EventManager.Instance.QueueAnimation(new UpdateLife_GUI(player.life, player.maxLife, Team.Me));
            }
            else
            {
                enemy.life += value;
                if (enemy.life > enemy.maxLife)
                {
                    enemy.life = enemy.maxLife;
                }
                EventManager.Instance.QueueAnimation(new UpdateLife_GUI(enemy.life, player.maxLife, Team.Opponent));

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
            EventManager.Instance.TriggerEvent(new TableCard_Trigger(turn, CardEffect.Trigger.EndOfTurn));

            if (turn == Team.Opponent)
            {
                
                DeckManager.player.Draw();
                turn = Team.Me;
                player.mana = player.maxMana;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(player.mana, player.maxMana, Team.Me));
            }
            else if (turn == Team.Me)
            {
                DeckManager.monster.Draw();
                turn = Team.Opponent;
                enemy.mana = enemy.maxMana;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(enemy.mana, enemy.maxMana, Team.Opponent));
                enemy.initAI();
            }


        }


        public void PlaceCard(CardManager card)
        {

            //Pay the mana cost.
            Debug.Log("Playing a new card: " + card.card.cardName);
            if (card.owner == Team.Me)
            {
                player.mana = player.mana - card.card.cost;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(player.mana, player.maxMana, card.owner));
            }
            else
            {
                enemy.mana = enemy.mana - card.card.cost;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(enemy.mana, enemy.maxMana, card.owner));
            }


            //Check for triggers from playing a card.
            Debug.Log("Start checking for triggers on play card");
            EventManager.Instance.TriggerEvent(new TableCard_Trigger(card.owner, CardEffect.Trigger.OnPlayCard));
            Debug.Log("Done checking for triggers");

            //Apply the cards effects if they are instant.
            Debug.Log("Start Applying Card effects");


            foreach (CardEffect effect in card.card.effects)
            {
                if (effect.trigger == CardEffect.Trigger.Instant || effect.trigger == CardEffect.Trigger.Passive)
                {
                    card.ApplyEffect(effect);

                }
            }
            Debug.Log("Done applying Card effects");


            //Set the new card position.
            Debug.Log("Start moving the card.");
            if (card.card.cardDuration == 0)
            {
                Debug.Log("Duration = 0, startpoint tabletop endpoint discard");
                card.SetCardPosition(CardManager.CardStatus.InDiscard);
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(card, tabletop, card.discard));
              
            }
            else
            {
                card.SetCardPosition(CardManager.CardStatus.OnTable);
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(card, tabletop, card.table));
               
            }


            //Queue up a move card event to move the card to it's final destination.
            

        }
    }
}