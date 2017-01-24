
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts

{
    [Serializable]
    public class DeckComponent
    {
        public string ComponentName;
        public List<string> CardNames;
        public string cardcount;
    }





    public class EnemyDeckbuilder {

       


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

    //  RandFuncs Randomness = new RandFuncs();

    //Randomness.Sample();




        





