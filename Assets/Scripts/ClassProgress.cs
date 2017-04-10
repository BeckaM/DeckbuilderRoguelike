using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class ClassProgress
    {
        public string className;

        public Dictionary<ProgressManager.Metric, int> cumulativeMetrics = new Dictionary<ProgressManager.Metric, int>();
        public Dictionary<ProgressManager.Metric, int> highestAchievedMetrics = new Dictionary<ProgressManager.Metric, int>();

    }
}