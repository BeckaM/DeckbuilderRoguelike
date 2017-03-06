using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DeckPanel : MonoBehaviour
    {
        public Button closeButton;
        public Button chooseButton;

        private static DeckPanel deckPanel;
        public GameObject deckPanelObject;

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

        internal void ShowDeck()
        {
            closeButton.gameObject.SetActive(true);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(closePanel);

            chooseButton.gameObject.SetActive(false);
        }

        internal void ChooseOne()
        {
            closeButton.gameObject.SetActive(true);
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(closePanel);

            chooseButton.gameObject.SetActive(false);
        }



        private void closePanel()
        {
            deckPanelObject.SetActive(false);
        }

    }
}