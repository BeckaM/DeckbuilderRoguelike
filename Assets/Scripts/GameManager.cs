using UnityEngine;
using System.Collections;
using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;					//Allows us to use UI.
using UnityEngine.SceneManagement;


namespace Assets.Scripts
{

    public class GameManager : MonoBehaviour
    {


        public PlayerClass playerClass;

        public int maxLife = 30;
        public int lifeHolder = 30;
        public Text lifeTextBoard;                      //UI Text to display current player life total.


        public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
        public float turnDelay = 0.2f;                          //Delay between each Player turn.

        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

        public bool playersTurn = true;     //Boolean to check if it's players turn, hidden in inspector but public.

        private Text levelText;                                 //Text to display current level number.
        public GameObject monsterDeck;
        private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.                 
        private GameObject CardGameCanvas;
        private DungeonManager boardScript;						//Store a reference to our BoardManager which will set up the level.
        public ModalPanel panel;
        //public DeckManager myDeck;
        //public DeckManager AIDeck;


        private int level = 0;                                  //Current level number, expressed in game as "Level 1".

        internal bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.

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
            panel = ModalPanel.Instance();


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
            if (scene.name == "Scene 3D")
            {
                //Add one to our level number.
                level++;
                //Call InitGame to initialize our level.
                InitGame();
            }
        }

        //Initializes the game for each level.
        void InitGame()
        {

            //Prevent player from moving while title card is up.
            doingSetup = true;

            //Find all the scene objects we need.
            FindLevelObjects();

            //Hide the cardgame overlay.            
            CardGameCanvas.SetActive(false);

            //Set the text of levelText to the string "Level" and append the current level number.
            levelText.text = "Level " + level;

            //Set levelImage to active blocking player's view of the game board during setup.
            levelImage.SetActive(true);

            //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
            Invoke("HideLevelImage", levelStartDelay);

            //Initialize the starting deck and create the cards.
            DeckManager.player.StartingDeck(playerClass.Startingdeck);

            //Call the SetupScene function of the BoardManager script, pass it current level number.
            boardScript.SetupScene(level);

        }

        private void FindLevelObjects()
        {
            //DungeonCanvas = GameObject.Find("Canvas(Board)");
            CardGameCanvas = GameObject.Find("Canvas(CardGame)");


            //Get a reference to our image LevelImage by finding it by name.
            levelImage = GameObject.Find("LevelImage");

            //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
            levelText = GameObject.Find("LevelText").GetComponent<Text>();

            lifeTextBoard = GameObject.Find("LifeTextBoard").GetComponent<Text>();


        }

        public void InitCardgame(Collider monster, Player3d player)
        {

            doingSetup = true;
            CardGameCanvas.SetActive(true);
            //Create the monster deck and instantiate the cards.
            var enemyManager = monster.gameObject.GetComponent<EnemyManager>();

            enemyManager.InitMonsterDeck();

            //Send in the Player and Monster to the card game.
            CardgameManager.instance.enemy = enemyManager;
            CardgameManager.instance.player = player;

            //Remove the monster from game view. Either it dies or the player does.
            monster.gameObject.SetActive(false);
           // player.gameObject.SetActive(false);

            //Prevent player from moving while in card game.
            doingSetup = true;

            //Enable the card game Canvas, which also starts the CardgameManager script.          
            CardGameCanvas.SetActive(true);
            CardgameManager.instance.Setup();

        }


        public void ReturnFromCardgame(bool win)
        {
            if (win == false)
            {
                GameOver();
            }
            doingSetup = false;
        }

        //Hides black image used between levels
        void HideLevelImage()
        {
            //Disable the levelImage gameObject.
            levelImage.SetActive(false);

            //Set doingSetup to false allowing player to move again.
            doingSetup = false;
        }




        //GameOver is called when the player reaches 0 life points
        public void GameOver()
        {
            //Set levelText to display number of levels passed and game over message
            levelText.text = "You died on level " + level;

            //Enable black background image gameObject.
            levelImage.SetActive(true);

            //Disable this GameManager.
            enabled = false;
        }






    }

}
