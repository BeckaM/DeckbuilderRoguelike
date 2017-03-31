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
        public GameObject selectedCard;

        void Awake()
        {
            instance = this;

        }

        void OnEnable()
        {
            EventManager.Instance.AddListener<UpdateMana_GUI>(GUIUpdateMana);
            EventManager.Instance.AddListener<UpdateLife_GUI>(GUIUpdateLife);
            EventManager.Instance.AddListener<ApplyDamage_GUI>(ApplyDamageGUI);
            EventManager.Instance.AddListener<UpdateDeckTexts_GUI>(GUIUpdateDeckDiscardText);
            EventManager.Instance.AddListener<EndGame_GUI>(EndGame);
        }

        void OnDisable()
        {
            EventManager.Instance.RemoveAll();
        }

        // Use this for initialization
        public void Setup()
        {
            //Sets opponent profile images and texts.
            SetOpponents();

            turn = Team.Me;

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

            player.life = GameManager.instance.lifeHolder;
            player.maxLife = GameManager.instance.maxLife;

            //enemy.UpdateLife();
            monsterPortrait.sprite = enemy.monsterRenderer.sprite;

            // player.UpdateLife();            
            player.playerImage = player.sprites[GameManager.instance.playerClass.SpriteIcon];

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
            }
            else
            {
                enemy.ward += value;
            }
        }


        internal void IncreaseDamage(int value, Team team)
        {
            if (team == Team.Me)
            {
                player.damageBoost += value;
            }
            else
            {
                enemy.damageBoost += value;
            }
        }


        internal void ApplyDamage(int value, Team team)
        {
            if (team == Team.Me)
            {                
                enemy.life -= ((value + player.damageBoost) - enemy.ward) > 0 ? ((value + player.damageBoost) - enemy.ward) : 0;
                EventManager.Instance.QueueAnimation(new ApplyDamage_GUI(value - enemy.ward, enemy.ward, player.damageBoost, Team.Opponent));
                EventManager.Instance.QueueAnimation(new UpdateLife_GUI(enemy.life, enemy.maxLife, Team.Opponent));
                GameManager.instance.progressManager.DamageDealt(((value + player.damageBoost) - enemy.ward) > 0 ? ((value + player.damageBoost) - enemy.ward) : 0);
            }
            else
            {
                player.life -= ((value + enemy.damageBoost) - player.ward) > 0 ? ((value + enemy.damageBoost) - player.ward): 0;
                EventManager.Instance.QueueAnimation(new ApplyDamage_GUI(value - player.ward, player.ward, enemy.damageBoost, Team.Me));
                EventManager.Instance.QueueAnimation(new UpdateLife_GUI(player.life, player.maxLife, Team.Me));
            }
        }


        internal void ApplyDamageGUI(ApplyDamage_GUI a)
        {
            var damageText = "" + a.damage + " Damage!";
            var reduceText = "" + a.reduced + " Reduced";
            var effectText = "";
            if (a.reduced > 0)
            {
                effectText = damageText + " " + reduceText;
            }
            else
            {
                effectText = damageText;
            }

            if (a.team == Team.Me)
            {
                StartCoroutine(EffectText(Color.red, playerLifeText, effectText, 3));
            }
            else
            {
                StartCoroutine(EffectText(Color.red, monsterLifeText, effectText, 3));
            }
        }


        private IEnumerator EffectText(Color color, Text textObject, string text, int loops)
        {
            var effectText = textObject.GetComponent<Text>();
            effectText.text = text;
            effectText.color = color;

            for (var n = 0; n < loops; n++)
            {
                effectText.color = Color.white;
                yield return new WaitForSeconds(.1f);
                effectText.color = color;
                yield return new WaitForSeconds(.1f);
            }
            EventManager.Instance.processingQueue = false;
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
                GameManager.instance.progressManager.Healing(value);
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


        private void CheckWinConditions()
        {
            bool win;

            if (player.life <= 0)
            {
                turn = Team.None;
                win = false;
                EventManager.Instance.QueueAnimation(new EndGame_GUI(win));
            }
            else if (enemy.life <= 0)
            {
                turn = Team.None;
                win = true;
                EventManager.Instance.QueueAnimation(new EndGame_GUI(win));
            }
        }


        private void EndGame(EndGame_GUI end)
        {
            Card cardReward = new Card();
            int goldReward;

            List<Card> monsterCards = new List<Card>();

            foreach (GameObject CardObject in GameObject.FindGameObjectsWithTag("Card"))
            {
                CardManager card = CardObject.GetComponent<CardManager>();

                if (card.owner == Team.Opponent)
                {
                    monsterCards.Add(card.card);
                }
            }
            cardReward = monsterCards[Random.Range(0, monsterCards.Count)];
            goldReward = Random.Range(5 + enemy.enemy.BaseEnemyLevel, 10 + enemy.enemy.BaseEnemyLevel);

            DeckManager.player.Cleanup();
            DeckManager.monster.Cleanup();

            player.mana = 1;
            player.maxMana = 1;
            player.ward = 0;
            player.damageBoost = 0;

            GameManager.instance.GainXP(enemy.experienceReward);
            GameManager.instance.lifeHolder = player.life;
            GameManager.instance.ReturnFromCardgame(end.playerWon, cardReward, goldReward);

            EventManager.Instance.processingQueue = false;

            this.gameObject.SetActive(false);
        }


        internal void Select(GameObject selectedCard)
        {
            if (this.selectedCard)
            {

                if (selectedCard == this.selectedCard)
                {
                    selectedCard.GetComponent<CardManager>().imagePanel.ShowFullDescription(false);
                    selectedCard.GetComponent<Selectable>().outline.enabled = false;
                    this.selectedCard = null;
                    return;
                }

                this.selectedCard.GetComponent<Selectable>().outline.enabled = false;
                this.selectedCard.GetComponent<CardManager>().imagePanel.ShowFullDescription(false);
            }
            this.selectedCard = selectedCard;
            //   selectedCard.GetComponent<CardManager>().cardDescription.SetActive(true);
            selectedCard.GetComponent<Selectable>().outline.enabled = true;
            //   selectedCard.GetComponent<CardManager>().descriptionPanel.SetActive(true);
            selectedCard.GetComponent<CardManager>().imagePanel.ShowFullDescription(true);
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
                DeckManager.player.cardsInHand.Remove(card.gameObject);
                player.mana = player.mana - card.card.cost;
                EventManager.Instance.QueueAnimation(new UpdateMana_GUI(player.mana, player.maxMana, card.owner));
            }
            else
            {
                DeckManager.monster.cardsInHand.Remove(card.gameObject);
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
                    EventManager.Instance.QueueAnimation(new MoveCard_GUI(card, tabletop, card.discard));
                    card.moveCounter++;
                }
            }
            else
            {
                card.SetCardPosition(CardManager.CardStatus.OnTable);

                EventManager.Instance.AddListener<MoveCard_GUI>(card.Move);
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(card, tabletop, card.table));
                card.moveCounter++;
            }
            CheckWinConditions();
            GameManager.instance.progressManager.CardPlayed();
        }
    }
}