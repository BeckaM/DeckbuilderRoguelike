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
        public ClassSelect classSelect;
        public GameObject perkSelect;

        public void ShowPlayerSelect()
        {
            splashScreen.SetActive(false);
            playerSelect.SetActive(true);
        }


        public void ShowStartScreen()
        {
            classSelect.HideClassSelect();
            playerSelect.SetActive(false);
            startScreen.SetActive(true);
        }

        public void ShowProgressScreen()
        {
            startScreen.SetActive(false);
            classSelect.ShowProgress();
        }

        public void ShowClassSelect()
        {
            startScreen.SetActive(false);
            classSelect.ShowClassSelect();
        }


        internal void ShowPerkSelect()
        {
            classSelect.HideClassSelect();
            perkSelect.SetActive(true);
        }


        public void StartGame()
        {
            GameManager.instance.progressManager.currentRunProgress = new PlayerProgress();
            //Initialize the starting deck and create the cards.
            
            SceneManager.LoadScene("Main");
        }


    }
}