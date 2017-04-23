using UnityEngine;
using System.Collections;
using UnityEditor;
using Assets.Scripts.DAL;




[CustomEditor(typeof(MonsterEditor))]
public class InspectorMonster : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MonsterEditor myScript = (MonsterEditor)target;
        if (GUILayout.Button("Get all Monsters"))
        {
            UnityEditor.AssetDatabase.Refresh();
            myScript.GetMonstersToEdit();
        }

        if (GUILayout.Button("Save"))
        {
            myScript.SaveMonsters();
        }
    }
}
