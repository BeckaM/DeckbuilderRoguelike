using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Assets.Scripts
{
    public class GameOverPanel : MonoBehaviour
    {
        public TMP_Text dungeonLevelText;
        public TMP_Text playerLVLText;
        public TMP_Text monsterKillsText;
        public TMP_Text cardsPlayedText;
        public TMP_Text goldEarnedText;
        public TMP_Text damageDealtText;
        public TMP_Text healingText;
        public TMP_Text chestsOpenedText;
        public TMP_Text shrinesFoundText;


        internal void UpdateGameOverText(int dungeonLevel, int playerLevel, PlayerProgress playerProgress)
        {
            dungeonLevelText.text = "You died on level " + dungeonLevel;

            playerLVLText.text = "Player Level: " + playerLevel;
            monsterKillsText.text = "Monster Kills: " + playerProgress.monsterKills;
            cardsPlayedText.text = "Cards Played: " + playerProgress.cardsPlayed;
            goldEarnedText.text = "Gold Earned: " + playerProgress.goldEarned;
            damageDealtText.text = "Damage Dealt: " + playerProgress.damageDealt;
            healingText.text = "Healing Done: " + playerProgress.healing;
            chestsOpenedText.text = "Chests Opened: " + playerProgress.chestsOpened;
            shrinesFoundText.text = "Shrines Found: " + playerProgress.shrinesOpened;

        }

        public void ShowRewards()
        {

        }

    }
}
