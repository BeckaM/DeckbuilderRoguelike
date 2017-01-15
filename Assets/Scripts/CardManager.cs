using Assets.Scripts;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{

    private const string fileName = @"C:\Users\Per\Documents\DeckbuilderRoguelike\Assets\JSON\Cards.json";


    public string CardName;
    public string CardText;
    public int SpriteIcon;
    public int Damage;
    public GameObject CardObject;
    public Sprite[] sprites;

    public enum Team { My, AI };
    public Team team = Team.My;

    public enum CardStatus { InDeck, InHand, OnTable, InDiscard };
    public CardStatus cardStatus = CardStatus.InDeck;

    // public List<Card> Deck = new List<Card>();


    public void SetCardStatus(CardStatus status)
    {
        cardStatus = status;
    }

    public void CreateCard()
    {
        string text = File.ReadAllText(fileName);
        var cardList = JsonUtility.FromJson<Wrapper>(text);

        Card card = new Card
        {
            CardName = this.CardName,
            CardText = this.CardText,
            SpriteIcon = this.SpriteIcon,
            Damage = this.Damage
        };

        if(cardList == null)
        {
            cardList = new Wrapper();
            cardList.CardItems = new System.Collections.Generic.List<Card>();
        }

        cardList.CardItems.Add(card);

                
        string jsonCard = JsonUtility.ToJson(cardList);
        SaveCard(jsonCard);
    }




    private void SaveCard(string awesomeNewCard)
    {
        if (!File.Exists(fileName))
        {
            return;
        }

        File.WriteAllText(fileName, awesomeNewCard);
    }
    public void GetCard(Card card)
    {

            CardName = card.CardName;
            CardText = card.CardText;
            SpriteIcon = card.SpriteIcon;
            Damage = card.Damage;
            
            var transformer = this.transform;

            //Set Image
            var imageObj = transformer.GetChild(0);
            var imageComponent = imageObj.GetComponent<Image>();
            imageComponent.sprite = sprites[card.SpriteIcon];

            //Set Card Title
            var cardTitle = transformer.GetChild(1);
            var titleComponent = cardTitle.GetComponent<Text>();
            titleComponent.text = CardName;

            //Set Card Description
            var cardDesc = transformer.GetChild(2);
            var DescComponent = cardDesc.GetComponent<Text>();
            DescComponent.text = CardText;

            
        

        
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


