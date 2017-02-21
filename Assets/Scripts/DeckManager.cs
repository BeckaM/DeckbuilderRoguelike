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
        
        CardManager cardManager;
        public string instancename;
        public GameObject cardObject;
        public CardManager.Team team;
        public List<GameObject> cardsInDeck = new List<GameObject>();
        public List<GameObject> cardsInDiscard = new List<GameObject>();
        public int cardsInDeckCount
        {
            get
            {
                var c = cardsInDeck.Count;
                return c;
            }
        }

        public int cardsInDiscardCount
        {
            get
            {
                var c = cardsInDiscard.Count;
                return c;
            }
        }

        //public GameObject playerDeckHolder;
        //public GameObject enemyDeckHolder;
        //public GameObject playerDiscard;
        //public GameObject enemyDiscard;

        // Use this for initialization
        void Awake()
        {
            name = this.gameObject.name;
            if (name == "MyDeck") {


                instancename = "Me";
                team = CardManager.Team.My;
                DontDestroyOnLoad(gameObject);
                
            }
            //If instance already exists and it's not this:
            else if (name == "AIDeck")
            {
                instancename = "AI";
                team = CardManager.Team.AI;
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



        public void StartingDeck(List <string> cardstoCreate)
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


                cardManager.team = team;
                cardsInDeck.Add(cardManager.gameObject);
                instance.transform.SetParent(this.transform);

                cardManager.PopulateCard(card);
            }
        }

        public void Draw()
        {
            if (cardsInDeck.Count != 0)
            {

                //Draw card logic.
                int random = Random.Range(0, cardsInDeck.Count);
                GameObject tempCard = cardsInDeck[random];

                CardManager manager = tempCard.GetComponent<CardManager>();
                manager.SetCardPosition(CardManager.CardStatus.InHand);

                cardsInDeck.Remove(tempCard);

                //Queue up a move card animation.
                EventManager.Instance.QueueSequentialEvent(new MoveCardEvent(tempCard.gameObject));
                EventManager.Instance.QueueSequentialEvent(new UpdateDeckTexts(cardsInDeckCount, cardsInDiscardCount, team));
                //Check for objects that trigger on draw card.
                EventManager.Instance.QueueFastEvent(new DrawCardEvent(tempCard.GetComponent<CardManager>().team));
               
            }
        }

       
    }


}