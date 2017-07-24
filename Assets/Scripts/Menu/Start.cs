using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Assets.Scripts.Menu
{

    public class Start : MonoBehaviour
    {        
        public Menu mainMenu;
        public GameObject progressScreen;
        public GameObject camera;
       

        public void ShowProgressScreen(bool show)
        {
            if (show)
            {
                progressScreen.SetActive(true);
            }
            else
            {
                progressScreen.SetActive(false);
            }

        }

    }
}
