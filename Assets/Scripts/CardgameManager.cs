using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

namespace Assets.Scripts
{

    public class CardgameManager : MonoBehaviour
    {

        public static CardgameManager instance;


        public enum Turn { MyTurn, AITurn };
        public Turn turn = Turn.MyTurn;

        public int PlayerHP;

        int maxMana = 1;
        public int MyMana = 1;
        public int AIMana = 1;

        public List<GameObject> MyDeckCards = new List<GameObject>();
        public List<GameObject> MyHandCards = new List<GameObject>();
        public List<GameObject> MyTableCards = new List<GameObject>();

        public List<GameObject> AIDeckCards = new List<GameObject>();
        public List<GameObject> AIHandCards = new List<GameObject>();
        public List<GameObject> AITableCards = new List<GameObject>();

        public EnemyManager enemy;

        void Awake()
        {
            instance = this;

        }


        // Use this for initialization
        void Start()
        {

            PlayerHP = GameManager.instance.life;

            //Organize the cards into the correct lists.
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

            //Draw our starting hand


            for (var i = 0; i < 3; i++)
            {
                DrawCardFromDeck(CardManager.Team.My);
                DrawCardFromDeck(CardManager.Team.AI);
            }

            UpdateGame();
        }

        internal void ApplyDamage(int value, CardManager.Team team)
        {
            if (team == CardManager.Team.My)
            {
                enemy.MonsterHP = enemy.MonsterHP - value;
            }
            else
            {
                PlayerHP = PlayerHP - value;
            }



        }

        void UpdateGame()
        {
            //MyManaText.text = MyMana.ToString() + "/" + maxMana;
            //AIManaText.text = AIMana.ToString() + "/" + maxMana;

            if (PlayerHP <= 0)
                EndGame();
            if (enemy.MonsterHP <= 0)
                EndGame();

            foreach (GameObject Card in MyHandCards)
            {
                CardManager c = Card.GetComponent<CardManager>();
                if (c.card.Cost <= MyMana && turn == Turn.MyTurn)
                {
                    c.Playable = true;

                }
            }
            foreach (GameObject Card in AIHandCards)
            {
                CardManager c = Card.GetComponent<CardManager>();
                if (c.card.Cost <= AIMana && turn == Turn.AITurn)
                {
                    c.Playable = true;

                }
            }

        }

        private void EndGame()
        {
            throw new NotImplementedException();
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
        }

        public void PlaceCard(CardManager card)
        {


            card.SetCardStatus(CardManager.CardStatus.OnTable);
            //PlaySound(cardDrop);

            MyHandCards.Remove(card.gameObject);
            MyTableCards.Add(card.gameObject);

            foreach (CardEffect effect in card.card.Effects)
            {
                if (effect.trigger == CardEffect.Trigger.Instant)
                {
                    card.ApplyEffect(effect);
                }
            }


            //   CheckTriggers(CardEffect.Trigger.OnPlayCard, card.team);




        }

        private void CheckTriggers(CardEffect.Trigger triggertype, CardManager.Team team)
        {
            throw new NotImplementedException();
        }
    }
}