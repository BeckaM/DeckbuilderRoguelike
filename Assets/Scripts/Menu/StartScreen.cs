using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Assets.Scripts.Menu
{
    
    public class StartScreen : MonoBehaviour
    {
        
                
        // continue to play, by ensuring the preference is set correctly, the overlay is not active, 
        // and that the audio listener is enabled, and time scale is 1 (normal)
        public void StartGame()
        {

           

            List<string> classlist = new List<string>()
        {
            "Iron Soul",
           
        };

            var tempClass = DAL.ObjectDAL.GetClasses(classlist);
            var classchoice = tempClass[0];
            GameManager.instance.playerClass = classchoice;
            

            SceneManager.LoadScene("Scene 3D");
            
        }

    }
}