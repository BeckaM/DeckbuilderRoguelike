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

        public List<PlayerClass> unlockedClasses = new List<PlayerClass>();
        public List<PlayerClass> lockedClasses = new List<PlayerClass>();

        public List<Perk> unlockedPerks = new List<Perk>();
        public List<Perk> lockedPerks = new List<Perk>();

        private int monsterKills;
        private int cardsPlayed;
        private int damageDealt;
        private int healing;
        private int goldEarned;
        private int highestPlayerLevel;
        private int highestDungeonLevel;
        private int chestsOpened;
        private int shrinesOpened; 

        public enum Metric { MonsterKills, DamageDealt, Healing, GoldEarned, HighestPlayerLevel, HighestDungeonLevel, ChestsOpened, ShrinesOpened, CardsPlayed };


        public void MonsterKill()
        {
            monsterKills++;
        }

        public void CardPlayed()
        {
            cardsPlayed++;
        }

        public void DamageDealt(int damage)
        {
            damageDealt+= damage;
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
            foreach(PlayerClass pClass in lockedClasses)
            {

            } 
        }


        public void SaveProgress()
        {
            DAL.PlayerSaveDAL.SaveProgress(totalProgress);
        }


        public void LoadUnlockables()
        {
            LoadClassUnlockables();
            LoadPerkUnlockables();
        }


        public void LoadClassUnlockables()
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


        public void LoadPerkUnlockables()
        {
            var allPerks = DAL.ObjectDAL.GetAllPerks();

            foreach (Perk perk in allPerks.perkList)
            {
                if (CheckPerkUnlock(perk.perkName))
                {
                    unlockedPerks.Add(perk);
                }
                else
                {
                    lockedPerks.Add(perk);
                }
            }
        }


        public bool CheckClassUnlock(string className)
        {
            var classcheck = totalProgress.classProgressList.Exists(item => item.className.Equals(className));

            return (classcheck);
        }

        public bool CheckPerkUnlock(string perkName)
        {
            var perkcheck = totalProgress.perkProgressList.Exists(item => item.Equals(perkName));

            return (perkcheck);
        }


        internal void SetProgressClass(PlayerClass selectedClass)
        {
            currentClass = totalProgress.classProgressList.Find(item => item.className.Equals(selectedClass));
        }


    }
}

