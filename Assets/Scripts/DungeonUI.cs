using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    public class DungeonUI : MonoBehaviour
    {

        public TMP_Text goldText;
        public TMP_Text XPText;
        public GameObject LevelUpButton;
        public TMP_Text lifeText;
        public GameObject levelImage;
        public TMP_Text levelText;

        public GameObject gameOverPanel;
        public GameOverPanel gameOverScript;

        public GameObject modalPanelObject;
        public GameObject deckPanelObject;

        

        public void UpdateXPText()
        {
            XPText.text = "XP:<#06409b>" + GameManager.instance.playerXP + "/" + GameManager.instance.nextLVLXP;
        }


        public void UpdateGoldText()
        {            
            goldText.text = "Gold:<#9b8e05>" + GameManager.instance.gold;
        }


        public void UpdateLifeText()
        {
            lifeText.text = "Life:<#910000>" + GameManager.instance.lifeHolder + "/" + GameManager.instance.maxLife;
        }


    }
}