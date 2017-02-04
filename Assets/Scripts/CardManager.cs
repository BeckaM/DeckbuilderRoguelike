using Assets.Scripts;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.DAL;

public class CardManager : MonoBehaviour
{

    private const string fileName = Constants.CardPath;

    public Sprite[] sprites;
    public Card card;
        
    public enum CardStatus { InDeck, InHand, OnTable, InDiscard };
    public CardStatus cardStatus = CardStatus.InDeck;

    public enum Team { My, AI };
    public Team team = Team.My;

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

    public void PlaceCard()
    {
        if (CardgameManager.instance.turn == CardgameManager.Turn.MyTurn && cardStatus == CardStatus.InHand && team == Team.My)
        {
            //Selected = false;
            CardgameManager.instance.PlaceCard(this);
        }
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


