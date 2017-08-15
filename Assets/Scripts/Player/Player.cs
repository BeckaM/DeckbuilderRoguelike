using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class Player
    {
        public int gold=0;

        public int playerLevel = 0;
        public int playerXP = 0;
        public int nextLVLXP = 20;

        public int life = 30;
        public int maxLife = 30;

        public int armor =0;
        public int damageReduction = 0;
        public int damageBoost = 0;     

        public int mana = 1;

        public int maxMana = 3;
        public int manaPerTurn = 1;
    }
}