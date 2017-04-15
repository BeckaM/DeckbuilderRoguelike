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
        public Sprite goldImage;
        public Text goldValueText;
        public int goldValue;

        public void PopulateGold(int goldValue)
        {
            var gold = goldValue * GameManager.instance.perkManager.goldIncrease;
            this.goldValue = (int)gold;
            goldValueText.text = goldValue.ToString() + " Gold.";
        }

    }
}
