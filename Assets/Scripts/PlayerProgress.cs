using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class PlayerProgress
    {
        public List<ClassProgress> classProgressList = new List<ClassProgress>();
        public List<string> perkProgressList = new List<string>();

        public Dictionary<ProgressManager.Metric, int> cumulativeMetrics = new Dictionary<ProgressManager.Metric, int>();
        public Dictionary<ProgressManager.Metric, int> highestAchievedMetrics = new Dictionary<ProgressManager.Metric, int>();

        public int player;
        public string playerName;
               
    }
}