using UnityEngine;
using System.Collections;


namespace Assets.Scripts
{	
	public class Loader : MonoBehaviour 
	{
		public GameObject gameManager;			//GameManager prefab to instantiate.
		public GameObject soundManager;         //SoundManager prefab to instantiate.
        public GameObject deckManager;
		
		void Awake ()
		{
			//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
			if (GameManager.instance == null)
				
				//Instantiate gameManager prefab
				Instantiate(gameManager);
			
			//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
			if (SoundManager.instance == null)
				
				//Instantiate SoundManager prefab
				Instantiate(soundManager);

            //Check if a DeckManager has already been assigned to static variable GameManager.instance or if it's still null
            if (DeckManager.instance == null)

                //Instantiate DeckManager prefab
                Instantiate(deckManager);
        }
	}
}