using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DeckPanel : MonoBehaviour
    {
        public bool isActive;
        public Button closeButton;
        public Button chooseButton;

        private static DeckPanel deckPanel;
        public GameObject deckPanelObject;
        public GameObject cardArea;

        public GameObject selectedCard;
        public string selectedTag;

        public int cardno;

        public static DeckPanel Instance()
        {
            if (!deckPanel)
            {
                deckPanel = FindObjectOfType(typeof(DeckPanel)) as DeckPanel;
                if (!deckPanel)
                    Debug.LogError("There needs to be one active DeckPanel script on a GameObject in your scene.");
            }
            return deckPanel;
        }

        internal void Select(GameObject selectedCard)
        {
            if (this.selectedCard)
            {

                if (selectedCard == this.selectedCard)
                {
                    selectedCard.GetComponent<CardManager>().imagePanel.ShowFullDescription(false);
                    selectedCard.GetComponent<Selectable>().outline.enabled = false;
                    this.selectedCard = null;
                    return;
                }

                this.selectedCard.GetComponent<Selectable>().outline.enabled = false;
                this.selectedCard.GetComponent<CardManager>().imagePanel.ShowFullDescription(false);
            }
            this.selectedCard = selectedCard;
            //   selectedCard.GetComponent<CardManager>().cardDescription.SetActive(true);
            selectedCard.GetComponent<Selectable>().outline.enabled = true;
            //   selectedCard.GetComponent<CardManager>().descriptionPanel.SetActive(true);
            selectedCard.GetComponent<CardManager>().imagePanel.ShowFullDescription(true);


        }

        internal void ShowDeckPanel()
        {
            deckPanelObject.SetActive(true);
            isActive = true;

            closeButton.gameObject.SetActive(true);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(closePanel);

            chooseButton.gameObject.SetActive(false);
        }

        internal void DuplicateCardPanel()
        {

            deckPanelObject.SetActive(true);
            isActive = true;

            closeButton.gameObject.SetActive(false);

            chooseButton.gameObject.SetActive(true);
            chooseButton.onClick.RemoveAllListeners();
            chooseButton.onClick.AddListener(Duplicate);
        }

        private void Duplicate()
        {
            DeckManager.player.AddCardtoDeck(selectedCard.GetComponent<CardManager>().card.cardName);

            closeButton.gameObject.SetActive(true);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(closePanel);

            chooseButton.gameObject.SetActive(false);
        }

        internal void DestroyRandomCardsPanel(int cardno)
        {
            this.cardno = cardno;

            deckPanelObject.SetActive(true);
            isActive = true;

            closeButton.gameObject.SetActive(false);

            chooseButton.gameObject.SetActive(true);
            chooseButton.onClick.RemoveAllListeners();
            chooseButton.onClick.AddListener(DestroyRandom);
        }

        private void DestroyRandom()
        {
            for (int i = 0; i < cardno; i++)
            {
                DeckManager.player.DestroyRandomCard();
            }
            closeButton.gameObject.SetActive(true);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(closePanel);

            chooseButton.gameObject.SetActive(false);
        }

        internal void DestroyCardPanel()
        {
            deckPanelObject.SetActive(true);
            isActive = true;

            closeButton.gameObject.SetActive(false);

            chooseButton.gameObject.SetActive(true);
            chooseButton.onClick.RemoveAllListeners();
            chooseButton.onClick.AddListener(Destroy);
        }

        private void Destroy()
        {
            DeckManager.player.DestroyCard(selectedCard);

            closeButton.gameObject.SetActive(true);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(closePanel);

            chooseButton.gameObject.SetActive(false);
        }


        internal void UpgradeCardPanel()
        {
            deckPanelObject.SetActive(true);
            isActive = true;

            closeButton.gameObject.SetActive(false);

            chooseButton.gameObject.SetActive(true);
            chooseButton.onClick.RemoveAllListeners();
            chooseButton.onClick.AddListener(Upgrade);
        }

        private void Upgrade()
        {
            var level = selectedCard.GetComponent<CardManager>().card.level;
            DeckManager.player.DestroyCard(selectedCard);

            var newCard = new Card();

            if (selectedCard.GetComponent<CardManager>().card.type == Card.Type.ClassCard)
            {
                newCard = DAL.ObjectDAL.GetRandomClassCard(level + 1, level + 1);
            }
            else
            {
                newCard = DAL.ObjectDAL.GetRandomCard(level + 1, level + 1);
            }
            DeckManager.player.AddCardtoDeck(newCard.cardName);

            closeButton.gameObject.SetActive(true);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(closePanel);

            chooseButton.gameObject.SetActive(false);
        }

        private void closePanel()
        {
            if (selectedCard)
            {
                selectedCard.GetComponent<Selectable>().outline.enabled = false;
            }
            isActive = false;
            deckPanelObject.SetActive(false);
        }
    }
}