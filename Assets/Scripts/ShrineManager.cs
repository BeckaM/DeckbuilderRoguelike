using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{

    public class ShrineManager : MonoBehaviour
    {
        public List<Sprite> sprites;
        public UnityAction prayer;
        public SpriteRenderer shrineSprite;
        public string shrineType;
        public string shrineDescription;
        public bool needsCardSelection;

        public List<string> shrineTypes = new List<string> {"Shrine of Duplication", "Shrine of Chaos", "Shrine of Destruction", "Shrine of Evolution" };

        internal void PopulateShrine(int level)
        {                        
            shrineType = shrineTypes[UnityEngine.Random.Range(0, shrineTypes.Count)];

            if (shrineType == "Shrine of Duplication")
            {                
                shrineDescription = "Choose a card in your deck and duplicate it.";
                needsCardSelection = true;
                prayer = new UnityAction(GameManager.instance.modalPanel.DuplicateSelectedCard);

                shrineSprite.color = Color.blue;
            }

            else if (shrineType == "Shrine of Chaos")
            {                
                shrineDescription = "Destroy three random cards in your deck.";
                needsCardSelection = false;
                prayer = new UnityAction(GameManager.instance.deckPanel.DestroyRandomCardsPanel);

                shrineSprite.color = Color.grey;
            }

            else if (shrineType == "Shrine of Destruction")
            {
                
                shrineDescription = "Choose a card in your deck and destroy it.";
                needsCardSelection = true;
                prayer = new UnityAction(GameManager.instance.modalPanel.DestroySelectedCard);

                shrineSprite.color = Color.red;
            }

            else if (shrineType == "Shrine of Evolution")
            {                
                shrineDescription = "Choose a card in your deck and replace it with a random one of higher level.";
                needsCardSelection = true;
                prayer = new UnityAction(GameManager.instance.modalPanel.UpgradeSelectedCard);

                shrineSprite.color = Color.red;
            }
        }
        

        internal void OpenShrine()
        {
            GameManager.instance.modalPanel.Shrine(shrineType, shrineDescription, prayer, Decline, needsCardSelection);
            GameManager.instance.progressManager.CumulativeMetric(ProgressManager.Metric.Shrines_Opened, 1);
        }

        private void Decline()
        {
            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }

        //private void Duplication()
        //{
        //    Debug.Log("Prayer of Duplication triggered");
        //    GameManager.instance.deckPanel.DuplicateCardPanel();

        //    this.gameObject.SetActive(false);
        //    GameManager.instance.doingSetup = false;
        //}

        //private void RandomDestroyThree()
        //{
        //    Debug.Log("Prayer of Chaos triggered");
        //    GameManager.instance.deckPanel.DestroyRandomCardsPanel(3);

        //    this.gameObject.SetActive(false);
        //    GameManager.instance.doingSetup = false;
        //}

        //private void Destruction()
        //{
        //    Debug.Log("Prayer of Destruction triggered");
        //    GameManager.instance.deckPanel.DestroyCardPanel();

        //    this.gameObject.SetActive(false);
        //    GameManager.instance.doingSetup = false;
        //}

        //private void Upgrade()
        //{
        //    Debug.Log("Prayer of Evolution triggered");
        //    GameManager.instance.deckPanel.UpgradeCardPanel();

        //    this.gameObject.SetActive(false);
        //    GameManager.instance.doingSetup = false;
        //}
    }
}


