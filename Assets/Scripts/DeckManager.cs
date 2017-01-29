using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using System.IO;



    public class DeckManager : MonoBehaviour
    {
        public static DeckManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
        CardManager cardManager;
        public GameObject CardObject;
        private const string fileName = Constants.CardPath;

    // Use this for initialization
    void Awake()
        {
            instance = this;
        }


        public void StartingDeck()
        {

        List<string> cardstoCreate = new List<string>()
        {
            "Phantom Strike",
            "Phantom Strike",
            "Phantom Strike",
            "Phantom Strike",
            "Phantom Strike"
        };


        AddCardtoDeck(cardstoCreate);
       

        }


        public List<Card> JSONreader(List<string> cardstoget)
        {

            string text = File.ReadAllText(fileName);
            var cardList = JsonUtility.FromJson<CardWrapper>(text);
            var Cardreturn = new List<Card>();



            foreach (string cardtoget in cardstoget)
             { 

                var card = cardList.CardItems.Find(item => item.CardName.Equals(cardtoget));
                Cardreturn.Add(card);

            }

            return Cardreturn;

        }



        public void AddCardtoDeck(List<string> cardsToCreate, string team="My")
        {

            var cardobjects = JSONreader(cardsToCreate);
            var deck = GameObject.Find("Deck").transform;
            var enemydeck = GameObject.Find("EnemyDeck").transform;

        foreach (Card card in cardobjects)
            {

            GameObject instance = Instantiate(CardObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            if (team == "AI")
            {
                card.team = Card.Team.AI;
                instance.transform.SetParent(enemydeck);
            }
            else
            {
                card.team = Card.Team.My;
                instance.transform.SetParent(deck);
            }
                
              //  GameObject instance = Instantiate(CardObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

               cardManager = instance.GetComponent<CardManager>();

               cardManager.GetCard(card);
                
           
              

            }
        }

    }

