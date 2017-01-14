using Assets.Scripts;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CardManager : MonoBehaviour
{

    private const string fileName = @"C:\tmp\Cards.json"; //@"C:\Users\Public\Documents\Unity Projects\DeckbuilderRoguelike\Assets\JSON\Cards.json";

    public string CardName;
    public string CardText;
    public int SpriteIcon;
    public int Damage;
    public GameObject CardObject;
    public Sprite[] sprites;

    // public List<Card> Deck = new List<Card>();

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

    public void AddCardtoDeck(string[] cardsToCreate)
    {
        string text = File.ReadAllText(fileName);
        var cardList = JsonUtility.FromJson<Wrapper>(text);


        foreach (string cardToCreate in cardsToCreate)
        {

            // Search for card in json file
            var card = cardList.CardItems.Find(item => item.CardName.Equals(cardToCreate));

            CardName = card.CardName;
            CardText = card.CardText;
            SpriteIcon = card.SpriteIcon;
            Damage = card.Damage;

            GameObject instance = Instantiate(CardObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            var deck = GameObject.Find("Deck").transform;
            instance.transform.SetParent(deck);


            var transformer = instance.transform;

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


