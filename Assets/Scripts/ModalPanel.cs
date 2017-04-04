using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;

namespace Assets.Scripts
{
    public class ModalPanel : MonoBehaviour
    {
        public TMP_Text title;
        public TMP_Text subText;

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

        public GameObject anvilPanel;
        public GameObject anvilCard;
        public GameObject anvilCardHolder;
        public GameObject anvilSelectText;

        public Button anvilDestroy;
        public Button anvilUpgrade;

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

        internal void LevelUp(List<Card> cardRewards, UnityAction complete)
        {
            modalPanelObject.SetActive(true);
            isActive = true;

            this.title.text = "Level Up!";
            this.subText.text = "Choose a reward.";

            foreach (Card card in cardRewards)
            {
                var tempCard = Instantiate(cardObject);
                selections.Add(tempCard);
                tempCard.transform.SetParent(choicesPanel.transform);
                var cardScript = tempCard.GetComponent<CardManager>();
                cardScript.PopulateCard(card);
            }

            addButton.onClick.RemoveAllListeners();
            addButton.onClick.AddListener(complete);
            addButton.onClick.AddListener(ClosePanel);

            addButton.GetComponent<Button>().interactable = false;
            addButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(false);
            thanksButton.gameObject.SetActive(false);
        }

        internal void Shrine(string title, string subText, List<UnityAction> prayers, UnityAction noEvent)
        {
            modalPanelObject.SetActive(true);
            isActive = true;

            //addButton.onClick.RemoveAllListeners();
            //addButton.onClick.AddListener(ClosePanel);

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


        internal void Anvil(string title, string subText, UnityAction noEvent)
        {
            modalPanelObject.SetActive(true);
            isActive = true;
            anvilPanel.SetActive(true);

            this.title.text = title;
            this.subText.text = subText;

            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(noEvent);
            noButton.onClick.AddListener(ClosePanel);

            noButton.gameObject.SetActive(true);
            thanksButton.gameObject.SetActive(false);
            addButton.gameObject.SetActive(false);

            anvilUpgrade.interactable = false;
            anvilDestroy.interactable = false;
        }


        public void SelectAnvilCard(GameObject card)
        {
            isActive = true;
            anvilSelectText.SetActive(false);

            card.transform.SetParent(anvilCardHolder.transform);
            anvilCard = card;

            //GameObject instance = Instantiate(cardObject) as GameObject;
            //var cardManager = instance.GetComponent<CardManager>();
            //cardManager.card = card;

            //instance.transform.SetParent(anvilCardHolder.transform);
            //instance.transform.localScale = new Vector3(1f, 1f, 1f);

            //cardManager.PopulateCard(cardManager.card);

            if (GameManager.instance.gold >= 10)
            {
                anvilUpgrade.interactable = true;
            }
            else
            {
                anvilUpgrade.interactable = false;
            }

            if (GameManager.instance.gold >= 3)
            {
                anvilDestroy.interactable = true;
            }
            else
            {
                anvilDestroy.interactable = false;
            }
        }


        public void AnvilDestroy()
        {           
            DeckManager.player.DestroyCard(anvilCard);
        }

        public void AnvilUpgrade()
        {
            var level = anvilCard.GetComponent<CardManager>().card.level;
            var type = anvilCard.GetComponent<CardManager>().card.type;
            DeckManager.player.DestroyCard(anvilCard);

            var newCard = new Card();

            if (type == Card.Type.ClassCard)
            {
                newCard = DAL.ObjectDAL.GetRandomClassCard(level + 1, level + 1);
            }
            else
            {
                newCard = DAL.ObjectDAL.GetRandomCard(level + 1, level + 1);
            }
            var card =  DeckManager.player.AddCardtoDeck(newCard);
            card.transform.SetParent(anvilCardHolder.transform);
            anvilCard = card;
            anvilUpgrade.interactable = false;

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
            else if (currentSelection.tag == "Card")
            {
                GameManager.instance.cardLoot = currentSelection.GetComponent<CardManager>().card;
                GameManager.instance.lootType = GameManager.Content.Card;
            }
        }


        void ClosePanel()
        {
            foreach (GameObject obj in selections)
            {
                Destroy(obj);
            }
            if (anvilCard)
            {
                anvilCard.transform.SetParent(DeckManager.player.deckHolder.transform);
            }
            selections.Clear();
            currentSelection = null;
            isActive = false;
            modalPanelObject.SetActive(false);
        }

    }
}