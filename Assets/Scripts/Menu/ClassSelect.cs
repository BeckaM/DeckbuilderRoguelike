﻿using System.Collections;
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
        public TMP_Text selectButtonText;

        public GameObject progressPanel;
        public ProgressPanel progressPanelManager;

        public GameObject classModelHolder;
        public GameObject currentPlayerModel;
        public List<GameObject> playerModels;

        public bool isProgress;
        
        void Start()
        {
            selectButton.interactable = false;

            var playerClasses = DAL.ObjectDAL.GetAllClasses();

            foreach (PlayerClass pClass in playerClasses.playerClasses)
            {
                var classButton = Instantiate(classButtonObj);
                classButton.transform.SetParent(classPanel.transform, false);
                var cbScript = classButton.GetComponent<ClassButton>();
                cbScript.classSelect = this;

                cbScript.PopulateClassButton(pClass);

                cbScript.LockClass(!GameManager.instance.progressManager.CheckClassUnlock(pClass.className));
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
        
        public void SelectClass(PlayerClass playerClass, bool locked)
        {
            if (isProgress)
            {
                progressPanelManager.ShowClassProgress(playerClass);
            }
            else
            {
                selectedClass = playerClass;                           
                selectedClassText.text = playerClass.className.ToString().Replace("_", " ");
                if(currentPlayerModel!= null)
                {
                    Destroy(currentPlayerModel);
                }
                currentPlayerModel = Instantiate(playerModels[playerClass.playerModel], classModelHolder.transform);

                if (!locked)
                {
                    selectButton.interactable = true;
                    selectButtonText.text = "Select";
                }
                else
                {
                    selectButton.interactable = false;
                    selectButtonText.text = "Locked";
                }
            }
        }
        
        public void Select()
        {
            GameManager.instance.playerClass = selectedClass;
            GameManager.instance.progressManager.SetProgressClass(selectedClass);
            DeckManager.player.StartingDeck(selectedClass.startingDeck);
            mainMenu.ShowPerkSelect();            
        }
        
        public void HideClassSelect()
        {
            this.gameObject.SetActive(false);
        }
    }
}
