using System.IO;
using UnityEngine;

namespace Assets.Scripts
{

    public class EnemyManager : MonoBehaviour
    {

        

        public Enemy enemy;
        public int MonsterHP;
        public int MonsterLevel;

        public GameObject EnemyObject;
        public Sprite[] sprites;  



        //Gets monster properties from Dungeon Manager and sets them on the enemy object.
        public void PopulateEnemy(Enemy enemytoget, int Level)
        {

            enemy = enemytoget;

            MonsterLevel = Random.Range(enemy.BaseEnemyLevel, Level+1);

            int HPperlevel = enemy.BaseEnemyHP / 10;
            int HPbonus = HPperlevel*(MonsterLevel-enemy.BaseEnemyLevel);
            MonsterHP = enemy.BaseEnemyHP+(HPperlevel*HPbonus);

            var transformer = transform;

            //Set Image
            var imageObj = transformer.GetChild(0);
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
    }
}
