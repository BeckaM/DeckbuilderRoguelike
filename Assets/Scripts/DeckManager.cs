using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using System.IO;

namespace Completed
{
    public class DeckManager : MonoBehaviour
    {

        CardManager cardManager;
        public GameObject CardObject;
        private const string fileName = @"C:\Users\Public\Documents\Unity Projects\DeckbuilderRoguelike\Assets\JSON\Cards.json";

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }




        public void StartingDeck()
        {

            string[] cardstoCreate = new string[5];
            cardstoCreate[0] = "Test";
            cardstoCreate[1] = "Murloc";
            cardstoCreate[2] = "Test";
            cardstoCreate[3] = "Rebecka";
            cardstoCreate[4] = "Rebecka";


            AddCardtoDeck(cardstoCreate);

        }


        private List<Card> JSONreader(string[] cardstoget)
        {

            string text = File.ReadAllText(fileName);
            var cardList = JsonUtility.FromJson<Wrapper>(text);
            var Cardreturn = new List<Card>();



            foreach (string cardtoget in cardstoget)
             { 

                var card = cardList.CardItems.Find(item => item.CardName.Equals(cardtoget));
                Cardreturn.Add(card);

            }

            return Cardreturn;

        }



        public void AddCardtoDeck(string[] cardsToCreate)
        {

            var cardobjects = JSONreader(cardsToCreate);
            var deck = GameObject.Find("Deck").transform;

            foreach (Card card in cardobjects)
            {
                GameObject instance = Instantiate(CardObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

                cardManager = instance.GetComponent<CardManager>();

               cardManager.GetCard(card);

                instance.transform.SetParent(deck);
            }
        }

    }
}
