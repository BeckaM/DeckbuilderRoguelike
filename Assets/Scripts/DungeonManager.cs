using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.
using System.IO;


namespace Assets.Scripts
{

    public class DungeonManager : MonoBehaviour
    {

        //Array of wall prefabs.
        // public GameObject[] enemyTiles;                                 //Array of enemies to place.
        public GameObject chestPrefab;
        public GameObject shrinePrefab;
        public GameObject enemyPrefab; //Enemy prefab.
        public GameObject anvilPrefab;

        int enemyCount;

        public List<int> enemyLevels;

        public List<GameObject> dungeonObjects;

        private MapGenerator mapGen;                                  //A variable to store a reference to the transform of our Board object.



        //SetupScene initializes our level and calls the previous functions to lay out the game board
        public void SetupScene(int level)
        {
            var m = GameObject.Find("Map Generator");
            mapGen = m.GetComponent<MapGenerator>();
            var size = 50 + level * 2;

            mapGen.width = size;
            mapGen.height = size;

            PrepareMapObjects(level);

            mapGen.GenerateMap();

            mapGen.PlacePlayerAndExit();

            mapGen.PlaceObjects(dungeonObjects);
        }


        private void PrepareMapObjects(int level)
        {
            dungeonObjects = new List<GameObject>();

            GetEnemies(level);
            GetChests(level);
            GetShrines(level);
            GetAnvils(level);
        }


        private void GetEnemies(int Level)
        {
            Debug.Log("Getting enemylist for level " + Level);
            if (Level == 1)
            {
                enemyLevels = new List<int> { 1, 1, 1 };
            }
            else
            {
                Debug.Log("Old enemylist" + enemyLevels.Count);
                //Determine number of enemies based on current level number, based on a logarithmic progression
                enemyCount = (int)Mathf.Log(Level, 2f) + 3;

                //if we should have more enemies, add one to the end of list. It will be the lowest level enemy from the last level.
                if (enemyCount > enemyLevels.Count)
                {
                    Debug.Log("Enemy count increased");
                    var newEnemy = enemyLevels[enemyLevels.Count - 1];
                    enemyLevels.Add(newEnemy);
                }

                //Gradually increase the levels of all enemies in the list
                //Remove the lowest level enemy.
                enemyLevels.RemoveAt(enemyLevels.Count - 1);

                //Add a new enemy with the same leves as the second biggest monster+1. This gives a good progression.
                var highEnemy = enemyLevels[1] + 1;
                enemyLevels.Insert(0, highEnemy);
            }
            Debug.Log("New enemylist" + enemyLevels.Count);

            //Get all enemies with level lower than current dungeon Level
            var enemiesToChooseFrom = DAL.ObjectDAL.GetEnemies(Level);
            // List<Enemy> enemyList = new List<Enemy>();

            foreach (int enemyLevel in enemyLevels)
            {
                Enemy enemyChoice = enemiesToChooseFrom[Random.Range(0, enemiesToChooseFrom.Count)];

                var instance = Instantiate(enemyPrefab);

                var script = instance.GetComponent<EnemyManager>();

                script.PopulateEnemy(enemyChoice, enemyLevel);

                dungeonObjects.Add(instance);
            }
        }


        private void GetChests(int Level)
        {
            //Determine number of chests based on current level number, based on a logarithmic progression
            int chestCount = (int)Mathf.Log(Level, 2f) + 1;

            for (int i = 0; i < chestCount; i++)
            {
                var instance = Instantiate(chestPrefab);

                var script = instance.GetComponent<ChestManager>();

                script.PopulateChest(Level);

                dungeonObjects.Add(instance);
            }
        }


        private void GetShrines(int Level)
        {

            //Determine number of shrines
            int shrineCount = 1;

            for (int i = 0; i < shrineCount; i++)
            {
                var instance = Instantiate(shrinePrefab);

                var script = instance.GetComponent<ShrineManager>();

                script.PopulateShrine(Level);

                dungeonObjects.Add(instance);
            }
        }

        private void GetAnvils(int Level)
        {
            int anvilCount=0;

            //Determine number of Anvils         
            if (Level % 2 == 0)
            {
                anvilCount = 1;
            }

            for (int i = 0; i < anvilCount; i++)
            {
                var instance = Instantiate(anvilPrefab);
                            
                dungeonObjects.Add(instance);
            }
        }
    }
}

