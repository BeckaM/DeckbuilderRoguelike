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

        public PlayerProgress currentRunProgress= new PlayerProgress();

        public List<PlayerClass> unlockedClasses = new List<PlayerClass>();
        public List<PlayerClass> lockedClasses = new List<PlayerClass>();

        public List<Item> unlockedPerks = new List<Item>();

        public List<Item> lockedPerks = new List<Item>();

        public enum Metric { Monsters_Killed, Damage_Dealt, Healing_Done, Gold_Earned, Highest_Player_Level, Highest_Dungeon_Level, Chests_Opened, Shrines_Opened, Cards_Played };

        public void CumulativeMetric(Metric metric, int value)
        {
            if (!currentRunProgress.cumulativeMetrics.ContainsKey(metric))
            {
                currentRunProgress.cumulativeMetrics.Add(metric, value);
            }
            else
            {
                currentRunProgress.cumulativeMetrics[metric] += value;
            }
        }

        public void HighestAchievedMetric(Metric metric, int value)
        {
            if (!currentRunProgress.highestAchievedMetrics.ContainsKey(metric))
            {
                currentRunProgress.highestAchievedMetrics.Add(metric, value);
            }
            else
            {
                if (value > currentRunProgress.highestAchievedMetrics[metric])
                    currentRunProgress.highestAchievedMetrics[metric] = value;
            }
        }


        public void EndRun()
        {
            foreach (Metric metric in currentRunProgress.cumulativeMetrics.Keys)
            {
                if (!totalProgress.cumulativeMetrics.ContainsKey(metric))
                {
                    totalProgress.cumulativeMetrics.Add(metric, currentRunProgress.cumulativeMetrics[metric]);
                }
                else
                {
                    totalProgress.cumulativeMetrics[metric] += currentRunProgress.cumulativeMetrics[metric];
                }

                if (!currentClass.cumulativeMetrics.ContainsKey(metric))
                {
                    currentClass.cumulativeMetrics.Add(metric, currentRunProgress.cumulativeMetrics[metric]);
                }
                else
                {
                    currentClass.cumulativeMetrics[metric] += currentRunProgress.cumulativeMetrics[metric];
                }
                               
            }

            foreach (Metric metric in currentRunProgress.highestAchievedMetrics.Keys)
            {
                if (!totalProgress.highestAchievedMetrics.ContainsKey(metric))
                {
                    totalProgress.highestAchievedMetrics.Add(metric, currentRunProgress.highestAchievedMetrics[metric]);                                       
                }
                else if(totalProgress.highestAchievedMetrics[metric] < currentRunProgress.highestAchievedMetrics[metric])
                {
                    totalProgress.highestAchievedMetrics[metric] = currentRunProgress.highestAchievedMetrics[metric];
                }

                if (!currentClass.highestAchievedMetrics.ContainsKey(metric))
                {
                    currentClass.highestAchievedMetrics.Add(metric, currentRunProgress.highestAchievedMetrics[metric]);
                }
                else if (totalProgress.highestAchievedMetrics[metric] < currentRunProgress.highestAchievedMetrics[metric])
                {
                    currentClass.highestAchievedMetrics[metric] = currentRunProgress.highestAchievedMetrics[metric];
                }
            }

           // SaveProgress();
        }


        public List<PlayerClass> GetNewClassUnlocks()
        {
            List<PlayerClass> newClassUnlocks = new List<PlayerClass>();

            foreach (PlayerClass pClass in lockedClasses)
            {
                if (CheckUnlockConditions(pClass))
                {
                    newClassUnlocks.Add(pClass);
                }
            }

            foreach (PlayerClass pClass in newClassUnlocks)
            {
                var unlockedClass = new ClassProgress();
                unlockedClass.className = pClass.className;
                totalProgress.classProgressList.Add(unlockedClass);                
            }

            return newClassUnlocks;
        }

       
        private bool CheckUnlockConditions(PlayerClass unlock)
        {
            if (totalProgress.cumulativeMetrics.ContainsKey(unlock.condition))
            {
                return unlock.conditionValue <= totalProgress.cumulativeMetrics[unlock.condition];
            }
            else if (totalProgress.highestAchievedMetrics.ContainsKey(unlock.condition))
            {
                return unlock.conditionValue <= totalProgress.highestAchievedMetrics[unlock.condition];
            }
            return false;
        }


        public List<Item> GetNewPerkUnlocks()
        {
            List<Item> newPerkUnlocks = new List<Item>();

            foreach (Item perk in lockedPerks)
            {                
                if (CheckUnlockConditions(perk))
                {
                    newPerkUnlocks.Add(perk);
                }
            }

            foreach (Item perk in newPerkUnlocks)
            {
                totalProgress.perkProgressList.Add(perk.itemName);
            }


            return newPerkUnlocks;
        }

        private bool CheckUnlockConditions(Item unlock)
        {
            if (totalProgress.cumulativeMetrics.ContainsKey(unlock.condition))
            {
                return unlock.conditionValue <= totalProgress.cumulativeMetrics[unlock.condition];
            }
            else if (totalProgress.highestAchievedMetrics.ContainsKey(unlock.condition))
            {
                return unlock.conditionValue <= totalProgress.highestAchievedMetrics[unlock.condition];
            }
            return false;
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

            foreach (PlayerClass playerClass in allClasses.playerClasses)
            {
                if (CheckClassUnlock(playerClass.className))
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
            var allPerks = DAL.ObjectDAL.GetAllItems();

            foreach (Item perk in allPerks.itemList)
            {
                if (CheckPerkUnlock(perk.itemName))
                {
                    unlockedPerks.Add(perk);
                }
                else
                {
                    lockedPerks.Add(perk);
                }
            }
        }


        public bool CheckClassUnlock(PlayerClass.ClassName className)
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
            currentClass = totalProgress.classProgressList.Find(item => item.className.Equals(selectedClass.className));
        }


    }
}

