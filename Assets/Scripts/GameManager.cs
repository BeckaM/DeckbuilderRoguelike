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

        public int gold;

        public int playerLevel = 0;
        public int playerXP = 0;
        public int nextLVLXP = 20;

        public int maxLife = 30;
        public int lifeHolder = 30;

        public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.     

        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

        public PerkManager perkManager = new PerkManager();

        public GameObject monsterDeck;
        public List<int> enemyLevels;

        public CardgameManager cardGameManager;
        public DungeonUI dungeonUI;
        public DungeonManager dungeonManager;                    //Store a reference to our dungeon manager which will set up the level.
        private CardgameUI cardGameUI;
        public enum Content { Gold, Consumable, Card };
        public Content lootType;
        public Card cardLoot;
        public int goldLoot;

        private int level = 0;                                  //Current level number, expressed in game as "Level 1".

        public bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.

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
            if (scene.name == "Main")
            {
                //Add one to our level number.
                level++;
                progressManager.HighestAchievedMetric(ProgressManager.Metric.Highest_Dungeon_Level, level);
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

            dungeonUI.modalPanel.gameObject.SetActive(false);
            dungeonUI.deckPanel.gameObject.SetActive(false);
            cardGameManager.gameObject.SetActive(false);
            cardGameUI.gameObject.SetActive(false);

            LevelUpCheck();

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
            dungeonManager.SetupScene(level);
        }


        private void FindLevelObjects()
        {

            dungeonManager = GameObject.Find("Dungeon").GetComponent<DungeonManager>();
            dungeonUI = GameObject.Find("DungeonUI").GetComponent<DungeonUI>();
            cardGameManager = GameObject.Find("CardGame").GetComponent<CardgameManager>();
            cardGameUI = GameObject.Find("CardGameUI").GetComponent<CardgameUI>();
        }


        public void InitCardgame(Collider monster, Player player)
        {
            doingSetup = true;
            //Create the monster deck and instantiate the cards.
            var enemyManager = monster.gameObject.GetComponent<EnemyManager>();

            // enemyManager.InitMonsterDeck();

            //Send the Player and Monster to the card game.
            CardgameManager.instance.enemy = enemyManager;
            CardgameManager.instance.player = player;

            //Remove the monster from game view. Either it dies or the player does.
            monster.gameObject.SetActive(false);

            //Prevent player from moving while in card game.
            doingSetup = true;
            player.gameObject.SetActive(false);

            //Enable the card game Canvas, which also starts the CardgameManager script.        
            cardGameManager.gameObject.SetActive(true);
            DeckManager.player.InitCardGameDeck();

            enemyManager.InitMonsterDeck();
            DeckManager.monster.InitCardGameDeck();

            dungeonManager.gameObject.SetActive(false);
            dungeonUI.gameObject.SetActive(false);

            CardgameManager.instance.Setup();
        }


        public void ReturnFromCardgame(bool win, List<Card> cardRewards, int goldReward)
        {           
            dungeonManager.gameObject.SetActive(true);
            dungeonUI.gameObject.SetActive(true);
          //  dungeonUI.deckPanel.gameObject.SetActive(false);
            dungeonUI.UpdateLifeText();

            if (win == false)
            {
                GameOver();
            }
            else
            {
                dungeonUI.modalPanel.MonsterLoot(cardRewards, goldReward, AddLoot);
                progressManager.CumulativeMetric(ProgressManager.Metric.Monsters_Killed, 1);
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
                DeckManager.player.AddCardtoDeck(DeckManager.player.CreateCardObject(cardLoot));
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
            dungeonUI.gameOverPanel.UpdateGameOverText(level, playerLevel);
            progressManager.EndRun();
            dungeonUI.gameOverPanel.UpdateNewUnlocks(progressManager.GetNewClassUnlocks(), progressManager.GetNewPerkUnlocks());
            progressManager.SaveProgress();

            dungeonUI.gameOverPanel.GameOver();
        }


        public void BackToMenu()
        {
            Destroy(this);
            SceneManager.LoadScene("Start");
        }


        public void GainXP(int XP)
        {
            playerXP = playerXP + XP;
            LevelUpCheck();
            dungeonUI.UpdateXPText();
        }

        //Check if player leveled up
        internal void LevelUpCheck()
        {
            if (playerXP >= nextLVLXP)
            {
                dungeonUI.LevelUpButton.SetActive(true);
            }
        }


        public void GainLife(int gain)
        {
            lifeHolder += gain;
            if (lifeHolder > maxLife)
            {
                lifeHolder = maxLife;
            }
            dungeonUI.UpdateLifeText();
        }


        internal void LevelUp()
        {
            var newCards = DAL.ObjectDAL.GetClassCards(playerLevel - 1, playerLevel + 1, playerClass.className);

            List<Card> rewardList = new List<Card>();
            rewardList.AddRange(newCards);

            dungeonUI.modalPanel.LevelUp(rewardList, LevelUpComplete);
        }


        internal void LevelUpComplete()
        {
            AddLoot();
            playerLevel++;
            lifeHolder = maxLife;
            var increase = 1.4 * nextLVLXP;
            nextLVLXP = nextLVLXP + (int)increase;
            dungeonUI.UpdateXPText();
            if (playerXP < nextLVLXP)
            {
                dungeonUI.LevelUpButton.SetActive(false);
            }
            progressManager.HighestAchievedMetric(ProgressManager.Metric.Highest_Player_Level, playerLevel);
        }


        public void ModifyGold(int gain)
        {
            gold = gold + gain;
            dungeonUI.UpdateGoldText();
            if (gain > 0)
            {
                progressManager.CumulativeMetric(ProgressManager.Metric.Gold_Earned, gain);
            }
        }
    }
}
