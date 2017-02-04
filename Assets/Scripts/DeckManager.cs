using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using System.IO;
using Assets.Scripts.DAL;

namespace Assets.Scripts
{

    public class DeckManager : MonoBehaviour
    {
        public static DeckManager instance = null;              //Static instance of DeckManager which allows it to be accessed by any other script.
        CardManager cardManager;
        public GameObject CardObject;

        // Use this for initialization
        void Awake()
        {
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a DeckManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
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

        public void AddCardtoDeck(List<string> cardsToCreate, string team = "My")
        {
            var cardobjects = ObjectDAL.GetCards(cardsToCreate);
            var deck = GameObject.Find("Deck").transform;
            var enemydeck = GameObject.Find("EnemyDeck").transform;

            foreach (Card card in cardobjects)
            {

                GameObject instance = Instantiate(CardObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                cardManager = instance.GetComponent<CardManager>();

                if (team == "AI")
                {
                    cardManager.team = CardManager.Team.AI;
                    instance.transform.SetParent(enemydeck);
                }
                else
                {
                    cardManager.team = CardManager.Team.My;
                    instance.transform.SetParent(deck);
                }




                cardManager.PopulateCard(card);
            }
        }

    }

}