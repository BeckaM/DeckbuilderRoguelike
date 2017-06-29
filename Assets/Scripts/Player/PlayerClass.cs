using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class PlayerClass
    {
        public enum ClassName { Monster, Iron_Soul, Fate_Weaver, Keeper_Of_The_Source }
        public ClassName className;
        public int spriteIcon;
        public GameObject playerModel;
        public List<string> startingDeck;

        public string conditionText;
        public ProgressManager.Metric condition;
        public int conditionValue;
    }

    [Serializable]
    public class PlayerClassWrapper
    {
        public List<PlayerClass> playerClasses;
    }
}