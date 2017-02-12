using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.Scripts
{


    public class CardManager : MonoBehaviour
    {
                
        
        public Sprite[] sprites;
        public Card card;

        public enum CardStatus { InDeck, InHand, OnTable, InDiscard };
        public CardStatus cardStatus = CardStatus.InDeck;

        public enum Team { My, AI };
        public Team team = Team.My;

        public bool isPlayable = false;
        public bool isDragable = false;

        public void SetCardStatus(CardStatus status)
        {
            cardStatus = status;
        }



        public void PopulateCard(Card card)
        {

            this.card = card;

            var transformer = this.transform;

            //Set Image
            var imageObj = transformer.GetChild(0);
            var imageComponent = imageObj.GetComponent<Image>();
            imageComponent.sprite = sprites[card.SpriteIcon];

            //Set Card Title
            var cardTitle = transformer.GetChild(1);
            var titleComponent = cardTitle.GetComponent<Text>();
            titleComponent.text = card.CardName;

            //Set Card Description
            var cardDesc = transformer.GetChild(2);
            var DescComponent = cardDesc.GetComponent<Text>();
            DescComponent.text = card.CardText;
        }
             
        internal void ApplyEffect(CardEffect cardEffect)
        {
            if(CardEffect.Effect.DealDamage.Equals(cardEffect.effect))
            {

                CardgameManager.instance.ApplyDamage(cardEffect.Value, team);

            }

            else if (CardEffect.Effect.Heal.Equals(cardEffect.effect))
            {

                CardgameManager.instance.ApplyHealing(cardEffect.Value, team);

            }


            //if effect is damage
            // deal "value"
            // trigger CardGameManager.DealDamage(Value)

            //else if is heal 

            // heal "value"
            //trigger CardGameManager.Heal(Value)

            //else if DrawCard
            //trigger CardGameManager.DrawCard(Value)



            
        }



        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

