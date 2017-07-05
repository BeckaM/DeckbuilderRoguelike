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
                return GameManager.instance.dungeonUI.deckPanel.cardArea;
            }
        }


        public List<string> deckStringHolder;

        public int deckOffset;
        public GameObject deck
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
        public int discardOffset;
        public GameObject discard
        {
            get
            {
                if (team == CardgameManager.Team.Me)
                {
                    return CardgameManager.instance.playerDiscard;
                }
                else
                {
                    return CardgameManager.instance.monsterDiscard;
                }
            }
        }

        public GameObject cardObject;
        public CardgameManager.Team team;
        public List<CardManager> cardsInDeck = new List<CardManager>();
        public List<CardManager> cardsInDiscard = new List<CardManager>();
        //public List<GameObject> cardsOnTable = new List<GameObject>();
        public List<CardManager> cardsInHand = new List<CardManager>();

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
            else if (name == "CardGame")
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
            cardsInDeck = new List<CardManager>();
            cardsInDiscard = new List<CardManager>();
            cardsInHand = new List<CardManager>();
            if (team == CardgameManager.Team.Me)
            {
                foreach (GameObject CardObject in GameObject.FindGameObjectsWithTag("Card"))
                {
                    CardManager card = CardObject.GetComponent<CardManager>();

                    if (card.owner == CardgameManager.Team.Me)
                    {
                        card.moveCounter = 0;
                        card.effectCounter = 0;
                        card.transform.SetParent(deckHolder.transform, false);
                        //card.transform.localScale = new Vector3(1f, 1f, 1f);
                        card.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
                        card.SetCardPosition(CardManager.CardStatus.InDeck);
                        // card.ResetCard(deck);
                        card.bottomPanel.ShowBottomPanel(true);
                      //  card.imagePanel.ResetPanel();
                        card.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 320);
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


        //public void AddCardstoDeck(List<string> cardsToCreate)
        //{
        //    deckStringHolder.AddRange(cardsToCreate);
        //    var cardobjects = ObjectDAL.GetCards(cardsToCreate);
        //    //   var deck = this.transform;

        //    AddCardstoDeck(cardobjects);

        //}

        public void AddCardstoDeck(List<CardManager> cards)
        {
            foreach (CardManager card in cards)
            {
                AddCardtoDeck(card);
            }
        }


        internal void InitDeck()
        {
            cardsInDeck = new List<CardManager>();
            var cardobjects = ObjectDAL.GetCards(deckStringHolder);

            foreach (Card card in cardobjects)
            {
                var cardMan = CreateCardObject(card);
                cardMan.owner = CardgameManager.Team.Me;
                cardsInDeck.Add(cardMan);
                cardMan.transform.SetParent(deckHolder.transform, false);

            }
        }

        internal void InitCardGameDeck()
        {
            deckOffset = 0;
            discardOffset = 0;
            cardsInDeck.Shuffle(new System.Random());
            foreach (CardManager card in cardsInDeck)
            {
                card.transform.SetParent(deck.transform, false);
                                
                card.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, deckOffset);
                deckOffset -= 2;

                //  card.GetComponent<CardManager>().ResetTransform();
            }
        }


        public CardManager AddCardtoDeck(CardManager card)
        {
            //var cardobject = ObjectDAL.GetCard(cardToCreate);
            deckStringHolder.Add(card.card.cardName);
            cardsInDeck.Add(card);
            cardManager.owner = team;
            card.transform.SetParent(deckHolder.transform, false);

            //  var deck = this.transform;

            //var card = CreateCardObject(cardobject);
            return card;
        }


        //public GameObject AddCardtoDeck(Card cardToCreate)
        //{
        //    deckStringHolder.Add(cardToCreate.cardName);
        //    //  var deck = this.transform;
        //    var card = CreateCardObject(cardToCreate);
        //    return card;
        //}


        internal CardManager CreateCardObject(Card card)
        {
            GameObject instance = Instantiate(cardObject) as GameObject;
            cardManager = instance.GetComponent<CardManager>();

            cardManager.PopulateCard(card);
            return cardManager;
        }

        internal List<CardManager> CreateCardObjects(List<Card> cards)
        {
            var returnList = new List<CardManager>();
            foreach (Card card in cards)
            {
                var cardManager = CreateCardObject(card);
                returnList.Add(cardManager);

            }
            return returnList;
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
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(manager, hand, manager.Discard));


                //Check for objects that trigger on discard card.
                EventManager.Instance.TriggerEvent(new TableCard_Trigger(manager.owner, CardEffect.Trigger.OnDiscard));
            }
        }

        private CardManager GetTopCard()
        {
            var topCard = cardsInDeck[cardsInDeck.Count - 1];
            return topCard;
        }


        internal List<CardManager> GetMuliganCards()
        {
            int muliganCount = 4 + GameManager.instance.itemManager.bonusInitialDraw;
            List<CardManager> muliganCards = new List<CardManager>();
            for (var i = 0; i < muliganCount; i++)
            {
                if (cardsInDeck.Count > 0)
                {

                    CardManager tempCard = GetTopCard();
                    muliganCards.Add(tempCard);
                    cardsInDeck.Remove(tempCard);
                    deckOffset += 2;
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
                CardManager tempCard = GetTopCard();

                //Prepare the card for being moved
                tempCard.SetCardPosition(CardManager.CardStatus.InHand);
                cardsInDeck.Remove(tempCard);
                
                //Queue up a move card animation.
                EventManager.Instance.AddListener<MoveCard_GUI>(tempCard.Move);
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(tempCard, deck, hand));
                tempCard.moveCounter++;

                //  EventManager.Instance.QueueAnimation(new UpdateDeckTexts_GUI(cardsInDeck.Count, cardsInDiscard.Count, team));

                //Check for objects that trigger on draw card.
                EventManager.Instance.TriggerEvent(new TableCard_Trigger(tempCard.owner, CardEffect.Trigger.OnDraw));
            }
            else
            {
                ShuffleDiscardIntoDeck();
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


        private void ShuffleDiscardIntoDeck()
        {
            foreach (CardManager card in cardsInDiscard)
            {
                card.SetCardPosition(CardManager.CardStatus.InDeck);
                EventManager.Instance.AddListener<MoveCard_GUI>(card.Move);
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(card, discard, deck));
                card.moveCounter++;
            }
            cardsInDiscard.Clear();
        //    discardOffset = 0;

            //     EventManager.Instance.QueueAnimation(new UpdateDeckTexts_GUI(cardsInDeck.Count, cardsInDiscard.Count, team));
        }


        public void DestroyCard(CardManager card)
        {
            cardsInDeck.Remove(card);
            deckStringHolder.Remove(card.GetComponent<CardManager>().card.cardName);
            Destroy(card.gameObject);
        }

        public void DestroyRandomCard()
        {
            if (cardsInDeck.Count != 0)
            {
                //Destroy card logic.
                int random = Random.Range(0, cardsInDeck.Count);
                CardManager tempCard = cardsInDeck[random];

                DestroyCard(tempCard);
            }
        }
    }
}


