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
        public Button selectButton;
        public Button applyButton;

        private static DeckPanel deckPanel;
        public GameObject deckPanelObject;
        public GameObject cardArea;

        public GameObject selectedCard;
        public string selectedTag;

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
            selectButton.interactable = true;
            if (this.selectedCard)
            {

                if (selectedCard == this.selectedCard)
                {
                  //  selectedCard.GetComponent<CardManager>().imagePanel.ShowFullDescription(false);
                    selectedCard.GetComponent<Selectable>().outline.enabled = false;
                    this.selectedCard = null;
                    selectButton.interactable = false;
                    return;
                }

                this.selectedCard.GetComponent<Selectable>().outline.enabled = false;
             //   this.selectedCard.GetComponent<CardManager>().imagePanel.ShowFullDescription(false);
            }
            this.selectedCard = selectedCard;            
            selectedCard.GetComponent<Selectable>().outline.enabled = true;            
          //  selectedCard.GetComponent<CardManager>().imagePanel.ShowFullDescription(true);
        }


        public void CardSelect()
        {
            deckPanelObject.SetActive(true);
            isActive = true;
            GameManager.instance.dungeonUI.modalPanel.isActive = false;
                        
            selectButton.gameObject.SetActive(true);
            selectButton.interactable = false;           
            selectButton.onClick.AddListener(SelectCard);
            selectButton.onClick.AddListener(ClosePanel);
        }


        private void SelectCard()
        {
            GameManager.instance.dungeonUI.modalPanel.SelectCard(selectedCard);
        }


        internal void ShowDeckPanel()
        {
            //GameManager.instance.dungeonManager.gameObject.SetActive(false);
            deckPanelObject.SetActive(true);
            isActive = true;

            closeButton.gameObject.SetActive(true);            
            closeButton.onClick.AddListener(ClosePanel);            
        }


        //internal void DuplicateCardPanel()
        //{
        //    deckPanelObject.SetActive(true);
        //    isActive = true;            

        //    selectButton.gameObject.SetActive(true);            
        //    selectButton.onClick.AddListener(Duplicate);
        //}


        //private void Duplicate()
        //{
        //    DeckManager.player.AddCardtoDeck(selectedCard.GetComponent<CardManager>().card.cardName);

        //    closeButton.gameObject.SetActive(true);
        //    closeButton.onClick.RemoveAllListeners();
        //    closeButton.onClick.AddListener(closePanel);

        //    selectButton.gameObject.SetActive(false);
        //}


        internal void DestroyRandomCardsPanel()
        {
            deckPanelObject.SetActive(true);
            isActive = true;

            closeButton.gameObject.SetActive(false);

            applyButton.gameObject.SetActive(true);
            applyButton.onClick.RemoveAllListeners();
            applyButton.onClick.AddListener(DestroyRandom);
        }


        private void DestroyRandom()
        {
            for (int i = 0; i < 3; i++)
            {
                DeckManager.player.DestroyRandomCard();
            }
            closeButton.gameObject.SetActive(true);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(ClosePanel);

            selectButton.gameObject.SetActive(false);
            applyButton.gameObject.SetActive(false);
        }


        //internal void DestroyCardPanel()
        //{
        //    deckPanelObject.SetActive(true);
        //    isActive = true;

        //    closeButton.gameObject.SetActive(false);

        //    selectButton.gameObject.SetActive(true);
        //    selectButton.onClick.RemoveAllListeners();
        //    selectButton.onClick.AddListener(Destroy);
        //}


        //private void Destroy()
        //{
        //    DeckManager.player.DestroyCard(selectedCard);

        //    closeButton.gameObject.SetActive(true);
        //    closeButton.onClick.RemoveAllListeners();
        //    closeButton.onClick.AddListener(closePanel);

        //    selectButton.gameObject.SetActive(false);
        //}


        //internal void UpgradeCardPanel()
        //{
        //    deckPanelObject.SetActive(true);
        //    isActive = true;

        //    closeButton.gameObject.SetActive(false);

        //    selectButton.gameObject.SetActive(true);
        //    selectButton.onClick.RemoveAllListeners();
        //    selectButton.onClick.AddListener(Upgrade);
        //}


        //private void Upgrade()
        //{
        //    var level = selectedCard.GetComponent<CardManager>().card.level;
        //    DeckManager.player.DestroyCard(selectedCard);

        //    var newCard = new Card();

        //    if (selectedCard.GetComponent<CardManager>().card.type == Card.Type.ClassCard)
        //    {
        //        newCard = DAL.ObjectDAL.GetRandomClassCard(level + 1, level + 1);
        //    }
        //    else
        //    {
        //        newCard = DAL.ObjectDAL.GetRandomCard(level + 1, level + 1);
        //    }
        //    DeckManager.player.AddCardtoDeck(newCard.cardName);

        //    closeButton.gameObject.SetActive(true);
        //    closeButton.onClick.RemoveAllListeners();
        //    closeButton.onClick.AddListener(closePanel);

        //    selectButton.gameObject.SetActive(false);
        //}


        private void ClosePanel()
        {
            
            isActive = false;
            ClearPanel();
            deckPanelObject.SetActive(false);
            //GameManager.instance.dungeonUI.modalPanel.isActive = true;
        }

        private void ClearPanel()
        {
            if (selectedCard)
            {
                selectedCard.GetComponent<Selectable>().ClearOutline();
                selectedCard.GetComponent<CardManager>().imagePanel.ResetPanel();
                selectedCard = null;

            }
            selectButton.gameObject.SetActive(false);
            selectButton.onClick.RemoveAllListeners();
            applyButton.gameObject.SetActive(false);
            applyButton.onClick.RemoveAllListeners();
            closeButton.gameObject.SetActive(false);
            closeButton.onClick.RemoveAllListeners();
        }
    }
}