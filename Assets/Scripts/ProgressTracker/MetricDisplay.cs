using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts
{

    public class MetricDisplay : MonoBehaviour
    {      
               
        public TMP_Text metricText;

        public void PopulateMetric(ProgressManager.Metric metric, int value)
        {
            metricText.text = metric.ToString().Replace("_", " ") + ": " + value.ToString();
            
        }

    }
}