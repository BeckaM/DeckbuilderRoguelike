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
        public List<UnityAction> prayers;

        internal void PopulateShrine(int level)
        {
            List<UnityAction> allPrayers = GetPrayers(level);
            prayers = new List<UnityAction>();

            for (var i = 0; i < 3; i++)
            {
                var prayer = allPrayers[UnityEngine.Random.Range(0, allPrayers.Count)];
                prayers.Add(prayer);
                allPrayers.Remove(prayer);
            }

        }

        private List<UnityAction> GetPrayers(int level)
        {
            List<UnityAction> prayerList = new List<UnityAction>();

            var prayer1 = new UnityAction(Duplication);
            prayerList.Add(prayer1);

            var prayer2 = new UnityAction(RandomDestroyThree);
            prayerList.Add(prayer2);

            var prayer3 = new UnityAction(Destruction);
            prayerList.Add(prayer3);

            var prayer4 = new UnityAction(Upgrade);
            prayerList.Add(prayer4);

            return prayerList;
        }


        internal void OpenShrine()
        {
            GameManager.instance.modalPanel.Shrine("You found a Shrine!", "Choose a prayer", prayers, Decline);
            GameManager.instance.progressManager.FoundShrine();
        }

        private void Decline()
        {
            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }

        private void Duplication()
        {
            Debug.Log("Prayer of Duplication triggered");
            GameManager.instance.deckPanel.DuplicateCardPanel();

            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }

        private void RandomDestroyThree()
        {
            Debug.Log("Prayer of Chaos triggered");
            GameManager.instance.deckPanel.DestroyRandomCardsPanel(3);

            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }

        private void Destruction()
        {
            Debug.Log("Prayer of Destruction triggered");
            GameManager.instance.deckPanel.DestroyCardPanel();

            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }

        private void Upgrade()
        {
            Debug.Log("Prayer of Evolution triggered");
            GameManager.instance.deckPanel.UpgradeCardPanel();

            this.gameObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }
    }
}


