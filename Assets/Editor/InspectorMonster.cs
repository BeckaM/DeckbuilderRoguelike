﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Assets.Scripts
{

    [CustomEditor(typeof(EnemyManager))]
    public class InspectorMonster : Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();


            EnemyManager myScript = (EnemyManager)target;
            if (GUILayout.Button("Create New Enemy"))
            {
                myScript.CreateEnemy();
            }





        }
    }
}