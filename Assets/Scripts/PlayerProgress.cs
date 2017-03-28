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

        public int player;
        public string playerName;

        public int monsterKills;
               
    }
}