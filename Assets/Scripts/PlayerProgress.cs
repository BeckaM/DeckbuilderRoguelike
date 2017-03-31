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
        public int cardsPlayed;
        public int damageDealt;
        public int healing;
        public int goldEarned;
        public int highestPlayerLevel;
        public int highestDungeonLevel;
        public int chestsOpened;
        public int shrinesOpened;
    }
}