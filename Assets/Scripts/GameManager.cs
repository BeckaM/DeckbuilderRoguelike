using UnityEngine;
using System.Collections;
using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;					//Allows us to use UI.
using UnityEngine.SceneManagement;


namespace Assets.Scripts
{

    public class GameManager : MonoBehaviour
    {
        public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
        public float turnDelay = 0.2f;							//Delay between each Player turn.
        public int playerLife = 500;
        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
       
        public bool playersTurn = true;     //Boolean to check if it's players turn, hidden in inspector but public.


        private Text levelText;                                 //Text to display current level number.
        private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.                 
        private GameObject CardGameCanvas;
        private DungeonManager boardScript;						//Store a reference to our BoardManager which will set up the level.
       
        private int level = 3;                                  //Current level number, expressed in game as "Day 1".

        private bool notplayersturn;
        private bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.


        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);


            //Get a component reference to the attached BoardManager script
            boardScript = GetComponent<DungeonManager>();

            
            
            //Call the InitGame function to initialize the first level 
           // InitGame();
            
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }
        void OnDisable()
        {
          
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }


        //This is called each time a scene is loaded.
        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            //Add one to our level number.
            level++;
            //Call InitGame to initialize our level.
            InitGame();
        }

        //Initializes the game for each level.
        void InitGame()
        {
            //While doingSetup is true the player can't move, prevent player from moving while title card is up.
            doingSetup = true;

            //DungeonCanvas = GameObject.Find("Canvas(Board)");
            CardGameCanvas = GameObject.Find("Canvas(CardGame)");

            //DungeonCanvas.SetActive(true);
            CardGameCanvas.SetActive(false);



            //Get a reference to our image LevelImage by finding it by name.
            levelImage = GameObject.Find("LevelImage");



            //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
            levelText = GameObject.Find("LevelText").GetComponent<Text>();

            //Set the text of levelText to the string "Level" and append the current level number.
            levelText.text = "Level " + level;

            //Set levelImage to active blocking player's view of the game board during setup.
            levelImage.SetActive(true);

            //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
            Invoke("HideLevelImage", levelStartDelay);

            //Call the Starting Deck function to initialize the starting deck
            
            DeckManager.instance.StartingDeck();

            //Call the SetupScene function of the BoardManager script, pass it current level number.
            boardScript.SetupScene(level);

        }

        public void InitCardgame(Collider2D monster)
        {
            var enemyManager = monster.gameObject.GetComponent<EnemyManager>();
            enemyManager.InitMonsterDeck();

            CardgameManager.instance.enemy = enemyManager.enemy;

            monster.gameObject.SetActive(false);

            //While doingSetup is true the player can't move, prevent player from moving while card game.

            doingSetup = true;

            //         DungeonBoard = GameObject.Find("Board");

            //          DungeonBoard.SetActive(false);
            //DungeonCanvas.SetActive(false);
            CardGameCanvas.SetActive(true);
                        
        }


        //Hides black image used between levels
        void HideLevelImage()
        {
            //Disable the levelImage gameObject.
            levelImage.SetActive(false);

            //Set doingSetup to false allowing player to move again.
            doingSetup = false;
        }

        //Update is called every frame.
        void Update()
        {
            ////Check that playersTurn or enemiesMoving or doingSetup are not currently true.
            if (playersTurn || doingSetup || notplayersturn)

                //    //If any of these are true, return and do not start MoveEnemies.
                return;

            //Start moving enemies.
            StartCoroutine(MoveEnemies());

        }



        //GameOver is called when the player reaches 0 food points
        public void GameOver()
        {
            //Set levelText to display number of levels passed and game over message
            levelText.text = "After " + level + " days, you starved.";

            //Enable black background image gameObject.
            levelImage.SetActive(true);

            //Disable this GameManager.
            enabled = false;
        }


        //Coroutine to move enemies in sequence.
        IEnumerator MoveEnemies()
        {

            notplayersturn = true;

            yield return new WaitForSeconds(turnDelay);




            playersTurn = true;
            notplayersturn = false;


        }



    }

}
