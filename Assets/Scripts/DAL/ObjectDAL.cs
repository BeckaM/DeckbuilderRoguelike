using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.DAL
{
    public static class ObjectDAL
    {

        public const string EnemyPath = @".\Resources\Enemies.json";
        public const string CardPath = @".\Resources\Cards.json";
        public const string ClassPath = @".\Resources\PlayerClasses.json";
        public const string PerkPath = @".\Resources\Perks.json";

        //public const string EnemyPath = @".\Assets\JSON\Enemies";
        //public const string CardPath = @".\Assets\JSON\Cards";
        //public const string ClassPath = @".\Assets\JSON\PlayerClasses";
        //public const string PerkPath = @".\Assets\JSON\Perks";



        internal static List<PlayerClass> GetClasses(List<string> classesToGet)
        {
            var playerclasses = GetAllClasses();
            var classReturn = new List<PlayerClass>();

            foreach (string classtoget in classesToGet)
            {
                var playerclass = playerclasses.PlayerClasses.Find(item => item.ClassName.Equals(classtoget));
                classReturn.Add(playerclass);
            }

            return classReturn;
        }

        internal static Card GetRandomConsumable(int level)
        {
            var cards = GetAllCards();

            var consumables = cards.cardItems.FindAll(item => item.type.Equals(Card.Type.Consumable) && item.level <= level);

            Card cardReturn = consumables[UnityEngine.Random.Range(0, consumables.Count)];

            return cardReturn;
        }


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

        internal static Card GetRandomCard(int minLevel, int maxLevel)
        {
            var cards = GetAllCards();

            var levelCards = new List<Card>();

            while (levelCards.Count == 0)
            {
                levelCards = cards.cardItems.FindAll(item => item.level >= minLevel && item.level <= maxLevel && item.type.Equals(Card.Type.MonsterCard));
                minLevel--;
            }

            Card cardReturn = levelCards[UnityEngine.Random.Range(0, levelCards.Count)];

            return cardReturn;
        }

        internal static Card GetRandomClassCard(int minLevel, int maxLevel)
        {
            var cards = GetAllCards();

            var levelCards = new List<Card>();

            while (levelCards.Count == 0)
            {
                levelCards = cards.cardItems.FindAll(item => item.level >= minLevel && item.level <= maxLevel && item.type.Equals(Card.Type.ClassCard));
                minLevel--;
            }

            Card cardReturn = levelCards[UnityEngine.Random.Range(0, levelCards.Count)];

            return cardReturn;
        }


        internal static List<Enemy> GetEnemies(int enemyLevel)
        {

            // string text = File.ReadAllText(EnemyPath);
            //var enemyList = JsonUtility.FromJson<EnemyWrapper>(text);
            var enemyList = GetAllEnemies();

            var enemies = enemyList.EnemyItems.FindAll(item => item.BaseEnemyLevel <= enemyLevel);

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

        internal static void SaveClasses(PlayerClassWrapper classesToEdit)
        {
            if (!File.Exists(ClassPath))
            {
                return;
            }
            string jsonClass = JsonUtility.ToJson(classesToEdit);

            File.WriteAllText(ClassPath, jsonClass);
        }


        internal static void SavePerks(PerkWrapper perksToEdit)
        {
            if (!File.Exists(PerkPath))
            {
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
