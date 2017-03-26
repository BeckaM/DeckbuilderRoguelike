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
        
        // Use this for initialization
        void Start()
        {
            var playerClasses = DAL.ObjectDAL.GetAllClasses();
            int num = 0;
           foreach(ClassButton classButton in classButtons)
            {
                classButton.PopulateClassButton(playerClasses.PlayerClasses[num]);
                num++;
                classButton.LockClass(CheckClassUnlock(classButton.playerClass.ClassName));

            }
        }

        private bool CheckClassUnlock(string className)
        {
            GameManager.instance.progressManager.currentProgress.classProgressList
            //if (   )
            //{
                return true;
            //}
        }


        public void SelectClass(PlayerClass playerClass)
        {
           
        }
                
    }
}
