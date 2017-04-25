using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public static class ObjectDAL
    {

        public const string EnemyPath = @".\Assets\Resources\Enemies.json";
        public const string CardPath = @".\Assets\Resources\Cards.json";
        public const string ClassPath = @".\Assets\Resources\PlayerClasses.json";
        public const string PerkPath = @".\Assets\Resources\Perks.json";
        
        internal static List<Card> GetCards(List<string> cardsToGet)
        {
            var cards = GetAllCards();
            var cardReturn = new List<Card>();

            foreach (string cardToGet in cardsToGet)
            {
                var card = cards.cardItems.Find(item => item.cardName.Equals(cardToGet));
                cardReturn.Add(card);
            }

            return cardReturn;
        }

        internal static Card GetCard(string cardToGet)
        {
            var cards = GetAllCards();

            var card = cards.cardItems.Find(item => item.cardName.Equals(cardToGet));

            return card;
        }

        internal static Card GetRandomCard(int minLevel, int maxLevel, Card.Type type)
        {
            var cards = GetAllCards();

            //var levelCards = new List<Card>();

            var levelCards = cards.cardItems.FindAll(item => item.level >= minLevel && item.level <= maxLevel && item.type.Equals(type));

            Card cardReturn = levelCards[UnityEngine.Random.Range(0, levelCards.Count)];

            return cardReturn;
        }

        internal static Card GetRandomCard(int minLevel, int maxLevel)
        {
            var cards = GetAllCards();
                        
            var levelCards = cards.cardItems.FindAll(item => item.level >= minLevel && item.level <= maxLevel && item.type != (Card.Type.Consumable));
                        
            Card cardReturn = levelCards[UnityEngine.Random.Range(0, levelCards.Count)];

            return cardReturn;
        }

        internal static List<Card> GetClassCards(int minLevel, int maxLevel, PlayerClass.ClassName classType)
        {
            var cards = GetAllCards();

            var levelCards = new List<Card>();

            while (levelCards.Count == 0)
            {
                levelCards = cards.cardItems.FindAll(item => item.level >= minLevel && item.level <= maxLevel && item.classOwner.Equals(classType));
                minLevel--;
            }

            return levelCards;
        }


        internal static List<Enemy> GetEnemies(int enemyLevel, Enemy.MonsterType type)
        {

            // string text = File.ReadAllText(EnemyPath);
            //var enemyList = JsonUtility.FromJson<EnemyWrapper>(text);
            var enemyList = GetAllEnemies();

            var enemies = enemyList.EnemyItems.FindAll(item => item.BaseEnemyLevel <= enemyLevel && item.type.Equals(type));

            return enemies;
        }

        internal static Enemy GetEnemy(string name)
        {

            // string text = File.ReadAllText(EnemyPath);
            //var enemyList = JsonUtility.FromJson<EnemyWrapper>(text);
            var enemyList = GetAllEnemies();

            var enemy = enemyList.EnemyItems.Find(item => item.EnemyName.Equals(name));

            return enemy;
        }

        internal static void SaveCards(CardWrapper cardsToEdit)
        {
            if (!File.Exists(CardPath))
            {
                Debug.LogError("No file at" + CardPath);
                return;
            }
            string jsonCard = JsonUtility.ToJson(cardsToEdit);
            File.WriteAllText(CardPath, jsonCard);
        }

        internal static void SaveEnemies(EnemyWrapper awesomeNewMonster)
        {
            if (!File.Exists(EnemyPath))
            {
                Debug.LogError("No file at" + EnemyPath);
                return;
            }
            string jsonEnemy = JsonUtility.ToJson(awesomeNewMonster);

            File.WriteAllText(EnemyPath, jsonEnemy);
        }

        internal static void SaveClasses(PlayerClassWrapper classesToEdit)
        {
            if (!File.Exists(ClassPath))
            {
                Debug.LogError("No file at" + ClassPath);
                return;
            }
            string jsonClass = JsonUtility.ToJson(classesToEdit);

            File.WriteAllText(ClassPath, jsonClass);
        }


        internal static void SavePerks(PerkWrapper perksToEdit)
        {
            if (!File.Exists(PerkPath))
            {
                Debug.LogError("No file at" + PerkPath);
                return;
            }
            string jsonPerks = JsonUtility.ToJson(perksToEdit);

            File.WriteAllText(PerkPath, jsonPerks);
        }


        internal static EnemyWrapper GetAllEnemies()
        {
            TextAsset file = Resources.Load<TextAsset>("Enemies");
            return JsonUtility.FromJson<EnemyWrapper>(file.text);
        }


        internal static CardWrapper GetAllCards()
        {
            TextAsset file = Resources.Load<TextAsset>("Cards");
            return JsonUtility.FromJson<CardWrapper>(file.text);
        }


        internal static PlayerClassWrapper GetAllClasses()
        {
            TextAsset file = Resources.Load<TextAsset>("PlayerClasses");
            return JsonUtility.FromJson<PlayerClassWrapper>(file.text);
        }


        internal static PerkWrapper GetAllPerks()
        {
            TextAsset file = Resources.Load<TextAsset>("Perks");
            return JsonUtility.FromJson<PerkWrapper>(file.text);
        }
    }
}
