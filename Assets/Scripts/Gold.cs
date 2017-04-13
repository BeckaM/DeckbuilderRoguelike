using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Gold : MonoBehaviour
    {
        public GameObject goldObject;

        public Text goldDescription;
        public Sprite goldImage;
        public Text goldName;
        public int goldValue;

        public void PopulateGold(int goldValue)
        {
            var gold = goldValue * GameManager.instance.perkManager.goldIncrease;
            this.goldValue =(int)gold;
            goldDescription.text = "" + goldValue + " Gold Pieces.";            
            goldName.text = "Gold!";
        }

    }
}
