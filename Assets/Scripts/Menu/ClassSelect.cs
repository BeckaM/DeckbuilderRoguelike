using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Assets.Scripts.Menu
{

    public class ClassSelect : MonoBehaviour
    {
        public Menu mainMenu;

        public List<ClassButton> classButtons;

        public GameObject selectedClassPanel;
        public PlayerClass selectedClass;
        public Image selectedClassImage;
        public TMP_Text selectedClassText;
        public Button selectButton;

        // Use this for initialization
        void Start()
        {
            selectButton.interactable = false;

            var playerClasses = DAL.ObjectDAL.GetAllClasses();
            int num = 0;
            foreach (ClassButton classButton in classButtons)
            {
                classButton.PopulateClassButton(playerClasses.PlayerClasses[num]);
                num++;
                classButton.LockClass(GameManager.instance.progressManager.CheckClassUnlock(classButton.playerClass.ClassName));
            }
        }


        public void SelectClass(PlayerClass playerClass)
        {
            selectedClass = playerClass;
            selectedClassImage.sprite = GameManager.instance.classImages[playerClass.SpriteIcon];
            selectedClassText.text = playerClass.ClassName;

            selectButton.interactable = true;
        }

        public void Select()
        {
            GameManager.instance.playerClass = selectedClass;
            mainMenu.HideClassSelect();
        }


    }
}
