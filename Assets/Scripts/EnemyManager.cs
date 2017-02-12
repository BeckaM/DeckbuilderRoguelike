using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{

    public class EnemyManager : MonoBehaviour
    {

        public Enemy enemy;

        public int life;
        public int maxLife;

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


         //   return hungryMonsterScale > 100 ? "Beasty gulp murloc will eat everything" : "Not that hungry Murloc but can swallow a hero or two";
        }

        public void LoseLife(int loss)
        {

            //Subtract lost life points from the monster total.
            life -= loss;
            CardgameManager.instance.monsterLifeText.text = "-" + loss;
            Invoke("UpdateLife", 1f);
        }

        internal void UpdateLife()
        {   
            if (life > maxLife)
            {
                life = maxLife;
            }
            CardgameManager.instance.monsterLifeText.text =  "Life:" + life + "/" + maxLife;
            

        }

        internal void GainLife(int gain)
        {
            life += gain;
            CardgameManager.instance.monsterLifeText.text = "+" + gain;
            Invoke("UpdateLife", 1f);
        }
    }
}
