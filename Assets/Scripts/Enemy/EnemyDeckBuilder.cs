//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    static class EnemyDeckBuilder
    {
       // private static List<Card> finalList;

        //This is where we bake the enemy deck-cake. It's a magic recipe.
        public static List<Card> BuildMonsterDeck(List<DeckComponent> deckComponents, int enemyLevel)
        {
            var finalList = new List<Card>();

            foreach (DeckComponent comp in deckComponents)
            {
                var componentCount = comp.cardCount;
                var componentCards = comp.cardNames;
                var componentFinalList = new List<Card>();

                var cardChoices = DAL.ObjectDAL.GetCards(componentCards);

                var tempList = new List<Card>();
                tempList.AddRange(cardChoices);

                //remove cards that are higher level than monster level
                //remove cards that are 5 levels lower than monster level
                foreach (Card card in tempList)
                {
                    if (card.level > enemyLevel)
                    {
                        cardChoices.Remove(card);
                    }
                    else if (card.level <= enemyLevel - 5)
                    {
                        cardChoices.Remove(card);
                    }
                }

                tempList = new List<Card>();
                tempList.AddRange(cardChoices);                

                //add one of each valid card to the list untill we have enough
                while (componentFinalList.Count < componentCount)
                {
                    foreach (Card card in tempList)
                    {
                        componentFinalList.Add(card);
                    }
                }

                //remove cards from the top end until we have the correct ammount
                while (componentFinalList.Count > componentCount)
                {
                    componentFinalList.RemoveAt(componentFinalList.Count - 1);
                }
                finalList.AddRange(componentFinalList);
            }
                        
            return finalList;

        }
    }
}
