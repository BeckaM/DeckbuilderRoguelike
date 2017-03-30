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

        public int player;
        public string playerName;

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