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

      
     


    }
}

