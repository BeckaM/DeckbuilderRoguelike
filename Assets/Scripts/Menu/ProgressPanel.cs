using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Assets.Scripts.Menu
{

    public class ProgressPanel : MonoBehaviour
    {
        public Menu mainMenu;

        public TMP_Text monsterKills;

        public GameObject classPanel;
        public TMP_Text monsterKillsClass;

        public Button closeButton;

        public GameObject metricObject;
        public GameObject totalProgressPane;

        public void ShowProgress()
        {
            var cumulativeMetrics = GameManager.instance.progressManager.totalProgress.cumulativeMetrics;
            var highestAchievedMetrics = GameManager.instance.progressManager.totalProgress.highestAchievedMetrics;

            foreach (ProgressManager.Metric metric in cumulativeMetrics.Keys)
            {
                var metricDisplay = Instantiate(metricObject);
                metricDisplay.transform.SetParent(totalProgressPane.transform, false);
                var script = metricDisplay.GetComponent<MetricDisplay>();

                script.PopulateMetric(metric, cumulativeMetrics[metric]);
            }

            foreach (ProgressManager.Metric metric in highestAchievedMetrics.Keys)
            {
                var metricDisplay = Instantiate(metricObject);
                metricDisplay.transform.SetParent(totalProgressPane.transform, false);
                var script = metricDisplay.GetComponent<MetricDisplay>();

                script.PopulateMetric(metric, highestAchievedMetrics[metric]);
            }
        }

        
        public void ShowClassProgress(PlayerClass selectedClass)
        {
            var progress = GameManager.instance.progressManager.totalProgress;
            var currentClass = progress.classProgressList.Find(item => item.className.Equals(selectedClass.className));
            classPanel.SetActive(true);

            monsterKillsClass.text = currentClass.cumulativeMetrics[ProgressManager.Metric.Monsters_Killed].ToString();

        }


    }
}
