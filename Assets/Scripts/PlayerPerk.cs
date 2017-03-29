using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [Serializable]
    public class Perk
    {
        public string perkName;
        public string perkEffectText;
        public int perkCost;

        public string perkConditionText;
        public ProgressManager.Metric condition;
        public int conditionValue;
    }


    [Serializable]
    public class PerkWrapper
    {
        public List<Perk> perkList;
    }
}