using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CardManager))]
public class InspectorCard : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();


        CardManager myScript = (CardManager)target;
        if (GUILayout.Button("Create New Card"))
        {
            myScript.CreateCard();
        }

      
        if (GUILayout.Button("Instantiate Card"))
        {
            string[] cardstoCreate = new string[5];
            cardstoCreate[0] = "Murloc2";
            cardstoCreate[1] = "Murloc3";
            cardstoCreate[2] = "Unicorn";
            cardstoCreate[3] = "Unicorn";
            cardstoCreate[4] = "Unicorn";

            myScript.AddCardtoDeck(cardstoCreate);
        }


    }
}

