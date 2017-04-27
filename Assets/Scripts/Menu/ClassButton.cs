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
        
        public PlayerClass playerClass;

        public void selectClass()
        {
            classSelect.SelectClass(playerClass);
        }

        public void PopulateClassButton(PlayerClass playerClass)
        {
            this.playerClass = playerClass;
            classImage.sprite = GameManager.instance.classImages[playerClass.spriteIcon];
            className.text = playerClass.className.ToString().Replace("_", " ");           
        }

        public void LockClass(bool locked)
        {
            if (locked)
            {
                lockPanel.SetActive(true);
                this.GetComponent<Button>().interactable = false;
            }
            else
            {
                this.GetComponent<Button>().interactable = true;
                lockPanel.SetActive(false);
            }
        }


    }
}