using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /**
     * A Panel where the player can choose cards to muligan at the start of the turn. 
     * @author: Per and Jonte. 
     **/
    public class MuliganPanel : MonoBehaviour
    {
        public List<GameObject> muliganCards;
        public GameObject muliganCardHolder;
        public int discardCards;

        public void StartMuligan()
        {
            this.gameObject.SetActive(true);
            muliganCards = DeckManager.player.GetMuliganCards();
            foreach (GameObject card in muliganCards)
            {
                card.transform.SetParent(muliganCardHolder.transform, false);
            }
        }
        /**
         * Moves the cards the player muliganed to your hand and discards the rest. Disables the
         * muliganPanel when done. 
         * @author: Per and JonteKing.
         * 
        **/
        public void MuliganComplete()
        {
            discardCards = 0;
            foreach (GameObject card in muliganCards)
            {
                var selectScript = card.GetComponent<Selectable>();
                if (selectScript.muliganKeep)
                {
                    card.GetComponent<CardManager>().SetCardPosition(CardManager.CardStatus.InHand);
                    card.transform.SetParent(DeckManager.player.hand.transform, false);
                    selectScript.ClearOutline();
                    card.GetComponent<CardManager>().imagePanel.ResetPanel();
                }
                else
                {
                    card.GetComponent<CardManager>().SetCardPosition(CardManager.CardStatus.InDiscard);
                    card.transform.SetParent(DeckManager.player.deckHolder.transform, false);
                    selectScript.ClearOutline();
                    card.GetComponent<CardManager>().imagePanel.ResetPanel();
                    discardCards++;
                }
            }
            CardgameManager.instance.DrawStartingHands(discardCards);
            this.gameObject.SetActive(false);
        }

        /**
         * Changes outline on selected card and sets muliganKeep bool.
         * 
         **/
        internal void Select(GameObject selectedCard)
        {
            var selectScript = selectedCard.GetComponent<Selectable>();
            if (selectScript.muliganKeep)
            {
                selectScript.muliganKeep = false;
                selectScript.RedOutline();
            }
            else
            {
                selectScript.muliganKeep = true;
                selectScript.ClearOutline();
            }


        }
    }
}
