
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{

    public class EnemyManager : MonoBehaviour
    {
        public Enemy enemy;

        public int life;
        public int maxLife;
        public int ward = 0;
        public int damageBoost = 0;

        public int mana = 1;
        public int maxMana = 1;

        public int monsterLevel;
        public int experienceReward;

        public GameObject enemyObject;
        public Sprite[] sprites;
        //  public Sprite monsterImage;
        public SpriteRenderer monsterRenderer;
        public TMP_Text monsterLVLText;

        //Gets monster properties from Dungeon Manager and sets them on the enemy object.
        public void PopulateEnemy(Enemy enemytoget, int level)
        {
            monsterLevel = level;
            enemy = enemytoget;

            int HPperlevel = enemy.BaseEnemyHP / 8;
            int HPbonus = HPperlevel * (monsterLevel - enemy.BaseEnemyLevel);
            life = enemy.BaseEnemyHP + (HPperlevel * HPbonus);
            maxLife = life;
            experienceReward = 4 + monsterLevel;

            var transformer = transform;

            //Set Image
            monsterRenderer.sprite = sprites[enemy.SpriteIcon];
            monsterRenderer.color = enemytoget.spriteColor;

            //Set lvl text
            monsterLVLText.text = "LVL" + level.ToString();
        }

        //Create all the cards in the scene for the monsters deck when the player fights it. 
        internal List<CardManager> InitMonsterDeck()
        {
            var enemyDeck = EnemyDeckBuilder.BuildMonsterDeck(enemy.Components, monsterLevel);

            var returnList = DeckManager.monster.CreateCardObjects(enemyDeck);

            foreach (CardManager card in returnList)
            {
                card.owner = CardgameManager.Team.Opponent;
                DeckManager.monster.cardsInDeck.Add(card);
            }

            return returnList;
                        

        }

        internal IEnumerator initAI()
        {
            Debug.Log("initAI");
            MonsterBrain.PlayCards();

            Debug.Log("Waiting for anumations to finish");

            while (EventManager.Instance.processingQueue == true)
            {
                yield return new WaitForSeconds(1);
            }
            
            CardgameManager.instance.EndTurn();
        }
    }
}
