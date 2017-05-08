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
        public List<CardManager> muliganCards;
        public GameObject muliganCardHolder;
        public int discardCards;

        public void StartMuligan()
        {
            this.gameObject.SetActive(true);
            Selectable.selectContext = Selectable.SelectContext.Muligan;
            muliganCards = DeckManager.player.GetMuliganCards();
            foreach (CardManager card in muliganCards)
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
            foreach (CardManager card in muliganCards)
            {
                var selectScript = card.GetComponent<Selectable>();
                if (selectScript.muliganKeep)
                {
                    card.SetCardPosition(CardManager.CardStatus.InHand);
                    card.transform.SetParent(DeckManager.player.hand.transform, false);
                    card.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
                    selectScript.ClearOutline();
                  //  card.GetComponent<CardManager>().imagePanel.ResetPanel();
                }
                else
                {
                    card.SetCardPosition(CardManager.CardStatus.InDiscard);
                    card.transform.SetParent(CardgameManager.instance.playerDiscard.transform, false);
                    card.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, card.DeckManager.discardOffset);
                    card.DeckManager.discardOffset -= 2;
                    //   card.GetComponent<CardManager>().ResetTransform();
                    selectScript.ClearOutline();
                  //  card.imagePanel.ResetPanel();
                    discardCards++;
                }
            }
            CardgameManager.instance.DrawStartingHands(discardCards);
            Selectable.selectContext = Selectable.SelectContext.NoSelect;
            this.gameObject.SetActive(false);
        }

        /**
         * Changes outline on selected card and sets muliganKeep bool.
         * 
         **/
        //internal void Select(GameObject selectedCard)
        //{
        //    var selectScript = selectedCard.GetComponent<Selectable>();
        //    if (selectScript.muliganKeep)
        //    {
        //        selectScript.muliganKeep = false;
        //        selectScript.RedOutline();
        //    }
        //    else
        //    {
        //        selectScript.muliganKeep = true;
        //        selectScript.ClearOutline();
        //    }


        //}
    }
}
