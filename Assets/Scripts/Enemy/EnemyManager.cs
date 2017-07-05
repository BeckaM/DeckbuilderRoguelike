
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine.EventSystems;
using System;

namespace Assets.Scripts
{

    public class EnemyManager : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
    {
        public Enemy enemy;

        public int life;
        public int maxLife;
        public int ward = 0;
        public int damageBoost = 0;

        public int mana = 1;
        public int maxMana = 10;
        public int manaPerTurn = 1;

        public int monsterLevel;
        public int experienceReward;
        public Outline outline;
              
        public TMP_Text monsterLVLText;

        //Gets monster properties from Dungeon Manager and sets them on the enemy object.
        public void PopulateEnemy(Enemy enemytoget, int level)
        {
            monsterLevel = level;
            enemy = enemytoget;
                       
            life = enemy.BaseEnemyHP + (enemy.HPPerLevel*level);
            maxLife = life;
            experienceReward = 4 + monsterLevel;
                        
            //Set Mesh 
            GameObject mesh = Resources.Load("Monsters/" + enemytoget.EnemyName, typeof(GameObject)) as GameObject;

            if (mesh == null)
            {               
                   mesh = Resources.Load("Monsters/" + "Placeholder", typeof(GameObject)) as GameObject;
            }

            var e = Instantiate(mesh, this.transform);

            var randomSize = UnityEngine.Random.Range(0f, 0.5f);
            e.transform.localScale += new  Vector3(randomSize, randomSize, randomSize);
            
            var randomRotation = UnityEngine.Random.Range(-30f, 30f);
            e.transform.Rotate(new Vector3(0, randomRotation, 0));

            var randomAnimSpeed = UnityEngine.Random.Range(1f, 1.5f);
            var animator = e.GetComponent<Animator>();
            animator.SetFloat("AnimDelay", randomAnimSpeed);


            outline = e.GetComponentInChildren<Outline>();
            outline.enabled = false;

            //Set Image
            //monsterRenderer.sprite = sprites[enemy.SpriteIcon];
            //monsterRenderer.color = enemytoget.spriteColor;

            //Set lvl text
            monsterLVLText.text = "LVL" + level.ToString();
            monsterLVLText.enabled = false;
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("pointer enter");
            outline.enabled = true;
            monsterLVLText.enabled = true;

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            outline.enabled = false;
            monsterLVLText.enabled = false;
        }
    }
}
