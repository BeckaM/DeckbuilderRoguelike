using UnityEngine;
using UnityEditor;
using Assets.Scripts.DAL;

[CustomEditor(typeof(CardEditor))]
public class InspectorCard : Editor
{    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CardEditor myScript = (CardEditor)target;
        if (GUILayout.Button("Get all cards"))
        {
            myScript.GetCardsToEdit();
        }

        if (GUILayout.Button("Save"))
        {
            myScript.SaveCards();
        }
    }
}

