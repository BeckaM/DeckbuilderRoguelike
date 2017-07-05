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

        public GameObject metricWindow;
        public GameObject metricObject;
        public GameObject metricPanel;

        public GameObject unlockWindow;
        public GameObject unlockObject;
        public GameObject unlockPanel;

        private bool newUnlocks=false;

        internal void GameOver()
        {
            unlockWindow.SetActive(false);
            this.gameObject.SetActive(true);
            metricWindow.SetActive(true);
        }

        internal void UpdateGameOverText(int dungeonLevel, int playerLevel)
        {
            dungeonLevelText.text = "You died on level " + dungeonLevel;

           // playerLVLText.text = "Player Level: " + playerLevel;

            var cumulativeMetrics = GameManager.instance.progressManager.currentRunProgress.cumulativeMetrics;
            var highestAchievedMetrics = GameManager.instance.progressManager.currentRunProgress.highestAchievedMetrics;

            foreach (ProgressManager.Metric metric in cumulativeMetrics.Keys)
            {
                var metricDisplay = Instantiate(metricObject);
                metricDisplay.transform.SetParent(metricPanel.transform, false);
                var script = metricDisplay.GetComponent<MetricDisplay>();

                script.PopulateMetric(metric, cumulativeMetrics[metric]);
            }

            //foreach (ProgressManager.Metric metric in highestAchievedMetrics.Keys)
            //{
            //    var metricDisplay = Instantiate(metricObject);
            //    metricDisplay.transform.SetParent(metricPanel.transform, false);
            //    var script = metricDisplay.GetComponent<MetricDisplay>();

            //    script.PopulateMetric(metric, highestAchievedMetrics[metric]);
            //}

        }

        public void ShowUnlocks()
        {
            if (newUnlocks)
            {
                metricWindow.SetActive(false);
                unlockWindow.SetActive(true);
            }
            else
            {
                BackToMenu();
            }
        }

        internal void UpdateNewUnlocks(List<PlayerClass> newClassUnlocks, List<Item> newPerkUnlocks)
        {
            foreach(PlayerClass pClass in newClassUnlocks)
            {
                var classUnlock = Instantiate(unlockObject);
                classUnlock.transform.SetParent(unlockPanel.transform, false);
                var cbScript = classUnlock.GetComponent<UnlockObject>();
                
                cbScript.PopulateUnlock(pClass);
                newUnlocks = true;
                
            }

            foreach (Item perk in newPerkUnlocks)
            {
                var classUnlock = Instantiate(unlockObject);
                classUnlock.transform.SetParent(unlockPanel.transform, false);
                var cbScript = classUnlock.GetComponent<UnlockObject>();

                cbScript.PopulateUnlock(perk);
                newUnlocks = true;
            }
        }

        public void BackToMenu()
        {
            GameManager.instance.BackToMenu();
        }              
    }
}
