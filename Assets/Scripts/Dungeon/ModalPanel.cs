using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class ModalPanel : MonoBehaviour
    {
        public TMP_Text title;
        public TMP_Text subText;

        public GameObject choicesPanel;
        public GameObject goldObject;
        public GameObject cardObject;

        public Button addButton;
        public Button closeButton;

        public GameObject rewardSelection;
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

            ClearPanel();

            choicesPanel.SetActive(true);

            addButton.onClick.AddListener(yesEvent);
            addButton.onClick.AddListener(ClosePanel);

            closeButton.onClick.AddListener(noEvent);
            closeButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;

            var card = Instantiate(cardObject);
            selections.Add(card);
            card.transform.SetParent(choicesPanel.transform, false);
            var cardScript = card.GetComponent<CardManager>();
            cardScript.PopulateCard(reward);

            addButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(true);
        }


        //Chest with gold.
        public void Chest(string title, string subText, int goldReward, UnityAction yesEvent)
        {
            modalPanelObject.SetActive(true);

            ClearPanel();

            choicesPanel.SetActive(true);

            closeButton.onClick.AddListener(yesEvent);
            closeButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;

            var gold = Instantiate(goldObject);
            selections.Add(gold);
            gold.transform.SetParent(choicesPanel.transform, false);
            var goldscript = gold.GetComponent<Gold>();
            goldscript.PopulateGold(goldReward);

            closeButton.gameObject.SetActive(true);
        }

        //Panel shown after killing a monster. Choose between 3 cards, and gold.
        internal void MonsterLoot(List<Card> cardRewards, int goldReward)
        {
            modalPanelObject.SetActive(true);
            Selectable.selectContext = Selectable.SelectContext.ModalPanel;

            ClearPanel();

            choicesPanel.SetActive(true);

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

            addButton.onClick.AddListener(AddReward);
            addButton.onClick.AddListener(ClosePanel);

            addButton.GetComponent<Button>().interactable = false;
            addButton.gameObject.SetActive(true);
        }

        private void AddReward()
        {
            var selected = Selectable.selectedObject;
            Selectable.selectContext = Selectable.SelectContext.NoSelect;

            if (selected.tag == "Card")
            {
                DeckManager.player.AddCardtoDeck(selected.GetComponent<CardManager>());
                selections.Remove(selected);
                //selected.GetComponent<Selectable>().ClearOutline();
            }
            else if (selected.tag == "Gold")
            {
                GameManager.instance.ModifyGold(selected.GetComponent<Gold>().goldValue);
            }

        }



        //Panel to choose between 3 cards when leveling up.
        internal void LevelUp(List<Card> cardRewards, UnityAction complete)
        {
            Selectable.selectContext = Selectable.SelectContext.ModalPanel;
            modalPanelObject.SetActive(true);
            
            ClearPanel();

            choicesPanel.SetActive(true);

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
            addButton.onClick.AddListener(AddReward);
            addButton.onClick.AddListener(ClosePanel);

            addButton.GetComponent<Button>().interactable = false;
            addButton.gameObject.SetActive(true);
        }

        //Panel shown when opening a shrine
        internal void Shrine(string title, string subText, UnityAction prayer, UnityAction noEvent, bool needsCardSelection, UnityAction shrineSpent)
        {
            modalPanelObject.SetActive(true);


            ClearPanel();
            prayerPanel.SetActive(true);

            closeButton.onClick.AddListener(noEvent);
            closeButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;

            prayButton.onClick.AddListener(prayer);
            prayButton.onClick.AddListener(shrineSpent);
            prayButton.onClick.AddListener(PrayerSpent);
            prayButton.interactable = true;

            if (needsCardSelection)
            {
                prayButton.interactable = false;
                selectCardPanel.SetActive(true);

            }

            closeButton.gameObject.SetActive(true);
        }

        //Panel shown when opening an anvil.
        internal void Anvil(string title, string subText, UnityAction noEvent)
        {
            modalPanelObject.SetActive(true);

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

        //Is called by Deck Panel when it returns a card selected by the player.
        public void SelectCard(GameObject card)
        {
            cardSelectText.SetActive(false);
            selectCardButton.interactable = false;

            card.transform.SetParent(selectedCardHolder.transform);
            selectedCard = card;
            prayButton.interactable = true;


            if (GameManager.instance.player.gold >= 50)
            {
                anvilUpgradeButton.interactable = true;
            }
            else
            {
                anvilUpgradeButton.interactable = false;
            }

            if (GameManager.instance.player.gold >= 20)
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

            Card newCard = null;
            int levelBonus = 0;
            while (newCard == null)
            {
                levelBonus++;
                if (levelBonus > 10)
                {
                    newCard = manager.card;
                }
                newCard = DAL.ObjectDAL.GetRandomCard(level + levelBonus, level + levelBonus, type);
            }

            var card = DeckManager.player.AddCardtoDeck(DeckManager.player.CreateCardObject(newCard));
            card.transform.SetParent(selectedCardHolder.transform);
            selectedCard = card.gameObject;
            anvilUpgradeButton.interactable = false;
        }

        internal void SelectReward()
        {
            addButton.GetComponent<Button>().interactable = true;            
        }
        
        void ClosePanel()
        {
            foreach (GameObject obj in selections)
            {
                Destroy(obj);
            }
            if (selectedCard)
            {
               // selectedCard.GetComponent<CardManager>().imagePanel.ResetPanel();
                selectedCard.transform.SetParent(DeckManager.player.deckHolder.transform);
            }
            selections.Clear();
            rewardSelection = null;

            modalPanelObject.SetActive(false);
            GameManager.instance.doingSetup = false;
        }


        private void ClearPanel()
        {
            this.title.text = "";
            this.subText.text = "";

            anvilPanel.SetActive(false);
            prayerPanel.SetActive(false);
            choicesPanel.SetActive(false);
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