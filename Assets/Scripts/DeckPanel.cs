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


        internal void Select()
        {
            selectButton.interactable = true;
            selectedCard = EventSystem.current.currentSelectedGameObject;
        }


        public void CardSelect()
        {
            Selectable.selectContext = Selectable.SelectContext.DeckPanel;
            deckPanelObject.SetActive(true);
                       
            selectButton.gameObject.SetActive(true);
            selectButton.interactable = false;
            selectButton.onClick.AddListener(SelectCard);
            selectButton.onClick.AddListener(ClosePanel);
        }


        private void SelectCard()
        {
            Selectable.selectContext = Selectable.SelectContext.NoSelect;
            GameManager.instance.dungeonUI.modalPanel.SelectCard(selectedCard);
        }

        //Show the deck panel and the players deck. Triggered from the button in the main dungeon UI. 
        internal void ShowDeckPanel()
        {
            //GameManager.instance.dungeonManager.gameObject.SetActive(false);
            deckPanelObject.SetActive(true);
            

            closeButton.gameObject.SetActive(true);
            closeButton.onClick.AddListener(ClosePanel);
        }
                      
        
        internal void DestroyRandomCardsPanel()
        {
            deckPanelObject.SetActive(true);
            
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




        private void ClosePanel()
        {

            
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