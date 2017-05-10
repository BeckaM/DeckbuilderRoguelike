using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public static class MonsterBrain
    {        
        public static List<CardManager> playableCards
        {
            get
            {
                List<CardManager> cardlist = new List<CardManager>();
                foreach (CardManager card in DeckManager.monster.cardsInHand)
                {                   
                    if (card.IsPlayable)
                    {
                        cardlist.Add(card);
                    }
                }
                return cardlist;
            }
        }


        internal static void PlayCards()
        {
            while (playableCards.Count > 0)
            {
                PlayCard(ChooseCard());
                Debug.Log("AI played card");
            }
            Debug.Log("AI done playing cards");
        }


        internal static CardManager ChooseCard()
        {
            int highestWeight = 0;
            CardManager cardChoice = null;
            foreach (CardManager card in playableCards)
            {
                var weight = CardWeight(card);

                if (weight > highestWeight)
                {
                    highestWeight = weight;
                    cardChoice = card;
                }
            }
                       
            return cardChoice;
        }

        private static int CardWeight(CardManager card)
        {
            int weight = 1;

            if (card.card.cardDuration == -1)
            {
                weight += 10;
            }
            else
            {
                weight += card.card.cardDuration;
            }

            return weight;
        }


        internal static void PlayCard(CardManager toPlay)
        {
            EventManager.Instance.AddListener<MoveCard_GUI>(toPlay.Move);
            toPlay.moveCounter++;
            EventManager.Instance.QueueAnimation(new MoveCard_GUI(toPlay, DeckManager.monster.hand, CardgameManager.instance.tabletop));
            CardgameManager.instance.PlaceCard(toPlay);
        }

    }
}
