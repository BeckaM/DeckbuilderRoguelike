using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public static class MonsterBrain
    {
        //TODO: AI should play more than one card per turn.
        public static List<CardManager> playableCards
        {
            get
            {
                List<CardManager> cardlist = new List<CardManager>();
                foreach (GameObject card in DeckManager.monster.cardsInHand)
                {
                    var manager = card.GetComponent<CardManager>();
                    if (manager.isPlayable)
                    {
                        cardlist.Add(manager);
                    }
                }
                return cardlist;
            }
        }

        internal static void PlayCards()
        {
            CardManager toPlay = playableCards[UnityEngine.Random.Range(0, playableCards.Count)];
            if (toPlay)
            {
                EventManager.Instance.AddListener<MoveCard_GUI>(toPlay.Move);
                toPlay.moveCounter++;
                EventManager.Instance.QueueAnimation(new MoveCard_GUI(toPlay, DeckManager.monster.hand, CardgameManager.instance.tabletop));
                CardgameManager.instance.PlaceCard(toPlay);
            }
        }
    }
}
