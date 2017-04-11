using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

namespace Assets.Scripts.Menu
{

    public class ClassSelect : MonoBehaviour
    {
        public Menu mainMenu;

        public GameObject classButtonObj;
        public GameObject classPanel;

        public GameObject selectedClassPanel;
        public PlayerClass selectedClass;
        public Image selectedClassImage;
        public TMP_Text selectedClassText;
        public Button selectButton;

        public GameObject progressPanel;
        public ProgressPanel progressPanelManager;

        public bool isProgress;
        
        void Start()
        {
            selectButton.interactable = false;

            var playerClasses = DAL.ObjectDAL.GetAllClasses();

            foreach (PlayerClass pClass in playerClasses.PlayerClasses)
            {
                var classButton = Instantiate(classButtonObj);
                classButton.transform.SetParent(classPanel.transform, false);
                var cbScript = classButton.GetComponent<ClassButton>();
                cbScript.classSelect = this;

                cbScript.PopulateClassButton(pClass);

                cbScript.LockClass(!GameManager.instance.progressManager.CheckClassUnlock(pClass.ClassName));
            }
        }


        internal void ShowClassSelect()
        {
            this.gameObject.SetActive(true);
            progressPanel.SetActive(false);
            selectedClassPanel.SetActive(true);
            progressPanelManager.ShowProgress();
        }


        internal void ShowProgress()
        {
            this.gameObject.SetActive(true);
            progressPanel.SetActive(true);
            selectedClassPanel.SetActive(false);
            progressPanelManager.ShowProgress();
        }


        public void SelectClass(PlayerClass playerClass)
        {
            if (isProgress)
            {
                progressPanelManager.ShowClassProgress(playerClass);
            }
            else
            {
                selectedClass = playerClass;
                selectedClassImage.sprite = GameManager.instance.classImages[playerClass.SpriteIcon];
                selectedClassText.text = playerClass.ClassName;

                selectButton.interactable = true;
            }
        }


        public void Select()
        {
            GameManager.instance.playerClass = selectedClass;
            GameManager.instance.progressManager.SetProgressClass(selectedClass);
            DeckManager.player.StartingDeck(selectedClass.Startingdeck);
            mainMenu.ShowPerkSelect();            
        }


        public void HideClassSelect()
        {
            this.gameObject.SetActive(false);
        }
    }
}
