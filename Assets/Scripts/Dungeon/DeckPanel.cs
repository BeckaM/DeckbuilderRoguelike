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


        //This is called when a card is selected in card select mode. Triggered by the Selectable script.
        internal void Select()
        {
            selectButton.GetComponent<Button>().interactable = true;
        }


        //Open the deckpanel in card select mode.
        public void CardSelectMode()
        {
            Selectable.selectContext = Selectable.SelectContext.DeckPanel;
            deckPanelObject.SetActive(true);
                       
            selectButton.gameObject.SetActive(true);
            selectButton.interactable = false;
            selectButton.onClick.AddListener(ReturnSelectedCard);
            selectButton.onClick.AddListener(ClosePanel);
        }

        //returns the selected card to the modal panel.
        private void ReturnSelectedCard()
        {
            Selectable.selectContext = Selectable.SelectContext.NoSelect;
            GameManager.instance.dungeonUI.modalPanel.SelectCard(Selectable.selectedObject);
        }

        //Shows the deck panel and the players deck. Triggered from the button in the main dungeon UI. 
        internal void ShowDeckPanel()
        {            
            deckPanelObject.SetActive(true);            

            closeButton.gameObject.SetActive(true);
            closeButton.onClick.AddListener(ClosePanel);
        }
                      
        //Shows the deckpanel after triggering the Chaos prayer Shrine.
        internal void DestroyRandomCardsPanel()
        {
            deckPanelObject.SetActive(true);
            
            closeButton.gameObject.SetActive(false);

            applyButton.gameObject.SetActive(true);
            applyButton.onClick.RemoveAllListeners();
            applyButton.onClick.AddListener(DestroyRandom);
        }

        //Destroys three random cards. Used by the Chaos prayer shrine.
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
        }

        private void ClearPanel()
        {
            if (selectedCard)
            {               
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