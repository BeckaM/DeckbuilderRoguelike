using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.Scripts
{

    public class EnemyManager : MonoBehaviour
    {

        private const string fileName = @"C:\Users\Public\Documents\Unity Projects\DeckbuilderRoguelike\Assets\JSON\Enemies.json";


        public string EnemyName;
        public int SpriteIcon;
        public int EnemyLevel;
        public int HP;

        public List<string> EnemyDeck;

        public GameObject EnemyObject;
        public Sprite[] sprites;

        public List<DeckComponent> Components;









        //Useless function, only for creating new enemies.
        public void CreateEnemy()
        {
            string text = File.ReadAllText(fileName);
            var enemylist = JsonUtility.FromJson<EnemyWrapper>(text);

            Enemy enemy = new Enemy
            {
                EnemyName = this.EnemyName,
                SpriteIcon = this.SpriteIcon,
                EnemyLevel = this.EnemyLevel,
                HP = this.HP,
                EnemyDeck = this.EnemyDeck
            };

            if (enemylist == null)
            {
                enemylist = new EnemyWrapper();
                enemylist.EnemyItems = new System.Collections.Generic.List<Enemy>();
            }

            enemylist.EnemyItems.Add(enemy);


            string jsonEnemy = JsonUtility.ToJson(enemylist);
            SaveEnemy(jsonEnemy);
        }



        //useless function, only for saving an enemy to File.
        private void SaveEnemy(string awesomeNewMonster)
        {
            if (!File.Exists(fileName))
            {
                return;
            }

            File.WriteAllText(fileName, awesomeNewMonster);
        }


        //Gets monster properties from Dungeon Manager and sets them on the enemy object.
        public void GetEnemy(Enemy enemy)
        {

            EnemyName = enemy.EnemyName;
            SpriteIcon = enemy.SpriteIcon;
            EnemyLevel = enemy.EnemyLevel;
            HP = enemy.HP;
            EnemyDeck = enemy.EnemyDeck;

            var transformer = this.transform;

            //Set Image
            var imageObj = transformer.GetChild(0);
            var imageComponent = imageObj.GetComponent<SpriteRenderer>();
            imageComponent.sprite = sprites[enemy.SpriteIcon];
        }
        
        //Create all the cards in the scene for the monsters deck when the player fights it. 
        internal void InitMonsterDeck()
        {
            throw new NotImplementedException();
        }
    }
}
