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
            prayers = new List<Prayer>();

            for (var i = 0; i < 3; i++)
            {
                var prayer = allPrayers[UnityEngine.Random.Range(0, allPrayers.Count)];
                prayers.Add(prayer);
                allPrayers.Remove(prayer);                
            }

        }

        private List<Prayer> GetPrayers(int level)
        {
            List<Prayer> prayerList = new List<Prayer>();

            var prayer1 = new Prayer("Prayer of Duplication", "Choose a card in your deck and duplicate it.", 0, Duplication);
            prayerList.Add(prayer1);

            var prayer2 = new Prayer("Prayer of Chaos", "Destroy three random cards in your deck.", 1, Duplication);
            prayerList.Add(prayer2);

            var prayer3 = new Prayer("Prayer of Destruction", "Choose a card in your deck and destroy it.", 2, Duplication);
            prayerList.Add(prayer3);

            var prayer4 = new Prayer("Prayer of Evolution", "Choose a card in your deck and replace it with a random one of higher level.", 3, Duplication);
            prayerList.Add(prayer4);

            return prayerList;
        }

        internal void OpenShrine()
        {
            GameManager.instance.modalPanel.Shrine("You found a Shrine!", "Choose a prayer", prayers, Decline);
        }

        private void Decline()
        {
            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }

        private void Duplication()
        {
            this.gameObject.SetActive(false);
            Debug.Log("Duplication Prayer triggered");
            GameManager.instance.doingSetup = false;
        }
    }

    public class Prayer
    {
        public string name;
        public string description;
        public int sprite;
        public UnityAction prayerEvent;

        public Prayer(string name, string description, int sprite, UnityAction prayerEvent)
        {
            this.name = name;
            this.description = description;
            this.sprite = sprite;
            this.prayerEvent = prayerEvent;
        }

    }
}
