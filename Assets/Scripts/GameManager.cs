using UnityEngine;
using UnityEngine.UI;					//Allows us to use UI.
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections.Generic;

namespace Assets.Scripts
{

    public class GameManager : MonoBehaviour
    {
        public PlayerClass playerClass;
        public List<Sprite> classImages;

        public ProgressManager progressManager = new ProgressManager();
        
        public int gold { get; private set; }
                
        public int playerLevel = 0;
        public int playerXP = 0;
        public int nextLVLXP = 20;

        public int maxLife = 30;
        public int lifeHolder = 30;

        public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
        public float turnDelay = 0.2f;                          //Delay between each Player turn.

        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

        //        public bool playersTurn = true;     //Boolean to check if it's players turn, hidden in inspector but public.

       

        public GameObject monsterDeck;

        private GameObject cardGameCanvas;
        private GameObject dungeonCanvas;
        public DungeonUI dungeonUI
        {
            get
            {
                return dungeonCanvas.GetComponent<DungeonUI>();
            }
        }

        private DungeonManager boardScript;                     //Store a reference to our BoardManager which will set up the level.

        public ModalPanel modalPanel
        {
            get
            {
                return dungeonUI.modalPanelObject.GetComponent<ModalPanel>();
            }
        }

        public DeckPanel deckPanel
        {
            get
            {
                return dungeonUI.deckPanelObject.GetComponent<DeckPanel>();
            }
        }
        public enum Content { Gold, Consumable, Card };
        public Content lootType;
        public Card cardLoot;
        public int goldLoot;


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

            modalPanel.gameObject.SetActive(false);
            deckPanel.gameObject.SetActive(false);
            cardGameCanvas.SetActive(false);

            dungeonUI.UpdateLifeText();
            dungeonUI.UpdateGoldText();
            dungeonUI.UpdateXPText();

            //Set levelImage to active blocking player's view of the game board during setup.
            dungeonUI.levelText.text = "Level " + level;
            dungeonUI.levelImage.SetActive(true);

            DeckManager.player.InitDeck();

            //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
            Invoke("HideLevelImage", levelStartDelay);

            //Call the SetupScene function of the BoardManager script, pass it current level number.
            boardScript.SetupScene(level);
        }


        private void FindLevelObjects()
        {
            dungeonCanvas = GameObject.Find("Canvas(Board)");
            cardGameCanvas = GameObject.Find("Canvas(CardGame)");
        }


        public void InitCardgame(Collider monster, Player player)
        {

            doingSetup = true;
            cardGameCanvas.SetActive(true);
            //Create the monster deck and instantiate the cards.
            var enemyManager = monster.gameObject.GetComponent<EnemyManager>();

            enemyManager.InitMonsterDeck();

            //Send in the Player and Monster to the card game.
            CardgameManager.instance.enemy = enemyManager;
            CardgameManager.instance.player = player;

            //Remove the monster from game view. Either it dies or the player does.
            monster.gameObject.SetActive(false);
            dungeonUI.deckPanelObject.SetActive(true);

            //Prevent player from moving while in card game.
            doingSetup = true;

            //Enable the card game Canvas, which also starts the CardgameManager script.          
            cardGameCanvas.SetActive(true);
            CardgameManager.instance.Setup();
        }


        public void ReturnFromCardgame(bool win, Card cardReward, int goldReward)
        {

            dungeonUI.deckPanelObject.SetActive(false);
            dungeonUI.UpdateLifeText();

            if (win == false)
            {
                GameOver();
            }
            else
            {
                cardLoot = cardReward;
                goldLoot = goldReward;
                var rand = UnityEngine.Random.Range(0, 10);
                if (rand < 5)
                {
                    modalPanel.Chest("Victory!", "The monster drops a card. Add to your deck?", cardReward, AddLoot, DeclineLoot);
                    lootType = Content.Card;
                }
                else
                {
                    modalPanel.Chest("Victory!", "The monster drops some gold.", goldReward, AddLoot);
                    lootType = Content.Gold;
                }

                progressManager.MonsterKill();
                               
            }
        }


        private void AddLoot()
        {
            if (lootType == Content.Gold)
            {
                ModifyGold(goldLoot);
            }
            else
            {
                DeckManager.player.AddCardtoDeck(cardLoot.cardName);
            }

            doingSetup = false;
        }


        private void DeclineLoot()
        {
            doingSetup = false;
        }


        //Hides black image used between levels
        void HideLevelImage()
        {
            //Disable the levelImage gameObject.
            dungeonUI.levelImage.SetActive(false);

            //Set doingSetup to false allowing player to move again.
            doingSetup = false;
        }


        //GameOver is called when the player reaches 0 life points
        public void GameOver()
        {
            progressManager.EndRun();
           
            dungeonUI.gameOverScript.UpdateGameOverText(level, playerLevel, progressManager.currentRunProgress);
            dungeonUI.gameOverPanel.SetActive(true);
           
        }

        public void GainXP(int XP)
        {
            playerXP = playerXP + XP;

            //Check if player leveled up
            if (playerXP >= nextLVLXP)
            {
                dungeonUI.LevelUpButton.SetActive(true);
            }
            dungeonUI.UpdateXPText();
        }


        internal void LevelUp()
        {
            List<Card> rewardList = new List<Card>();
            for (var i = 0; i < 3; i++)
            {
                var newCard = DAL.ObjectDAL.GetRandomClassCard(level - 1, level + 1);
                rewardList.Add(newCard);
            }

           modalPanel.LevelUp(rewardList, LevelUpComplete);
        }


        internal void LevelUpComplete()
        {
            AddLoot();
            playerLevel++;
            var increase = 1.4* nextLVLXP;
            nextLVLXP = nextLVLXP + (int)increase;
            dungeonUI.UpdateXPText();
            if(playerXP < nextLVLXP)
            {
                dungeonUI.LevelUpButton.SetActive(false);
            }
        }


        public void ModifyGold(int gain)
        {
            gold = gold + gain;
            dungeonUI.UpdateGoldText();
        }

      
    }

}
