using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ViewDeckButton : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(ShowDeck);
        }

        private void ShowDeck()
        {
            GameManager.instance.deckPanel.SetActive(true);
            GameManager.instance.deckPanel.GetComponent<DeckPanel>().ShowDeck();
        }
    }


}
