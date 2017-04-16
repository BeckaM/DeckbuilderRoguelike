using UnityEngine;
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
                if (team == CardgameManager.Team.Me)
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
                        //  cardsInDeck.Add(card.gameObject);
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

            AddCardstoDeck(cardobjects);

        }

        public void AddCardstoDeck(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                CreateCardObject(card);

            }
        }

        internal void InitDeck()
        {
            cardsInDeck = new List<GameObject>();
            var cardobjects = ObjectDAL.GetCards(deckStringHolder);
            foreach (Card card in cardobjects)
            {
                CreateCardObject(card);
            }
        }


        public GameObject AddCardtoDeck(string cardToCreate)
        {
            var cardobject = ObjectDAL.GetCard(cardToCreate);
            deckStringHolder.Add(cardobject.cardName);
            //  var deck = this.transform;

            var card = CreateCardObject(cardobject);
            return card;
        }

        public GameObject AddCardtoDeck(Card cardToCreate)
        {
            deckStringHolder.Add(cardToCreate.cardName);
            //  var deck = this.transform;
            var card = CreateCardObject(cardToCreate);
            return card;
        }


        private GameObject CreateCardObject(Card card)
        {
            GameObject instance = Instantiate(cardObject) as GameObject;
            cardManager = instance.GetComponent<CardManager>();

            cardManager.owner = team;
            cardsInDeck.Add(cardManager.gameObject);
            instance.transform.SetParent(deckHolder.transform);
            instance.transform.localScale = new Vector3(1f, 1f, 1f);

            cardManager.PopulateCard(card);
            return instance;
        }


        public void DiscardRandomCard()
        {
            if (cardsInHand.Count > 0)
            {
                var card = cardsInHand[Random.Range(0, cardsInHand.Count)];
                cardsInHand.Remove(card);
                var manager = card.GetComponent<CardManager>();
                manager.SetCardPosition(CardManager.CardStatus.InDiscard);

                EventManager.Instance.AddListener<MoveCard_GUI>(manager.Move);
                manager.moveCounter++;
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(manager, hand, manager.discard));


                //Check for objects that trigger on discard card.
                EventManager.Instance.TriggerEvent(new TableCard_Trigger(manager.owner, CardEffect.Trigger.OnDiscard));
            }
        }

        internal List<GameObject> GetMuliganCards()
        {
            int muliganCount = 3 + GameManager.instance.perkManager.bonusInitialDraw;
            List<GameObject> muliganCards = new List<GameObject>();
            for (var i = 0; i < muliganCount; i++)
            {
                if (cardsInDeck.Count > 0)
                {
                    int random = Random.Range(0, cardsInDeck.Count);
                    GameObject tempCard = cardsInDeck[random];
                    muliganCards.Add(tempCard);
                    cardsInDeck.Remove(tempCard);
                    tempCard.GetComponent<Selectable>().muliganKeep = true;
                }
            }
            return muliganCards;
        }
        public void Draw()
        {
            if (cardsInDeck.Count > 0)
            {
                //Draw card logic.
                int random = Random.Range(0, cardsInDeck.Count);
                GameObject tempCard = cardsInDeck[random];

                //Prepare the card for being moved
                CardManager manager = tempCard.GetComponent<CardManager>();
                manager.SetCardPosition(CardManager.CardStatus.InHand);

                cardsInDeck.Remove(tempCard);

                //Queue up a move card animation.
                EventManager.Instance.AddListener<MoveCard_GUI>(manager.Move);
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(manager, deckManager, hand));
                manager.moveCounter++;

                EventManager.Instance.QueueAnimation(new UpdateDeckTexts_GUI(cardsInDeck.Count, cardsInDiscard.Count, team));

                //Check for objects that trigger on draw card.
                EventManager.Instance.TriggerEvent(new TableCard_Trigger(manager.owner, CardEffect.Trigger.OnDraw));
            }
            else
            {
                ShuffleDeck();
                if (cardsInDeck.Count != 0)
                {
                    Draw();
                }
                else
                {
                    Debug.Log("You have no cards in your deck!");
                }
            }
        }


        private void ShuffleDeck()
        {
            foreach (GameObject card in cardsInDiscard)
            {
                CardManager manager = card.GetComponent<CardManager>();

                manager.SetCardPosition(CardManager.CardStatus.InDeck);
            }
            cardsInDiscard.Clear();
            EventManager.Instance.QueueAnimation(new UpdateDeckTexts_GUI(cardsInDeck.Count, cardsInDiscard.Count, team));
        }


        public void DestroyCard(GameObject card)
        {
            cardsInDeck.Remove(card);
            deckStringHolder.Remove(card.GetComponent<CardManager>().card.cardName);
            Destroy(card);
        }

        public void DestroyRandomCard()
        {
            if (cardsInDeck.Count != 0)
            {
                //Destroy card logic.
                int random = Random.Range(0, cardsInDeck.Count);
                GameObject tempCard = cardsInDeck[random];

                DestroyCard(tempCard);
            }
        }
    }
}


