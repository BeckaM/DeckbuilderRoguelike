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

        public GameObject deckholder
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
        public List<GameObject> cardsOnTable = new List<GameObject>();
        public List<GameObject> cardsInHand = new List<GameObject>();

        // Use this for initialization
        void Awake()
        {
            name = this.gameObject.name;
            if (name == "PlayerDeck(Clone)")
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


        ////public void Cleanup()
        ////{
        ////    foreach(GameObject CardObject in GameObject.FindGameObjectsWithTag("Card"))
        ////    { 
        ////        CardManager card = CardObject.GetComponent<CardManager>();

        ////        if (card.team == CardManager.Team.My)
        ////        {
        ////            card.transform.SetParent(playerDeckHolder.transform);
        ////            card.SetCardStatus(CardManager.CardStatus.InDeck);

        ////        }


        ////        else
        ////        {
        ////            Destroy(CardObject);
        ////        }
        ////    }
        ////}



        public void StartingDeck(List<string> cardstoCreate)
        {

            AddCardtoDeck(cardstoCreate);

        }

        public void AddCardtoDeck(List<string> cardsToCreate)
        {
            var cardobjects = ObjectDAL.GetCards(cardsToCreate);
            var deck = this.transform;


            foreach (Card card in cardobjects)
            {

                GameObject instance = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                cardManager = instance.GetComponent<CardManager>();


                cardManager.owner = team;
                cardsInDeck.Add(cardManager.gameObject);
                instance.transform.SetParent(this.transform);

                cardManager.PopulateCard(card);
            }
        }
        public void AddCardtoDeck(Card card)
        {

            GameObject instance = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            cardManager = instance.GetComponent<CardManager>();


            cardManager.owner = team;
            cardsInDeck.Add(cardManager.gameObject);
            instance.transform.SetParent(this.transform);

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
                cardsInHand.Add(tempCard);


                //Queue up a move card animation.
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(manager, deckholder, hand));
                EventManager.Instance.QueueAnimation(new UpdateDeckTexts_GUI(cardsInDeck.Count, cardsInDiscard.Count, team));
                //Check for objects that trigger on draw card.
                EventManager.Instance.TriggerEvent(new TableCard_Trigger(manager.owner, CardEffect.Trigger.OnDraw));

            }
        }


    }


}