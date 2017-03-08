using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class ModalPanel : MonoBehaviour
    {
        public Text title;
        public Text subText;

        public bool isActive;
        public GameObject choicesPanel;
        public GameObject goldObject;
        public GameObject cardObject;
        public GameObject prayerObject;
        public Button addButton;
        public Button noButton;
        public Button thanksButton;
        public GameObject currentSelection;
        public List<GameObject> selections;

        public GameObject modalPanelObject;        

        private static ModalPanel modalPanel;

        public static ModalPanel Instance()
        {
            if (!modalPanel)
            {
                modalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
                if (!modalPanel)
                    Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
            }

            return modalPanel;
        }

        //  Chest with card or consumable.
        public void Chest(string title, string subText, Card reward, UnityAction yesEvent, UnityAction noEvent)
        {
            modalPanelObject.SetActive(true);
            isActive = true;

            addButton.onClick.RemoveAllListeners();
            addButton.onClick.AddListener(yesEvent);
            addButton.onClick.AddListener(ClosePanel);

            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(noEvent);
            noButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;

            var card = Instantiate(cardObject);
            selections.Add(card);
            card.transform.SetParent(choicesPanel.transform);
            var cardScript = card.GetComponent<CardManager>();
            cardScript.PopulateCard(reward);

            addButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);
            thanksButton.gameObject.SetActive(false);

        }

        //Chest with gold.
        public void Chest(string title, string subText, int goldReward, UnityAction yesEvent)
        {
            modalPanelObject.SetActive(true);
            isActive = true;

            thanksButton.onClick.RemoveAllListeners();
            thanksButton.onClick.AddListener(yesEvent);
            thanksButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;

            var gold = Instantiate(goldObject);
            selections.Add(gold);
            gold.transform.SetParent(choicesPanel.transform);
            var goldscript = gold.GetComponent<Gold>();
            goldscript.PopulateGold(goldReward);


            addButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            thanksButton.gameObject.SetActive(true);
        }

        internal void Shrine(string title, string subText, List<UnityAction> prayers, UnityAction noEvent)
        {
            modalPanelObject.SetActive(true);
            isActive = true;

            addButton.onClick.RemoveAllListeners();
            addButton.onClick.AddListener(ClosePanel);

            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(noEvent);
            noButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;

            foreach (UnityAction prayer in prayers)
            {
                var p = Instantiate(prayerObject);
                selections.Add(p);
                p.transform.SetParent(choicesPanel.transform);
                var pscript = p.GetComponent<Prayer>();
                pscript.PopulatePrayer(prayer);
            }


            addButton.gameObject.SetActive(true);
            addButton.GetComponent<Button>().interactable = false;

            noButton.gameObject.SetActive(true);
            thanksButton.gameObject.SetActive(false);
        }
                               
        internal void Select(GameObject selection)
        {
            if (currentSelection)
            {
                currentSelection.GetComponent<Selectable>().outline.enabled = false;
            }
            currentSelection = selection;
            selection.GetComponent<Selectable>().outline.enabled = true;
            addButton.GetComponent<Button>().interactable = true;

            if (currentSelection.tag == "Prayer")
            {
                var prayer = currentSelection.GetComponent<Prayer>();
                addButton.onClick.RemoveAllListeners();
                addButton.onClick.AddListener(prayer.prayerEvent);
                addButton.onClick.AddListener(ClosePanel);
            }            
        }
        
        void ClosePanel()
        {
            foreach(GameObject obj in selections)
            {
                Destroy(obj);
            }
            isActive = false;
            modalPanelObject.SetActive(false);
        }
               
    }
}