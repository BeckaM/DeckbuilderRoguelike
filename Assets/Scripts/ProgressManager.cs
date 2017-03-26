using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.
using System.IO;


namespace Assets.Scripts
{
    public class ProgressManager
    {
        public PlayerProgress currentProgress;

        public void MonsterKill()
        {
            currentProgress.monsterKills++;
        }


        public void SaveProgress()
        {
            DAL.PlayerSaveDAL.SaveProgress(currentProgress);
        }

    }
}

