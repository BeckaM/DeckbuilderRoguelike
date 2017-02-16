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
        public GameObject cardObject;
        public GameObject playerDeckHolder;
        public GameObject enemyDeckHolder;
        public GameObject playerDiscard;
        public GameObject enemyDiscard;
        
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


        public void Cleanup()
        {
            foreach(GameObject CardObject in GameObject.FindGameObjectsWithTag("Card"))
            { 
                CardManager card = CardObject.GetComponent<CardManager>();
            
                if (card.team == CardManager.Team.My)
                {
                    card.transform.SetParent(playerDeckHolder.transform);
                    card.SetCardStatus(CardManager.CardStatus.InDeck);

                }


                else
                {
                    Destroy(CardObject);
                }
            }
        }



        public void StartingDeck(List <string> cardstoCreate)
        {

              AddCardtoDeck(cardstoCreate);
            
        }

        public void AddCardtoDeck(List<string> cardsToCreate, string team = "My")
        {
            var cardobjects = ObjectDAL.GetCards(cardsToCreate);
            var deck = this.transform;
            

            foreach (Card card in cardobjects)
            {

                GameObject instance = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                cardManager = instance.GetComponent<CardManager>();

                if (team == "My")
                {
                    cardManager.team = CardManager.Team.My;
                    instance.transform.SetParent(playerDeckHolder.transform);
                }
                else
                {
                    cardManager.team = CardManager.Team.AI;
                    instance.transform.SetParent(enemyDeckHolder.transform);
                }




                cardManager.PopulateCard(card);
            }
        }

    }

}