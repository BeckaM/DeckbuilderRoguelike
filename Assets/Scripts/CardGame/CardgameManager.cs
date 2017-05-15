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

        public enum Team { Me, Opponent, None };
        public Team turn = Team.Me;

        public CardgameUI cardgameUI;
        
        public GameObject playerDiscard;
        public GameObject monsterDiscard;

        public GameObject playerTable;
        public GameObject monsterTable;

        public GameObject playerHand;
        public GameObject monsterHand;

        public GameObject playerDeck;
        public GameObject monsterDeck;

        public GameObject tabletop;

        public EnemyManager enemy;
        public Player player;
        public GameObject selectedCard;

        public Camera cam;
      
        void Awake()
        {
            instance = this;
        }
        
        // Use this for initialization
        public void Setup()
        {
            this.gameObject.SetActive(true);
            cardgameUI.gameObject.SetActive(true);

            //Sets opponent profile images and texts.
            SetOpponents();

            turn = Team.Me;
            cardgameUI.endTurnButton.interactable = true;

            //Update life and mana in the GUI.
            cardgameUI.ResetUI(player, enemy);
            
            //Draw our starting hand
            ShowMuligan();            
        }
        
        public void DrawStartingHands(int cardsMuliganed)
        {
            
            for (var i = 0; i < cardsMuliganed; i++)
            {
                DeckManager.player.Draw();               
            }

            var monsterStarthand = 3;
            for (var i = 0; i < monsterStarthand; i++)
            {                
                DeckManager.monster.Draw();
            }
        }

        private void ShowMuligan()
        {
            cardgameUI.muliganPanelScript.StartMuligan();
        }

        private void SetOpponents()
        {
            player.mana = 1;
            enemy.mana = 0;
            /* Old code for non stacking mana
             * player.mana = player.maxMana;
            enemy.mana = enemy.maxMana;
            */           
            //cardgameUI.monsterPortrait.sprite = enemy.monsterRenderer.sprite;
            //cardgameUI.monsterPortrait.color = enemy.enemy.spriteColor;
                        
           // cardgameUI.playerPortrait.sprite = player.playerImage;
            cardgameUI.monsterNameText.text = enemy.enemy.EnemyName;
        }


        internal void IncreaseMaxMana(int value, Team team)
        {
            if (team == Team.Me)
            {
                player.manaPerTurn += value;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(player.mana, player.maxMana, Team.Me));
            }
            else
            {
                enemy.manaPerTurn += value;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(enemy.mana, enemy.maxMana, Team.Opponent));
            }
        }


        internal void IncreaseDamageReduction(int value, Team team)
        {
            if (team == Team.Me)
            {
                player.ward += value;
                EventManager.Instance.QueueAnimation(new UpdateArmor_GUI(player.ward, Team.Me));
            }
            else
            {
                enemy.ward += value;
                EventManager.Instance.QueueAnimation(new UpdateArmor_GUI(enemy.ward, Team.Opponent));
            }
        }


        internal void IncreaseDamage(int value, Team team)
        {
            if (team == Team.Me)
            {
                player.damageBoost += value;
                EventManager.Instance.QueueAnimation(new UpdateDamageIncrease_GUI(player.damageBoost, Team.Me));
            }
            else
            {
                enemy.damageBoost += value;
                EventManager.Instance.QueueAnimation(new UpdateDamageIncrease_GUI(enemy.damageBoost, Team.Opponent));
            }
        }


        internal void ApplyDamage(int value, bool ignoreArmor, Team team)
        {            
            if (team == Team.Me)
            {
                int amount;
                if (ignoreArmor)
                {
                    amount = value + player.damageBoost;
                }
                else
                {
                    amount = ((value + player.damageBoost) - enemy.ward) > 0 ? ((value + player.damageBoost) - enemy.ward) : 0;
                }
                enemy.life -= amount;
                EventManager.Instance.QueueAnimation(new ApplyDamage_GUI(amount, Team.Opponent));
                EventManager.Instance.QueueAnimation(new UpdateLife_GUI(enemy.life, enemy.maxLife, Team.Opponent));
                GameManager.instance.progressManager.CumulativeMetric(ProgressManager.Metric.Damage_Dealt, amount);
            }
            else
            {
                int amount;
                if (ignoreArmor)
                {
                    amount = value + enemy.damageBoost;
                }
                else
                {
                    amount = ((value + enemy.damageBoost) - player.ward) > 0 ? ((value + enemy.damageBoost) - player.ward) : 0;
                }
                player.life -= amount;
                EventManager.Instance.QueueAnimation(new ApplyDamage_GUI(amount, Team.Me));
                EventManager.Instance.QueueAnimation(new UpdateLife_GUI(player.life, player.maxLife, Team.Me));
            }
            CheckWinConditions();
        }

        internal void PlayerCleanup()
        {
            player.mana = 1;
            player.maxMana = 10;
            player.ward = 0;
            player.damageBoost = 0;
            player.manaPerTurn = 1; 
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
                GameManager.instance.progressManager.CumulativeMetric(ProgressManager.Metric.Healing_Done, value);
            }
            else
            {
                enemy.life += value;
                if (enemy.life > enemy.maxLife)
                {
                    enemy.life = enemy.maxLife;
                }
                EventManager.Instance.QueueAnimation(new UpdateLife_GUI(enemy.life, enemy.maxLife, Team.Opponent));
            }
        }


        private void CheckWinConditions()
        {
            bool win;

            if (player.life <= 0)
            {
                turn = Team.None;
                win = false;
                EndGame(win);
            }
            else if (enemy.life <= 0)
            {
                turn = Team.None;
                win = true;
                EndGame(win);
            }
        }


        private void EndGame(bool win)
        {
            List<Card> cardRewards = new List<Card>();
            
            List<Card> monsterCards = new List<Card>();

            foreach (GameObject CardObject in GameObject.FindGameObjectsWithTag("Card"))
            {
                CardManager card = CardObject.GetComponent<CardManager>();

                if (card.owner == Team.Opponent)
                {
                    monsterCards.Add(card.card);
                }
            }

            for (var i = 0; i < 3; i++)
            {
                var cardReward = monsterCards[Random.Range(0, monsterCards.Count)];
                cardRewards.Add(cardReward);
            }

            float baseGold = Random.Range(10 + enemy.enemy.BaseEnemyLevel, 20 + enemy.enemy.BaseEnemyLevel);
            var bonusGold = Math.Ceiling(baseGold * GameManager.instance.perkManager.goldIncrease);

            var goldReward = (int)bonusGold;
           
            if(enemy.enemy.type == Enemy.MonsterType.Boss)
            {
                GameManager.instance.dungeonManager.exit.SetActive(true);

            }
            
            GameManager.instance.GainXP(enemy.experienceReward);
           
            EventManager.Instance.QueueAnimation(new EndGame_GUI(win, cardRewards, goldReward));
        }
            
        
        //Triggered by end turn button.
        public void EndTurn()
        {            
            EventManager.Instance.TriggerEvent(new TableCard_Trigger(turn, CardEffect.Trigger.EndOfTurn));

            if (turn == Team.Opponent)
            {
                Debug.Log("AI ended the turn");
                DeckManager.player.Draw();
                turn = Team.Me;
                player.mana += player.manaPerTurn;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(player.mana, player.maxMana, Team.Me));
                cardgameUI.endTurnButton.interactable = true;
            }
            else if (turn == Team.Me)
            {
                cardgameUI.endTurnButton.interactable = false;
                DeckManager.monster.Draw();
                turn = Team.Opponent;
                enemy.mana += enemy.manaPerTurn;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(enemy.mana, enemy.maxMana, Team.Opponent));

                StartCoroutine(enemy.initAI());
                Debug.Log("Turn was ended, AI starting to play.");
               
            }
        }


        public void PlaceCard(CardManager card)
        {          
            //Pay the mana cost.
            Debug.Log("Playing a new card: " + card.card.cardName);
            if (card.owner == Team.Me)
            {
                DeckManager.player.cardsInHand.Remove(card);
                player.mana = player.mana - card.card.cost;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(player.mana, player.maxMana, card.owner));
                GameManager.instance.progressManager.CumulativeMetric(ProgressManager.Metric.Cards_Played, 1);
            }
            else
            {
                DeckManager.monster.cardsInHand.Remove(card);
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
                if (card.card.type == Card.Type.Consumable)
                {
                    EventManager.Instance.AddListener<DestroyCard_GUI>(card.DestroyCardGUI);
                    EventManager.Instance.QueueAnimation(new DestroyCard_GUI(card));
                }
                else
                {
                    Debug.Log("Duration = 0, startpoint tabletop endpoint discard");
                    card.SetCardPosition(CardManager.CardStatus.InDiscard);

                    EventManager.Instance.AddListener<MoveCard_GUI>(card.Move);
                    EventManager.Instance.QueueAnimation(new MoveCard_GUI(card, tabletop, card.Discard));
                    card.moveCounter++;
                }
            }
            else
            {
                card.SetCardPosition(CardManager.CardStatus.OnTable);

                EventManager.Instance.AddListener<MoveCard_GUI>(card.Move);
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(card, tabletop, card.Table));
                card.moveCounter++;
            }                    
        }
    }
}