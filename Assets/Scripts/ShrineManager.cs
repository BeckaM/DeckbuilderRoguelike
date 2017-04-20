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
        public bool isSpent=false; 

        public List<string> shrineTypes = new List<string> {"Shrine of Duplication", "Shrine of Chaos", "Shrine of Destruction", "Shrine of Evolution" };

        internal void PopulateShrine(int level)
        {                        
            shrineType = shrineTypes[UnityEngine.Random.Range(0, shrineTypes.Count)];

            if (shrineType == "Shrine of Duplication")
            {                
                shrineDescription = "Choose a card in your deck and duplicate it.";
                needsCardSelection = true;
                prayer = new UnityAction(GameManager.instance.dungeonUI.modalPanel.DuplicateSelectedCard);

                shrineSprite.color = Color.blue;
            }

            else if (shrineType == "Shrine of Chaos")
            {                
                shrineDescription = "Destroy three random cards in your deck.";
                needsCardSelection = false;
                prayer = new UnityAction(GameManager.instance.dungeonUI.deckPanel.DestroyRandomCardsPanel);

                shrineSprite.color = Color.cyan;
            }

            else if (shrineType == "Shrine of Destruction")
            {
                
                shrineDescription = "Choose a card in your deck and destroy it.";
                needsCardSelection = true;
                prayer = new UnityAction(GameManager.instance.dungeonUI.modalPanel.DestroySelectedCard);

                shrineSprite.color = Color.red;
            }

            else if (shrineType == "Shrine of Evolution")
            {                
                shrineDescription = "Choose a card in your deck and replace it with a random one of higher level.";
                needsCardSelection = true;
                prayer = new UnityAction(GameManager.instance.dungeonUI.modalPanel.UpgradeSelectedCard);

                shrineSprite.color = Color.red;
            }
        }
        

        internal void OpenShrine()
        {
            GameManager.instance.dungeonUI.modalPanel.Shrine(shrineType, shrineDescription, prayer, Decline, needsCardSelection, ShrineSpent);
            GameManager.instance.progressManager.CumulativeMetric(ProgressManager.Metric.Shrines_Opened, 1);
        }

        public void ShrineSpent()
        {
            isSpent = true;

        }

        private void Decline()
        {
            if (isSpent)
            {
                shrineSprite.color = Color.grey;
                GetComponent<Collider>().enabled = false;
            }
            
            GameManager.instance.doingSetup = false;
        }              
    }
}


