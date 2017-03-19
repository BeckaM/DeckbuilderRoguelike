using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LevelUpButton : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(GameManager.instance.LevelUp);
        }
    }


}
