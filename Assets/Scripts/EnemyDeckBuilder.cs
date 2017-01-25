using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class EnemyDeckBuilder
    {
        public void BuildMonsterDeck(List<DeckComponent> DeckComponents)
        {

            foreach (DeckComponent comp in DeckComponents)
            {
                var cardstoget = comp.CardNames;
                DeckManager.instance.JSONreader(cardstoget);

            }

        }
    }
}
