using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{

    public class ChestManager : MonoBehaviour
    {
        internal void PopulateChest(int level)
        {
            
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
