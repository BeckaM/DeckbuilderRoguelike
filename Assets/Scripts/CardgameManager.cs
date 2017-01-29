using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CardgameManager : MonoBehaviour
{


    
    public static CardgameManager instance;

    public enum Turn { MyTurn, AITurn };
    public Turn turn = Turn.MyTurn;

    int maxMana = 1;
    int MyMana = 1;
    int AIMana = 1;

    public List<GameObject> MyDeckCards = new List<GameObject>();
    public List<GameObject> MyHandCards = new List<GameObject>();
    public List<GameObject> MyTableCards = new List<GameObject>();

    public List<GameObject> AIDeckCards = new List<GameObject>();
    public List<GameObject> AIHandCards = new List<GameObject>();
    public List<GameObject> AITableCards = new List<GameObject>();



    void Awake()
    {
        instance = this;

    }


    // Use this for initialization
    void Start()
    {

       
        //Organize the cards into the correct lists.
        foreach (GameObject CardObject in GameObject.FindGameObjectsWithTag("Card"))
        {
            //CardObject.GetComponent<Rigidbody>().isKinematic = true;
            CardManager c = CardObject.GetComponent<CardManager>();

            if (c.team == CardManager.Team.My)
                MyDeckCards.Add(CardObject);
            else
                AIDeckCards.Add(CardObject);
        }

        //Draw our starting hand


        for (var i = 0; i < 3; i++)
        {


            DrawCardFromDeck(CardManager.Team.My);
            DrawCardFromDeck(CardManager.Team.AI);

        }

       

    }

    // Update is called once per frame
    void Update()
    {

    }



    //Triggered by end turn button.
    public void EndTurn()
    {
        if (turn == Turn.AITurn)
        {
            DrawCardFromDeck(CardManager.Team.My);
            turn = Turn.MyTurn;
        }
        else if (turn == Turn.MyTurn)
        {
            DrawCardFromDeck(CardManager.Team.AI);
            turn = Turn.AITurn;
        }




    }



    //Draw card function, Team indicates who draws.
    public void DrawCardFromDeck(CardManager.Team team)
    {

        if (team == CardManager.Team.My && MyDeckCards.Count != 0 && MyHandCards.Count < 10)
        {
            int random = Random.Range(0, MyDeckCards.Count);
            GameObject tempCard = MyDeckCards[random];


            var hand = GameObject.Find("Hand").transform;
            tempCard.transform.SetParent(hand);
            tempCard.GetComponent<CardManager>().SetCardStatus(CardManager.CardStatus.InHand);

            MyDeckCards.Remove(tempCard);
            MyHandCards.Add(tempCard);
        }

        if (team == CardManager.Team.AI && AIDeckCards.Count != 0 && AIHandCards.Count < 10)
        {
            int random = Random.Range(0, AIDeckCards.Count);
            GameObject tempCard = AIDeckCards[random];

            var AIhand = GameObject.Find("AIHand").transform;
            tempCard.transform.SetParent(AIhand);
            tempCard.GetComponent<CardManager>().SetCardStatus(CardManager.CardStatus.InHand);

            AIDeckCards.Remove(tempCard);
            AIHandCards.Add(tempCard);
        }
    }

         public void PlaceCard(CardManager card)
    {
        if (card.team == CardManager.Team.My && MyMana - card.Cost >= 0 && MyTableCards.Count < 10)
        {
            //card.gameObject.transform.position = MyTablePos.position;
    //        card.GetComponent<CardManager>().newPos = MyTablePos.position;

            MyHandCards.Remove(card.gameObject);
            MyTableCards.Add(card.gameObject);

            card.SetCardStatus(CardManager.CardStatus.OnTable);
      
            //PlaySound(cardDrop);

        //    if (card.cardtype == CardManager.CardType.Instant)///Apply Magic Effect 
        //    {
        //        card.canPlay = true;
        //        if (card.cardeffect == CardBehaviourScript.CardEffect.ToAll)
        //        {
        //            card.AddToAll(card, true, delegate { card.Destroy(card); });
        //        }
        //        else if (card.cardeffect == CardBehaviourScript.CardEffect.ToEnemies)
        //        {
        //            card.AddToEnemies(card, AITableCards, true, delegate { card.Destroy(card); });
        //        }
        //    }

        //    MyMana -= card.Cost;
        }

        //if (card.team == CardBehaviourScript.Team.AI && AIMana - card.Cost >= 0 && AITableCards.Count < 10)
        //{
        //    //card.gameObject.transform.position = AITablePos.position;
        //    card.GetComponent<CardBehaviourScript>().newPos = AITablePos.position;

        //    AIHandCards.Remove(card.gameObject);
        //    AITableCards.Add(card.gameObject);

        //    card.SetCardStatus(CardBehaviourScript.CardStatus.OnTable);
        //    //PlaySound(cardDrop);

        //    if (card.cardtype == CardBehaviourScript.CardType.Magic)///Apply Magic Effect 
        //    {
        //        card.canPlay = true;
        //        if (card.cardeffect == CardBehaviourScript.CardEffect.ToAll)
        //        {
        //            card.AddToAll(card, true, delegate { card.Destroy(card); });
        //        }
        //        else if (card.cardeffect == CardBehaviourScript.CardEffect.ToEnemies)
        //        {
        //            card.AddToEnemies(card, MyTableCards, true, delegate { card.Destroy(card); });
        //        }
        //    }

        //    AIMana -= card.Cost;
        //}

    //    TablePositionUpdate();
     //   HandPositionUpdate();
     //   UpdateGame();

    }


}