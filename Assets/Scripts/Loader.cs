﻿using UnityEngine;
using System.Collections;


namespace Assets.Scripts
{	
	public class Loader : MonoBehaviour 
	{
		public GameObject gameManager;			//GameManager prefab to instantiate.
		public GameObject soundManager;         //SoundManager prefab to instantiate.    
        

        void Awake ()
		{
            Application.targetFrameRate = 60;           
            //Check if a GameManager has already been assigned to static variable instance or if it's still null
            if (GameManager.instance == null)
				
				//Instantiate gameManager prefab
				Instantiate(gameManager);
			
			//Check if a SoundManager has already been assigned to static variable instance or if it's still null
			if (SoundManager.instance == null)
				
				//Instantiate SoundManager prefab
				Instantiate(soundManager);                     
                



        }
	}
}