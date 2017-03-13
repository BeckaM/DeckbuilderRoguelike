﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public class FullCardEditor : MonoBehaviour
    {
        public CardWrapper cardsToEdit;
        public GameObject cardObject;
        public GameObject editorPanel;
        public List<GameObject> cardObjects;
        public string cardToEdit;
        public Card cardBeingEdited;
        public GameObject cardObjectBeingEdited;

        public void GetCardsToEdit()
        {
            Clear();
            cardsToEdit = ObjectDAL.GetAllCards();

            foreach (Card card in cardsToEdit.cardItems)
            {
                CreateCardObject(card);


            }

        }

        public void Clear()
        {
            foreach (GameObject card in cardObjects)
            {
                DestroyImmediate(card);
            }
            cardObjects.Clear();
        }


        public void EditCard()
        {
            // Clear();

            cardObjectBeingEdited.GetComponent<Selectable>().outline.enabled = false;

            var card = GetCard(cardToEdit);
            cardBeingEdited = card;

            cardObjectBeingEdited = cardObjects.Find(item => item.GetComponent<CardManager>().card.cardName.Equals(cardToEdit));

            cardObjectBeingEdited.GetComponent<Selectable>().outline.enabled = true;


            // CreateCardObject(card);
            //  cardObjectBeingEdited.transform.localScale = new Vector3(2f, 2f, 2f);
        }


        internal Card GetCard(string cardToGet)
        {
            var card = cardsToEdit.cardItems.Find(item => item.cardName.Equals(cardToGet));
            return card;
        }


        public void ApplyCardChanges()
        {
            //reverse populate the card
                                              
            var edited = cardObjectBeingEdited.GetComponent<CardManager>();

            cardBeingEdited.spriteColor = edited.cardImage.color;
            cardBeingEdited.spriteHighlightColor = edited.highlight.color;
            cardBeingEdited.spriteGlowColor = edited.glow.color;
                        
            cardBeingEdited.spriteBackgroundColor= edited.imageBackground.color;
            cardBeingEdited.backgroundGlowColor = edited.backgroundGlow.color;

            cardBeingEdited.backgroundColor = edited.cardPaneImage.color;

            cardBeingEdited.cardName = edited.cardName.text;

            cardBeingEdited.cardText = edited.descriptionText.text;
            ////Set Card Title
            ////   var titleComponent = cardName.GetComponent<Text>();
            //cardName.text = card.cardName;

            ////Set Card Description
            ////  var cardtext = cardDescription.GetComponent<Text>();
            //descriptionText.text = card.cardText;
            

        }

        private void CreateCardObject(Card card)
        {
            GameObject instance = Instantiate(cardObject);
            //  as GameObject;
            var cardManager = instance.GetComponent<CardManager>();

            instance.transform.SetParent(editorPanel.transform);
            instance.transform.localScale = new Vector3(1f, 1f, 1f);

            cardManager.PopulateCard(card);
            cardObjects.Add(instance);
            cardObjectBeingEdited = instance;
        }

        public void SaveCards()
        {
            ObjectDAL.SaveCards(cardsToEdit);

        }
    }
}
