using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public class FullCardEditor : MonoBehaviour
    {
        private CardWrapper alLCards;
        public List<Card> cardsToEdit;
        public GameObject cardObject;
        public GameObject editorPanel;
        public List<GameObject> cardObjects;
        public string cardToEdit;
        private Card cardBeingEdited;
        public GameObject cardObjectBeingEdited;
        public Enemy enemy;
        public string enemyName;
        public int enemyLevel;
        public PlayerClass.ClassName cardType = PlayerClass.ClassName.Monster;

        public void GetCardsToEdit()
        {
            Clear();
            alLCards = ObjectDAL.GetAllCards();

            foreach (Card card in alLCards.cardItems)
            {
                if (card.classOwner == cardType) {
                    CreateCardObject(card);
                    cardsToEdit.Add(card);
                }
            }
        }
        
        public void GetMonsterDeck()
        {

            var enemies = ObjectDAL.GetAllEnemies();
            enemy = enemies.EnemyItems.Find(item => item.EnemyName.Equals(enemyName));

            var enemyCards = EnemyDeckBuilder.BuildMonsterDeck(enemy.Components, enemyLevel);
            foreach (Card card in enemyCards)
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
            alLCards = null;
            cardsToEdit.Clear();

        }

        public void EditCard()
        {
            // Clear();
            if (cardObjectBeingEdited != null)
            {
                cardObjectBeingEdited.GetComponent<Selectable>().outline.enabled = false;
            }

            var card = GetCard(cardToEdit);
            cardBeingEdited = card;

            cardObjectBeingEdited = cardObjects.Find(item => item.GetComponent<CardManager>().card.cardName.Equals(cardToEdit));

            cardObjectBeingEdited.GetComponent<Selectable>().outline.enabled = true;


            // CreateCardObject(card);
            //  cardObjectBeingEdited.transform.localScale = new Vector3(2f, 2f, 2f);
        }


        internal Card GetCard(string cardToGet)
        {
            var card = alLCards.cardItems.Find(item => item.cardName.Equals(cardToGet));
            return card;
        }

        public void ApplyCardChanges()
        {
            //reverse populate the card

            var edited = cardObjectBeingEdited.GetComponent<CardManager>();

          //  cardBeingEdited.spriteColor = edited.imagePanel.cardImage.color;
            //cardBeingEdited.spriteHighlightColor = edited.imagePanel.highlight.color;
            //cardBeingEdited.spriteGlowColor = edited.imagePanel.glow.color;

            //cardBeingEdited.spriteBackgroundColor = edited.imagePanel.imageBackground.color;
            //cardBeingEdited.backgroundGlowColor = edited.imagePanel.backgroundGlow.color;

            cardBeingEdited.backgroundColor = edited.cardPanel.color;

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
            if (alLCards != null)
            {
                ObjectDAL.SaveCards(alLCards);
            }
            else
            {
                Debug.Log("CardWrapper was null, not saving.");
            }
        }        
    }
}
