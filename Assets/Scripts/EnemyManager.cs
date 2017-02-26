using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{

    public class EnemyManager : MonoBehaviour
    {

        public Enemy enemy;

        public int life;
        public int maxLife;
        public int ward = 0;

        public int mana = 1;
        public int maxMana = 1;

        public int MonsterLevel;

        public GameObject EnemyObject;
        public Sprite[] sprites;
        public Sprite monsterImage;

        
        //Gets monster properties from Dungeon Manager and sets them on the enemy object.
        public void PopulateEnemy(Enemy enemytoget, int Level)
        {

            enemy = enemytoget;

            MonsterLevel = UnityEngine.Random.Range(enemy.BaseEnemyLevel, Level+1);

            int HPperlevel = enemy.BaseEnemyHP / 10;
            int HPbonus = HPperlevel*(MonsterLevel-enemy.BaseEnemyLevel);
            life = enemy.BaseEnemyHP+(HPperlevel*HPbonus);
            maxLife = life;

            var transformer = transform;

            //Set Image
            var imageObj = transformer.GetChild(0);
            monsterImage = sprites[enemy.SpriteIcon];
            var imageComponent = imageObj.GetComponent<SpriteRenderer>();
            imageComponent.sprite = sprites[enemy.SpriteIcon];
        }
        
        //Create all the cards in the scene for the monsters deck when the player fights it. 
        internal void InitMonsterDeck()
        {

            var EnemyDeckBuilder = new EnemyDeckBuilder();
            EnemyDeckBuilder.BuildMonsterDeck(enemy.Components, MonsterLevel);
                                 
        }

        internal void initAI()
        {
            MonsterBrain.PlayCards();

            CardgameManager.instance.EndTurn();


        }
    }
}
