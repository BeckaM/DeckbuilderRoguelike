using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts.Menu
{

    // Hi! This script presents the overlay info for our tutorial content, linking you back to the relevant page.
    public class StartScreen : MonoBehaviour
    {

        // allow user to choose whether to show this menu 
        //	public bool showAtStart = true;



        // store the GameObject which renders the overlay info
        public GameObject overlay;

        // store a reference to the audio listener in the scene, allowing for muting of the scene during the overlay
        public AudioListener mainListener;


        // used to ensure that the launch screen isn't more than once per play session if the project reloads the main scene
        private static bool alreadyShownThisSession = false;


        void Awake()
        {
            // have we already shown this once?
            if (alreadyShownThisSession)
            {
                StartGame();
            }
            else
            {
                alreadyShownThisSession = true;

            }
        }

        // show overlay info, pausing game time, disabling the audio listener 
        // and enabling the overlay info parent game object
        public void ShowLaunchScreen()
        {
            Time.timeScale = 0f;
            mainListener.enabled = false;
            overlay.SetActive(true);
        }


        // continue to play, by ensuring the preference is set correctly, the overlay is not active, 
        // and that the audio listener is enabled, and time scale is 1 (normal)
        public void StartGame()
        {
            overlay.SetActive(false);
            mainListener.enabled = true;
            Time.timeScale = 1f;
        }

    }
}