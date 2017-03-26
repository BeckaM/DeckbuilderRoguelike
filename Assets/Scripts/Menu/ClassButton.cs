using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Menu
{

    public class ClassButton : MonoBehaviour
    {
        public ClassSelect classSelect;
        public GameObject lockPanel;
        public Image classImage;
        public TMP_Text className;
        public List<Image> classImages;

        public PlayerClass playerClass;

        public void selectClass()
        {
            classSelect.SelectClass(playerClass);
        }

        public void PopulateClassButton(PlayerClass playerClass)
        {
            this.playerClass = playerClass;
            classImage = classImages[playerClass.SpriteIcon];
            className.text = playerClass.ClassName;
        }

        public void LockClass(bool locked)
        {
            if (locked)
            {
                lockPanel.SetActive(true);
            }
            else
            {
                lockPanel.SetActive(false);
            }
        }


    }
}