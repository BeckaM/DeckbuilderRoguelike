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
        public PlayerClass.ClassName className;

        public MyDictionary cumulativeMetrics = new MyDictionary();
        public MyDictionary highestAchievedMetrics = new MyDictionary();

    }
}