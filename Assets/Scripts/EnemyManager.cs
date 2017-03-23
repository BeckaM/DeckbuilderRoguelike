using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts
{

    public class EnemyManager : MonoBehaviour
    {
        public Enemy enemy;

        public int life;
        public int maxLife;
        public int ward = 0;
        public int damageBoost = 0;

        public int mana = 1;
        public int maxMana = 1;

        public int monsterLevel;
        public int experienceReward;

        public GameObject enemyObject;
        public Sprite[] sprites;
      //  public Sprite monsterImage;
        public SpriteRenderer monsterRenderer;
        public TMP_Text monsterLVLText;

        //Gets monster properties from Dungeon Manager and sets them on the enemy object.
        public void PopulateEnemy(Enemy enemytoget, int level)
        {
            monsterLevel = level;
            enemy = enemytoget;

            int HPperlevel = enemy.BaseEnemyHP / 10;
            int HPbonus = HPperlevel * (monsterLevel - enemy.BaseEnemyLevel);
            life = enemy.BaseEnemyHP + (HPperlevel * HPbonus);
            maxLife = life;
            experienceReward = 4 + monsterLevel;

            var transformer = transform;

            //Set Image
            monsterRenderer.sprite = sprites[enemy.SpriteIcon];

            //Set lvl text
            monsterLVLText.text = "LVL" + level.ToString();
        }

        //Create all the cards in the scene for the monsters deck when the player fights it. 
        internal void InitMonsterDeck()
        {                        
            var enemyDeck = EnemyDeckBuilder.BuildMonsterDeck(enemy.Components, monsterLevel);

            DeckManager.monster.AddCardstoDeck(enemyDeck);

        }

        internal void initAI()
        {
            MonsterBrain.PlayCards();

            CardgameManager.instance.EndTurn();


        }
    }
}
