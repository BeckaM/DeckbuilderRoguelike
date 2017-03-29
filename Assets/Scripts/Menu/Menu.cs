using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Menu
{

    public class Menu : MonoBehaviour
    {
        public GameObject splashScreen;
        public GameObject playerSelect;
        public GameObject startScreen;
        public GameObject classSelect;
        public GameObject perkSelect;

        public void HideSplashScreen()
        {
            splashScreen.SetActive(false);
            playerSelect.SetActive(true);
        }
             
        
       public void HidePlayerSelect()
        {
            playerSelect.SetActive(false);
            startScreen.SetActive(true);
        }

        public void HideStart()
        {
            startScreen.SetActive(false);
            classSelect.SetActive(true);
        }


        internal void HideClassSelect()
        {
            classSelect.SetActive(false);
            perkSelect.SetActive(true);
        }


        public void StartGame()
        {
            List<string> classlist = new List<string>()
            {
                "Iron Soul",
            };

            var tempClass = DAL.ObjectDAL.GetClasses(classlist);
            var classchoice = tempClass[0];
            GameManager.instance.playerClass = classchoice;
            GameManager.instance.progressManager.currentRunProgress = new PlayerProgress();

            //Initialize the starting deck and create the cards.
            DeckManager.player.StartingDeck(classchoice.Startingdeck);

            SceneManager.LoadScene("Scene 3D");
        }

      
    }
}