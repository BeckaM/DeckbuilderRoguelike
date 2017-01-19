using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Enemy
    {
        public string EnemyName;
        public int SpriteIcon;
        public int Level;
        public int HP;
        public List<string> EnemyDeck;
    }

    [Serializable]
    public class EnemyWrapper
    {
        public List<Enemy> EnemyItems;
    }
}