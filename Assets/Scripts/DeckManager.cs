﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using System.IO;
using Assets.Scripts.DAL;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{

    public class DeckManager : MonoBehaviour
    {
        public static DeckManager player = null;
        public static DeckManager monster = null;
        CardManager cardManager;
        public string instancename;
        public GameObject deckHolder
        {
            get
            {
                if(team == CardgameManager.Team.Me)
                {
                    return GameManager.instance.deckPanel.cardArea;
                }
                else
                {
                    return this.gameObject;
                }
            }

        }

        public List<string> deckStringHolder;

        public GameObject deckManager
        {
            get
            {
                if (team == CardgameManager.Team.Me)
                {
                    return CardgameManager.instance.playerDeck;
                }
                else
                {
                    return CardgameManager.instance.monsterDeck;
                }
            }
        }

        public GameObject hand
        {
            get
            {
                if (team == CardgameManager.Team.Me)
                {
                    return CardgameManager.instance.playerHand;
                }
                else
                {
                    return CardgameManager.instance.monsterHand;
                }
            }
        }

        public GameObject cardObject;
        public CardgameManager.Team team;
        public List<GameObject> cardsInDeck = new List<GameObject>();
        public List<GameObject> cardsInDiscard = new List<GameObject>();
        //public List<GameObject> cardsOnTable = new List<GameObject>();
        public List<GameObject> cardsInHand = new List<GameObject>();

        // Use this for initialization
        void Awake()
        {
            name = this.gameObject.name;
            if (name == "GameManager(Clone)")
            {


                instancename = "Me";
                player = this;
                team = CardgameManager.Team.Me;
                DontDestroyOnLoad(gameObject);

            }
            //If instance already exists and it's not this:
            else if (name == "MonsterDeck")
            {
                instancename = "AI";
                monster = this;

                team = CardgameManager.Team.Opponent;

            }

        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;

        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {

        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;

        }

        public void Cleanup()
        {
            cardsInDeck = new List<GameObject>();
            cardsInDiscard = new List<GameObject>();
            cardsInHand = new List<GameObject>();
            if (team == CardgameManager.Team.Me)
            {
                foreach (GameObject CardObject in GameObject.FindGameObjectsWithTag("Card"))
                {
                    CardManager card = CardObject.GetComponent<CardManager>();

                    if (card.owner == CardgameManager.Team.Me)
                    {
                        card.moveCounter = 0;
                        card.effectCounter = 0;
                        card.transform.SetParent(deckHolder.transform);
                       // card.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                        card.SetCardPosition(CardManager.CardStatus.InDeck);
                        cardsInDeck.Add(card.gameObject);
                    }

                    else
                    {
                        Destroy(CardObject);
                    }
                }
            }
        }


        public void StartingDeck(List<string> startingCards)
        {
            deckStringHolder = new List<string>();
            deckStringHolder.AddRange(startingCards);
        }


        public void AddCardstoDeck(List<string> cardsToCreate)
        {
            deckStringHolder.AddRange(cardsToCreate);
            var cardobjects = ObjectDAL.GetCards(cardsToCreate);
            //   var deck = this.transform;


            foreach (Card card in cardobjects)
            {
                CreateCardObject(card);

            }
        }

        internal void InitDeck()
        {
            var cardobjects = ObjectDAL.GetCards(deckStringHolder);
            foreach (Card card in cardobjects)
            {
                CreateCardObject(card);
            }
        }


        public void AddCardtoDeck(string cardToCreate)
        {
            deckStringHolder.Add(cardToCreate);
            var cardobject = ObjectDAL.GetCard(cardToCreate);
            //  var deck = this.transform;
            CreateCardObject(cardobject);
        }


        private void CreateCardObject(Card card)
        {
            GameObject instance = Instantiate(cardObject) as GameObject;
            cardManager = instance.GetComponent<CardManager>();
            

            cardManager.owner = team;
            cardsInDeck.Add(cardManager.gameObject);
            instance.transform.SetParent(deckHolder.transform);
            instance.transform.localScale = new Vector3(1f, 1f, 1f);

            cardManager.PopulateCard(card);
        }


        public void Draw()
        {
            if (cardsInDeck.Count != 0)
            {

                //Draw card logic.
                int random = Random.Range(0, cardsInDeck.Count);
                GameObject tempCard = cardsInDeck[random];

                //Prepare the card for being moved
                CardManager manager = tempCard.GetComponent<CardManager>();
                manager.SetCardPosition(CardManager.CardStatus.InHand);
                //manager.startPoint = deckholder;
                //manager.endPoint = hand;

                cardsInDeck.Remove(tempCard);
                // cardsInHand.Add(tempCard);

                //Queue up a move card animation.
                EventManager.Instance.AddListener<MoveCard_GUI>(manager.Move);
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(manager, deckManager, hand));
                manager.moveCounter++;

                EventManager.Instance.QueueAnimation(new UpdateDeckTexts_GUI(cardsInDeck.Count, cardsInDiscard.Count, team));
                //Check for objects that trigger on draw card.
                EventManager.Instance.TriggerEvent(new TableCard_Trigger(manager.owner, CardEffect.Trigger.OnDraw));
            }
        }

        public void DestroyCard(GameObject card)
        {
            cardsInDeck.Remove(card);
            Destroy(card);
        }

        public void DestroyRandomCard()
        {
            if (cardsInDeck.Count != 0)
            {
                //Destroy card logic.
                int random = Random.Range(0, cardsInDeck.Count);
                GameObject tempCard = cardsInDeck[random];

                cardsInDeck.Remove(tempCard);
                Destroy(tempCard);


            }
        }
    }




}


