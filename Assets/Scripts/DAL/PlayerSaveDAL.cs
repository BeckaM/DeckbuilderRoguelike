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

        public static void LoadPlayer(int Player)
        {

        }

        internal static void CreateNewPlayer(int player, string playerName)
        {
            PlayerProgress progress = new PlayerProgress();
            progress.playerName = playerName;

            var saker = File.Create(playerSave + player);
            saker.Dispose();
            SaveProgress(progress, player);
        }

        internal static void SaveProgress(PlayerProgress progress, int player)
        {
            if (!File.Exists(playerSave + player))
            {
                return;
            }
            string jsonProgress = JsonUtility.ToJson(progress);

            File.WriteAllText(playerSave + player, jsonProgress);
        }

        internal static string GetMoon(int playerNr)
        {
            string text = File.ReadAllText(playerSave + playerNr);
             var moon = JsonUtility.FromJson<PlayerProgress>(text);

            return moon.playerName; // fix bug caused by previous bug. More bugs fixed. Dont panic! We got this.
        }
    }
}
