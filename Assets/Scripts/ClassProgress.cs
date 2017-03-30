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

        public int monsterKills;
        private int cardsPlayed;
        private int damageDealt;
        private int healing;
        private int goldEarned;
        private int highestPlayerLevel;
        private int highestDungeonLevel;
        private int chestsOpened;
        private int shrinesOpened;

    }
}