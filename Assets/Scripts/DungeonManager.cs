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
        public GameObject bossPrefab;

        int enemyCount;

        // public List<int> enemyLevels;

        public List<GameObject> dungeonObjects;

        public MapGenerator mapGen;                                  //A variable to store a reference to the transform of our Board object.

        private int dungeonLevel;

        public GameObject player;
        public GameObject exit;




        //SetupScene initializes our level and calls the previous functions to lay out the game board
        public void SetupScene(int level)
        {
            dungeonLevel = level;
            GetComponent<MapGenerator>();
            var size = 50 + level * 2;

            mapGen.width = size;
            mapGen.height = size;

            PrepareMapObjects();

            mapGen.GenerateMap();

            PlacePlayerAndExit();

            mapGen.PlaceObjects(dungeonObjects);
        }


        private void PlacePlayerAndExit()
        {
            var spots = mapGen.GetPlayerAndExitSpots();

            player.transform.position = spots[0];
            var exitSpot = spots[1];
            exit.transform.position = exitSpot;

            switch (dungeonLevel)
            {
                case 3:
                    {
                        AddBoss("Eliath", exitSpot);
                        break;
                    }
                case 5:
                    {
                        AddBoss("The Prince", exitSpot);
                        break;
                    }
                case 10:
                    {
                        AddBoss("Lo-Ruhammah", exitSpot);
                        break;
                    }
                default:
                    {
                        exit.transform.position = spots[1];
                        break;
                    }
            }

            //exit.transform.position = CoordToWorldPoint(bestSpotB);

        }

        private void AddBoss(string bossName, Vector3 exitSpot)
        {

            exit.gameObject.SetActive(false);

            var boss = Instantiate(bossPrefab);
            boss.transform.SetParent(mapGen.transform);
            boss.transform.position = exitSpot;

            var enemy = DAL.ObjectDAL.GetEnemy(bossName);

            boss.GetComponent<EnemyManager>().PopulateEnemy(enemy, dungeonLevel);
        }


        private void PrepareMapObjects()
        {
            dungeonObjects = new List<GameObject>();

            GetEnemies(dungeonLevel);
            GetChests(dungeonLevel);
            GetShrines(dungeonLevel);
            GetAnvils(dungeonLevel);
        }


        private void GetEnemies(int level)
        {

            Debug.Log("Getting enemylist for level " + level);
            if (level == 1)
            {
                GameManager.instance.enemyLevels = new List<int> { 1, 1, 1 };
            }
            else
            {
                Debug.Log("Old enemylist" + GameManager.instance.enemyLevels.Count);
                //Determine number of enemies based on current level number, based on a logarithmic progression
                enemyCount = (int)Mathf.Log(level, 2f) + 3;

                //if we should have more enemies, add one to the end of list. It will be the lowest level enemy from the last level.
                if (enemyCount > GameManager.instance.enemyLevels.Count)
                {
                    Debug.Log("Enemy count increased");
                    var newEnemy = GameManager.instance.enemyLevels[GameManager.instance.enemyLevels.Count - 1];
                    GameManager.instance.enemyLevels.Add(newEnemy);
                }

                //Gradually increase the levels of all enemies in the list
                //Remove the lowest level enemy.
                GameManager.instance.enemyLevels.RemoveAt(GameManager.instance.enemyLevels.Count - 1);

                //Add a new enemy with the same leves as the second biggest monster+1. This gives a good progression.
                var highEnemy = GameManager.instance.enemyLevels[1] + 1;
                GameManager.instance.enemyLevels.Insert(0, highEnemy);
            }
            Debug.Log("New enemylist" + GameManager.instance.enemyLevels.Count);

            var enemyLevels = GameManager.instance.enemyLevels;
            //Get all enemies with level lower than current dungeon Level
            var enemiesToChooseFrom = DAL.ObjectDAL.GetEnemies(level, Enemy.MonsterType.Basic);
            // List<Enemy> enemyList = new List<Enemy>();

            foreach (int enemyLevel in enemyLevels)
            {
                Enemy enemyChoice = enemiesToChooseFrom[Random.Range(0, enemiesToChooseFrom.Count)];

                var instance = Instantiate(enemyPrefab);

                instance.transform.SetParent(mapGen.transform);

                var script = instance.GetComponent<EnemyManager>();

                script.PopulateEnemy(enemyChoice, enemyLevel);

                dungeonObjects.Add(instance);
            }
        }


        private void GetChests(int level)
        {
            //Determine number of chests based on current level number, based on a logarithmic progression
            int chestCount = (int)Mathf.Log(level, 2f) + 1;

            for (int i = 0; i < chestCount; i++)
            {
                var instance = Instantiate(chestPrefab);
                instance.transform.SetParent(mapGen.transform);

                var script = instance.GetComponent<ChestManager>();

                script.PopulateChest(level);

                dungeonObjects.Add(instance);
            }
        }


        private void GetShrines(int level)
        {
            //Determine number of shrines
            int shrineCount = 1;

            for (int i = 0; i < shrineCount; i++)
            {
                var instance = Instantiate(shrinePrefab);
                instance.transform.SetParent(mapGen.transform);

                var script = instance.GetComponent<ShrineManager>();

                script.PopulateShrine(level);

                dungeonObjects.Add(instance);
            }
        }

        private void GetAnvils(int level)
        {
            int anvilCount = 0;

            //Determine number of Anvils         
            if (level % 2 == 0)
            {
                anvilCount = 1;
            }

            for (int i = 0; i < anvilCount; i++)
            {
                var instance = Instantiate(anvilPrefab);
                instance.transform.SetParent(mapGen.transform);

                dungeonObjects.Add(instance);
            }
        }
    }
}

