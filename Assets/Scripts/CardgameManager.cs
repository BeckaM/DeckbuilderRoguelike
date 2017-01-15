using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardgameManager : MonoBehaviour
{


    public enum Turn { MyTurn, AITurn };

    public List<GameObject> MyDeckCards = new List<GameObject>();
    public List<GameObject> MyHandCards = new List<GameObject>();
    public List<GameObject> MyTableCards = new List<GameObject>();

    public List<GameObject> AIDeckCards = new List<GameObject>();
    public List<GameObject> AIHandCards = new List<GameObject>();
    public List<GameObject> AITableCards = new List<GameObject>();



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

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

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
}