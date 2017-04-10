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

        public GameObject unlockObject;

        public GameObject unlockPanel;

        internal void UpdateGameOverText(int dungeonLevel, int playerLevel)
        {
            dungeonLevelText.text = "You died on level " + dungeonLevel;

            playerLVLText.text = "Player Level: " + playerLevel;

            monsterKillsText.text = "Monster Kills" + GameManager.instance.progressManager.GetCurrentRunMetric(ProgressManager.Metric.MonsterKills);

            //monsterKillsText.text = "Monster Kills: " + playerProgress.cumulativeMetrics[ProgressManager.Metric.MonsterKills];
            //cardsPlayedText.text = "Cards Played: " + playerProgress.cumulativeMetrics[ProgressManager.Metric.CardsPlayed];
            //goldEarnedText.text = "Gold Earned: " + playerProgress.cumulativeMetrics[ProgressManager.Metric.GoldEarned];
            //damageDealtText.text = "Damage Dealt: " + playerProgress.cumulativeMetrics[ProgressManager.Metric.DamageDealt];
            //healingText.text = "Healing Done: " + playerProgress.cumulativeMetrics[ProgressManager.Metric.Healing];
            //chestsOpenedText.text = "Chests Opened: " + playerProgress.cumulativeMetrics[ProgressManager.Metric.ChestsOpened];
            //shrinesFoundText.text = "Shrines Found: " + playerProgress.cumulativeMetrics[ProgressManager.Metric.ShrinesOpened];

        }

        public void ShowUnlocks()
        {
            
        }

        internal void UpdateNewUnlocks(List<PlayerClass> newClassUnlocks, List<Perk> newPerkUnlocks)
        {
            foreach(PlayerClass pClass in newClassUnlocks)
            {
                var classUnlock = Instantiate(unlockObject);
                classUnlock.transform.SetParent(unlockPanel.transform, false);
                var cbScript = classUnlock.GetComponent<UnlockObject>();
                
                cbScript.PopulateUnlock(pClass);
            }

            foreach (Perk perk in newPerkUnlocks)
            {
                var classUnlock = Instantiate(unlockObject);
                classUnlock.transform.SetParent(unlockPanel.transform, false);
                var cbScript = classUnlock.GetComponent<UnlockObject>();

                cbScript.PopulateUnlock(perk);
            }
        }
    }
}
