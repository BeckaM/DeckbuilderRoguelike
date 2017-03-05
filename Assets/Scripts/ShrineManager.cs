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
        public List<Prayer> prayers;

        internal void PopulateShrine(int level)
        {
            List<Prayer> allPrayers = GetPrayers(level);
          
            for (var i = 0; i < 3; i++)
            {
                var prayer = allPrayers[UnityEngine.Random.Range(0, allPrayers.Count)];
                allPrayers.Remove(prayer);
                prayers.Add(prayer);

            }

        }

        private List<Prayer> GetPrayers(int level)
        {
            List<Prayer> prayerList = new List<Prayer>();

            var prayer1 = new Prayer("Prayer of Duplication", "Choose a card in your deck and duplicate it.", 0);
            prayerList.Add(prayer1);

            var prayer2 = new Prayer("Prayer of Chaos", "Destroy three random cards in your deck.", 1);
            prayerList.Add(prayer2);

            var prayer3 = new Prayer("Prayer of Destruction", "Choose a card in your deck and destroy it.", 2);
            prayerList.Add(prayer3);

            var prayer4 = new Prayer("Prayer of Evolution", "Choose a card in your deck and replace it with a random one of higher level.", 3);
            prayerList.Add(prayer4);
            
            return prayerList;
        }

        internal void OpenShrine()
        {
            GameManager.instance.modalPanel.Shrine("You found a Shrine!", "Choose a prayer", prayers, ChoosePrayer(), Decline());


        }

        private void Decline()
        {
            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }

        private void ChoosePrayer()
        {
            //if (content == Content.Gold)
            //{
            //    GameManager.instance.gold = GameManager.instance.gold + goldReward;
            //}
            //else
            //{
            //    DeckManager.player.AddCardtoDeck(cardReward);

            //}
            //this.gameObject.SetActive(false);
            //GameManager.instance.doingSetup = false;
        }
    }

    public class Prayer
    {
        public string name;
        public string description;
        public int sprite;

        public Prayer(string name, string description, int sprite)
        {
            name = this.name;
            description = this.description;
            sprite = this.sprite;
        }

    }
