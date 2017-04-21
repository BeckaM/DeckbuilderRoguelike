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
        //public GameObject prayerObject;

        public Button addButton;
        public Button closeButton;

        public GameObject currentSelection;
        public List<GameObject> selections;

        public GameObject modalPanelObject;

        private static ModalPanel modalPanel;

        public GameObject prayerPanel;
        public Button prayButton;

        public GameObject selectCardPanel;
        public GameObject cardSelectText;
        public GameObject selectedCard;
        public GameObject selectedCardHolder;
        public Button selectCardButton;

        public GameObject selectFromThreePanel;

        public GameObject anvilPanel;
        public Button anvilDestroyButton;
        public Button anvilUpgradeButton;

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

            ClearPanel();

            selectFromThreePanel.SetActive(true);

            addButton.onClick.AddListener(yesEvent);
            addButton.onClick.AddListener(ClosePanel);

            closeButton.onClick.AddListener(noEvent);
            closeButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;

            var card = Instantiate(cardObject);
            selections.Add(card);
            card.transform.SetParent(choicesPanel.transform);
            var cardScript = card.GetComponent<CardManager>();
            cardScript.PopulateCard(reward);

            addButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(true);
        }


        //Chest with gold.
        public void Chest(string title, string subText, int goldReward, UnityAction yesEvent)
        {
            modalPanelObject.SetActive(true);
            isActive = true;

            ClearPanel();

            selectFromThreePanel.SetActive(true);

            closeButton.onClick.AddListener(yesEvent);
            closeButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;

            var gold = Instantiate(goldObject);
            selections.Add(gold);
            gold.transform.SetParent(choicesPanel.transform);
            var goldscript = gold.GetComponent<Gold>();
            goldscript.PopulateGold(goldReward);

            closeButton.gameObject.SetActive(true);
        }

        internal void MonsterLoot(List<Card> cardRewards, int goldReward, UnityAction complete)
        {
            modalPanelObject.SetActive(true);
            isActive = true;

            ClearPanel();

            selectFromThreePanel.SetActive(true);

            this.title.text = "You are victorious";
            this.subText.text = "Choose a reward from your dead foe.";

            foreach (Card card in cardRewards)
            {
                var tempCard = Instantiate(cardObject);
                selections.Add(tempCard);
                tempCard.transform.SetParent(choicesPanel.transform, false);
                var cardScript = tempCard.GetComponent<CardManager>();
                cardScript.PopulateCard(card);
            }

            var gold = Instantiate(goldObject);
            selections.Add(gold);
            gold.transform.SetParent(choicesPanel.transform, false);
            var goldscript = gold.GetComponent<Gold>();
            goldscript.PopulateGold(goldReward);

            addButton.onClick.AddListener(complete);
            addButton.onClick.AddListener(ClosePanel);

            addButton.GetComponent<Button>().interactable = false;
            addButton.gameObject.SetActive(true);
        }


        internal void LevelUp(List<Card> cardRewards, UnityAction complete)
        {
            modalPanelObject.SetActive(true);
            isActive = true;

            ClearPanel();

            selectFromThreePanel.SetActive(true);

            this.title.text = "Level Up!";
            this.subText.text = "Choose a reward.";

            foreach (Card card in cardRewards)
            {
                var tempCard = Instantiate(cardObject);
                selections.Add(tempCard);
                tempCard.transform.SetParent(choicesPanel.transform, false);
                var cardScript = tempCard.GetComponent<CardManager>();
                cardScript.PopulateCard(card);
            }

            addButton.onClick.AddListener(complete);
            addButton.onClick.AddListener(ClosePanel);

            addButton.GetComponent<Button>().interactable = false;
            addButton.gameObject.SetActive(true);
        }


        internal void Shrine(string title, string subText, UnityAction prayer, UnityAction noEvent, bool needsCardSelection, UnityAction shrineSpent)
        {
            modalPanelObject.SetActive(true);
            isActive = true;

            ClearPanel();
            prayerPanel.SetActive(true);

            closeButton.onClick.AddListener(noEvent);
            closeButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;

            prayButton.onClick.AddListener(prayer);
            prayButton.onClick.AddListener(shrineSpent);            
            prayButton.interactable = true;

            if (needsCardSelection)
            {
                prayButton.interactable = false;
                selectCardPanel.SetActive(true);

            }

            closeButton.gameObject.SetActive(true);
        }


        internal void Anvil(string title, string subText, UnityAction noEvent)
        {
            modalPanelObject.SetActive(true);
            isActive = true;

            ClearPanel();

            anvilPanel.SetActive(true);
            selectCardPanel.SetActive(true);

            this.title.text = title;
            this.subText.text = subText;

            closeButton.onClick.AddListener(noEvent);
            closeButton.onClick.AddListener(ClosePanel);

            closeButton.gameObject.SetActive(true);

            anvilUpgradeButton.interactable = false;
            anvilDestroyButton.interactable = false;
        }


        public void SelectCard(GameObject card)
        {
            isActive = true;
            cardSelectText.SetActive(false);

            card.transform.SetParent(selectedCardHolder.transform);
            selectedCard = card;
            prayButton.interactable = true;
            

            if (GameManager.instance.gold >= 50)
            {
                anvilUpgradeButton.interactable = true;
            }
            else
            {
                anvilUpgradeButton.interactable = false;
            }

            if (GameManager.instance.gold >= 20)
            {
                anvilDestroyButton.interactable = true;
            }
            else
            {
                anvilDestroyButton.interactable = false;
            }
        }

        internal void PrayerSpent()
        {
            prayButton.interactable = false;
            selectCardButton.interactable = false;
        }

        public void DestroySelectedCard()
        {
            var card = selectedCard.GetComponent<CardManager>();
            DeckManager.player.DestroyCard(card);
        }

        public void DuplicateSelectedCard()
        {
            DeckManager.player.AddCardtoDeck(DeckManager.player.CreateCardObject(selectedCard.GetComponent<CardManager>().card));
        }


        public void UpgradeSelectedCard()
        {
            var manager = selectedCard.GetComponent<CardManager>();
            var level = manager.card.level;
            var type = manager.card.type;
            DeckManager.player.DestroyCard(manager);

            var newCard = new Card();

            if (type == Card.Type.ClassCard)
            {
                newCard = DAL.ObjectDAL.GetRandomClassCard(level + 1, level + 1);
            }
            else
            {
                newCard = DAL.ObjectDAL.GetRandomCard(level + 1, level + 1);
            }
            var card = DeckManager.player.AddCardtoDeck(DeckManager.player.CreateCardObject(newCard));
            card.transform.SetParent(selectedCardHolder.transform);
            selectedCard = card.gameObject;
            anvilUpgradeButton.interactable = false;
        }

        internal void Select(GameObject selection)
        {
            addButton.GetComponent<Button>().interactable = true;
            if (this.currentSelection)
            {

                if (selection == currentSelection)
                {
                    if (selection.tag == "Card")
                    {
                        selection.GetComponent<CardManager>().imagePanel.ShowFullDescription(false);
                    }
                    selection.GetComponent<Selectable>().outline.enabled = false;
                    currentSelection = null;
                    return;
                }

                currentSelection.GetComponent<Selectable>().outline.enabled = false;
                if (currentSelection.tag == "Card")
                {
                    currentSelection.GetComponent<CardManager>().imagePanel.ShowFullDescription(false);
                }
            }
            currentSelection = selection;
            selection.GetComponent<Selectable>().outline.enabled = true;
            if (selection.tag == "Card")
            {
                selection.GetComponent<CardManager>().imagePanel.ShowFullDescription(true);
                GameManager.instance.cardLoot = currentSelection.GetComponent<CardManager>().card;
                GameManager.instance.lootType = GameManager.Content.Card;
            }
            else if (selection.tag == "Gold")
            {
                GameManager.instance.goldLoot = currentSelection.GetComponent<Gold>().goldValue;
                GameManager.instance.lootType = GameManager.Content.Gold;
            }
        }

        void ClosePanel()
        {
            foreach (GameObject obj in selections)
            {
                Destroy(obj);
            }
            if (selectedCard)
            {
                selectedCard.GetComponent<Selectable>().ClearOutline();
                selectedCard.GetComponent<CardManager>().imagePanel.ResetPanel();
                selectedCard.transform.SetParent(DeckManager.player.deckHolder.transform);
            }
            selections.Clear();
            currentSelection = null;
            isActive = false;
            modalPanelObject.SetActive(false);
        }


        private void ClearPanel()
        {
            this.title.text = "";
            this.subText.text = "";

            anvilPanel.SetActive(false);
            prayerPanel.SetActive(false);
            selectFromThreePanel.SetActive(false);
            selectCardPanel.SetActive(false);
            cardSelectText.SetActive(true);
            selectCardButton.interactable = true;


            closeButton.gameObject.SetActive(false);
            closeButton.onClick.RemoveAllListeners();

            addButton.gameObject.SetActive(false);
            addButton.onClick.RemoveAllListeners();

            prayButton.onClick.RemoveAllListeners();
        }

        public void PayForAnvil(string type)
        {
            if (type == "Destroy")
            {
                GameManager.instance.ModifyGold(-20);
            }
            if (type == "Upgrade")
            {
                GameManager.instance.ModifyGold(-50);
            }
        }
    }
}