using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    
    public class ChestManager : MonoBehaviour
    {
        private enum Content {Gold, Consumable, Card};
        private Content content;
        private Card cardReward;
        
        internal void PopulateChest(int level)
        {
           var rand = UnityEngine.Random.Range(0, 10);
            if (rand == 0)
            {
                content = Content.Card;
                level = level + UnityEngine.Random.Range(0, 3);
                cardReward = DAL.ObjectDAL.GetRandomCard(level);

            }
            else if (rand > 0 && rand < 6)
            {
                content = Content.Consumable;
            }
            else
            {
                content = Content.Gold;
            }
        }

        internal void OpenChest()
        {
            GameManager.instance.panel.Chest("You found a chest!", AddReward, DeclineReward);
        }

        private void  DeclineReward()
        {
            throw new NotImplementedException();
        }

        private void AddReward()
        {
            throw new NotImplementedException();
        }
    }


    

}
