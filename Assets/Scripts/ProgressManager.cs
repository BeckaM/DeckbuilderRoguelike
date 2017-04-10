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

        public enum Metric { MonsterKills, DamageDealt, Healing, GoldEarned, HighestPlayerLevel, HighestDungeonLevel, ChestsOpened, ShrinesOpened, CardsPlayed };


        public void MonsterKill()
        {
            currentRunProgress.monsterKills++;
        }

        public void CardPlayed()
        {
            currentRunProgress.cardsPlayed++;
        }

        public void DamageDealt(int damage)
        {
            currentRunProgress.damageDealt += damage;
        }

        public void Healing(int heal)
        {
            currentRunProgress.healing += heal;
        }

        public void GoldEarned(int gold)
        {
            currentRunProgress.goldEarned += gold;
        }

        public void HighestPlayerLevel(int level)
        {
            currentRunProgress.highestPlayerLevel = level;
        }

        public void HighestDungeonLevel(int level)
        {
            currentRunProgress.highestDungeonLevel = level;
        }

        public void FoundShrine()
        {
            currentRunProgress.shrinesOpened++;
        }

        public void FoundChest()
        {
            currentRunProgress.chestsOpened++;
        }

        public void EndRun()
        {            
            totalProgress.monsterKills += currentRunProgress.monsterKills;
            currentClass.monsterKills += currentRunProgress.monsterKills;

            totalProgress.healing += currentRunProgress.healing;
            currentClass.healing += currentRunProgress.healing;

            totalProgress.damageDealt += currentRunProgress.damageDealt;
            currentClass.damageDealt += currentRunProgress.damageDealt;

            totalProgress.cardsPlayed += currentRunProgress.cardsPlayed;
            currentClass.cardsPlayed += currentRunProgress.cardsPlayed;

            totalProgress.goldEarned += currentRunProgress.goldEarned;
            currentClass.goldEarned += currentRunProgress.goldEarned;

            totalProgress.shrinesOpened += currentRunProgress.shrinesOpened;
            currentClass.shrinesOpened += currentRunProgress.shrinesOpened;

            totalProgress.chestsOpened += currentRunProgress.chestsOpened;
            currentClass.chestsOpened += currentRunProgress.chestsOpened;

            if(currentRunProgress.highestDungeonLevel > totalProgress.highestDungeonLevel)
            {
                totalProgress.highestDungeonLevel = currentRunProgress.highestDungeonLevel;
            }

            if(currentRunProgress.highestDungeonLevel > currentClass.highestDungeonLevel)
            {
                currentClass.highestDungeonLevel = currentRunProgress.highestDungeonLevel;
            }

            if (currentRunProgress.highestPlayerLevel > totalProgress.highestPlayerLevel)
            {
                totalProgress.highestPlayerLevel = currentRunProgress.highestPlayerLevel;
            }

            if (currentRunProgress.highestPlayerLevel > currentClass.highestPlayerLevel)
            {
                currentClass.highestPlayerLevel = currentRunProgress.highestPlayerLevel;
            }
                       
            SaveProgress();
        }


        public List<PlayerClass> GetNewClassUnlocks()
        {
            List<PlayerClass> newClassUnlocks = new List<PlayerClass>();

            foreach(PlayerClass pClass in lockedClasses)
            {

            }

            return newClassUnlocks;
        }

        public List<Perk> GetNewPerkUnlocks()
        {
            List<Perk> newPerkUnlocks = new List<Perk>();

            foreach (PlayerClass pClass in lockedClasses)
            {

            }

            return newPerkUnlocks;
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
            currentClass = totalProgress.classProgressList.Find(item => item.className.Equals(selectedClass.ClassName));
        }


    }
}

