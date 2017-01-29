﻿using System;
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
        public int EnemyLevel;
        public int HP;

        public List<DeckComponent> Components;
    }

    [Serializable]
    public class EnemyWrapper
    {
        public List<Enemy> EnemyItems;
    }
}