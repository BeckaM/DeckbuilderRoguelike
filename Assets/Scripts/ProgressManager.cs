using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.
using System.IO;


namespace Assets.Scripts
{
    public class ProgressManager
    {
        public PlayerProgress totalProgress;
        public ClassProgress currentClass;

        public PlayerProgress currentRunProgress;

        public List<PlayerClass> unlockedClasses;
        public List<PlayerClass> lockedClasses;

        public List<Perk> unlockedPerks;
        public List<Perk> lockedPerks;

        public int monsterKills;

        public enum Metric { MonsterKills, DamageDealt, Healing, GoldEarned, HighestPlayerLevel, HighestDungeonLevel, ChestsOpened, ShrinesOpened, CardsPlayed };


        public void MonsterKill()
        {
            monsterKills++;
        }


        public void EndRun()
        {
            totalProgress.monsterKills += currentRunProgress.monsterKills;
            currentClass.monsterKills += currentRunProgress.monsterKills;

            CheckNewUnlocks();
            SaveProgress();

        }

        private void CheckNewUnlocks()
        {
            throw new NotImplementedException();
        }

        public void SaveProgress()
        {
            DAL.PlayerSaveDAL.SaveProgress(totalProgress);
        }


        public void LoadUnlockables()
        {
            var allClasses = DAL.ObjectDAL.GetAllClasses();

            foreach (PlayerClass playerClass in allClasses.PlayerClasses)
            {
                if (CheckClassUnlock(playerClass.ClassName))
                {
                    unlockedClasses.Add(playerClass);
                }
                else
                {
                    lockedClasses.Add(playerClass);
                }
            }

        }


        public bool CheckClassUnlock(string className)
        {
            var classcheck = totalProgress.classProgressList.Exists(item => item.className.Equals(className));

            if (classcheck)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        internal void SetProgressClass(PlayerClass selectedClass)
        {
            currentClass = totalProgress.classProgressList.Find(item => item.className.Equals(selectedClass));
        }



    }
}

