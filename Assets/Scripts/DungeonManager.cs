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
        //public GameObject enemyPrefab;                                //Enemy prefab.
        //public GameObject[] outerWallTiles;                             //Array of outer tile prefabs.

        private Transform DungeonCanvas;
        private MapGenerator mapGen;                                  //A variable to store a reference to the transform of our Board object.
        private List<Vector3> gridPositions = new List<Vector3>();  //A list of possible locations to place tiles.


        //SetupScene initializes our level and calls the previous functions to lay out the game board
        public void SetupScene(int level)
        {
            var m = GameObject.Find("Map Generator");
            mapGen = m.GetComponent<MapGenerator>();

            mapGen.GenerateMap();

            mapGen.PlacePlayerAndExit();

            PlaceEnemies(level);

            
        }
        private void PlaceEnemies(int Level)
        {

            //Determine number of enemies based on current level number, based on a logarithmic progression
            int enemyCount = (int)Mathf.Log(Level, 2f)+2;

            //Get all enemies with level lower than current dungeon Level
            var enemiesToChooseFrom = DAL.ObjectDAL.GetEnemies(Level);
            List<Enemy> enemyList = new List<Enemy>();

            for (int i = 0; i < enemyCount; i++)
            {
                Enemy enemyChoice = enemiesToChooseFrom[Random.Range(0, enemiesToChooseFrom.Count)];
                enemyList.Add(enemyChoice);

            }

            //Instantiate enemies until the chosen limit enemyCount is reached
            foreach (var enemy in enemyList)
            {
                ////Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
                //Vector3 randomPosition = RandomPosition();

                ////Choose a random tile from tileArray and assign it to tileChoice
                ////GameObject tileChoice = enemylist[Random.Range(0, enemylist.Count)];

                ////Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
                //var instance = Instantiate(enemyPrefab, randomPosition, Quaternion.identity, boardHolder);
                //var script = instance.GetComponent<EnemyManager>();

                //script.PopulateEnemy(enemy, Level);


            }


        }

    }

}