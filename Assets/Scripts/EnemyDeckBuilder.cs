//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class EnemyDeckBuilder
    {
        private List<string> FinalList = new List<string>();

        //This is where we build the enemy deck.
        public void BuildMonsterDeck(List<DeckComponent> DeckComponents, int EnemyLevel)
        {   
            foreach (DeckComponent comp in DeckComponents)
            {
                var cardnumbers = comp.cardcount;
                var cardstoget = comp.CardNames;
                
                for (int i = 0; i < cardnumbers; i++)
                {
                    string cardChoice = cardstoget[Random.Range(0, cardstoget.Count)];                  
                    FinalList.Add(cardChoice);
                }
            }


            DeckManager.monster.AddCardstoDeck(FinalList);

        }
    }
}
