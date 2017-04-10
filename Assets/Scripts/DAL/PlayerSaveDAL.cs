using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public static class PlayerSaveDAL
    {
        private const string playerSave = @".\Assets\JSON\player";

        public static bool PlayerExists(int player)
        {
            return File.Exists(playerSave + player);
        }

        public static void LoadPlayer(int player)
        {
            string text = File.ReadAllText(playerSave + player);
            var playerProgress = JsonUtility.FromJson<PlayerProgress>(text);

            GameManager.instance.progressManager.totalProgress = playerProgress;
            GameManager.instance.progressManager.LoadUnlockables();
        }

        internal static void CreateNewPlayer(int player, string playerName)
        {
            PlayerProgress progress = new PlayerProgress();
            progress.playerName = playerName;
            progress.player = player;

            var startclass = new ClassProgress();
            startclass.className = "Iron Soul";
            progress.classProgressList.Add(startclass);
            
            var saker = File.Create(playerSave + player);
            saker.Dispose();
            SaveProgress(progress);
        }

        internal static void SaveProgress(PlayerProgress progress)
        {
            if (File.Exists(playerSave + progress.player))
            {
                var jsonProgress = JsonUtility.ToJson(progress);
                File.WriteAllText(playerSave + progress.player, jsonProgress);
            }
        }

        //WARNING: Code review was done by rebecka+champagne.

        internal static string GetPlayerName(int playerNr)
        {
            string text = File.ReadAllText(playerSave + playerNr);
            var moon = JsonUtility.FromJson<PlayerProgress>(text);

            return moon.playerName;
        }
    }
}
