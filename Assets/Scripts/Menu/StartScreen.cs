using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Assets.Scripts.Menu
{

    public class StartScreen : MonoBehaviour
    {        
        public void StartGame()
        {
            List<string> classlist = new List<string>()
            {
                "Iron Soul",
            };

            var tempClass = DAL.ObjectDAL.GetClasses(classlist);
            var classchoice = tempClass[0];
            GameManager.instance.playerClass = classchoice;

            //Initialize the starting deck and create the cards.
            DeckManager.player.StartingDeck(classchoice.Startingdeck);

            SceneManager.LoadScene("Scene 3D");
        }
    }
}