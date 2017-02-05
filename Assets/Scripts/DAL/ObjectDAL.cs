using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public static class ObjectDAL
    {
        public const string EnemyPath = @".\Assets\JSON\Enemies";
        public const string CardPath = @".\Assets\JSON\Cards";

        internal static List<Card> GetCards(List<string> cardsToGet)
        {
            var cards = GetAllCards();
            var cardReturn = new List<Card>();
            
            foreach (string cardToGet in cardsToGet)
            {
                var card = cards.CardItems.Find(item => item.CardName.Equals(cardToGet));
                cardReturn.Add(card);
            }

            return cardReturn;
        }


        internal static List<Enemy> GetEnemies(int enemyLevel)
        {

            string text = File.ReadAllText(EnemyPath);
            var enemyList = JsonUtility.FromJson<EnemyWrapper>(text);
            
            var enemies = enemyList.EnemyItems.FindAll(item => item.EnemyLevel <= enemyLevel);
            
            return enemies;

        }



        internal static void SaveCards(CardWrapper cardsToEdit)
        {


            if (!File.Exists(CardPath))
            {
                return;
            }
            string jsonCard = JsonUtility.ToJson(cardsToEdit);
            File.WriteAllText(CardPath, jsonCard);
        }

        internal static void SaveEnemies(EnemyWrapper awesomeNewMonster)
        {
            if (!File.Exists(EnemyPath))
            {
                return;
            }
            string jsonEnemy = JsonUtility.ToJson(awesomeNewMonster);

            File.WriteAllText(EnemyPath, jsonEnemy);
        }


        internal static EnemyWrapper GetAllEnemies()
        {
            string text = File.ReadAllText(Constants.EnemyPath);
            return JsonUtility.FromJson<EnemyWrapper>(text);

        }


        internal static CardWrapper GetAllCards()
        {
            string text = File.ReadAllText(Constants.CardPath);
            return JsonUtility.FromJson<CardWrapper>(text);
            
        }
    }
}
