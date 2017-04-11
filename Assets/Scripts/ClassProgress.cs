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

        public MyDictionary cumulativeMetrics = new MyDictionary();
        public MyDictionary highestAchievedMetrics = new MyDictionary();

    }
}