using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public class ModalPanel : MonoBehaviour
    {
        public Text title;
        public Text subText;

        public Button addButton;
        public Button noButton;
        public Button thanksButton;
        public GameObject modalPanelObject;
        public GameObject choice1;
        public GameObject choice2;
        public GameObject choice3;
        public int currentChoice;
        
        private static ModalPanel modalPanel;

        public static ModalPanel Instance()
        {
            if (!modalPanel)
            {
                modalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
                if (!modalPanel)
                    Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
            }

            return modalPanel;
        }

        //  Chest with card or consumable.
        public void Chest(string title, string subText, Card reward, UnityAction yesEvent, UnityAction noEvent)
        {
            modalPanelObject.SetActive(true);

            addButton.onClick.RemoveAllListeners();
            addButton.onClick.AddListener(yesEvent);
            addButton.onClick.AddListener(ClosePanel);

            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(noEvent);
            noButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;
            choice1.GetComponent<Choice>().PopulateChoice(reward);
            choice1.SetActive(true);
            choice2.SetActive(false);
            choice3.SetActive(false);

            addButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);
            thanksButton.gameObject.SetActive(false);
            
        }
               
                //Chest with gold.
        public void Chest(string title, string subText, int goldReward, UnityAction yesEvent)
        {
            modalPanelObject.SetActive(true);

            thanksButton.onClick.RemoveAllListeners();
            thanksButton.onClick.AddListener(yesEvent);
            thanksButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;
            choice1.GetComponent<Choice>().PopulateChoice(goldReward);
            choice1.SetActive(true);
            choice2.SetActive(false);
            choice3.SetActive(false);

            addButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            thanksButton.gameObject.SetActive(true);
        }

        internal void Shrine(string title, string subText, List<Prayer> prayers, UnityAction noEvent)
        {
            modalPanelObject.SetActive(true);

            addButton.onClick.RemoveAllListeners();
            addButton.onClick.AddListener(prayers[currentChoice].prayerEvent);
            addButton.onClick.AddListener(ClosePanel);

            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(noEvent);
            noButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;
                       
            choice1.GetComponent<Choice>().PopulateChoice(prayers[0]);
            choice1.SetActive(true);
            choice1.GetComponent<Button>().interactable = true;
            choice1.GetComponent<Button>().onClick.AddListener(Choose1);

            choice2.GetComponent<Choice>().PopulateChoice(prayers[1]);
            choice2.SetActive(true);
            choice2.GetComponent<Button>().interactable = true;
            choice2.GetComponent<Button>().onClick.AddListener(Choose2);

            choice3.GetComponent<Choice>().PopulateChoice(prayers[2]);
            choice3.SetActive(true);
            choice3.GetComponent<Button>().interactable = true;
            choice3.GetComponent<Button>().onClick.AddListener(Choose3);

            addButton.gameObject.SetActive(true);
            addButton.GetComponent<Button>().interactable = false;

            noButton.gameObject.SetActive(true);
            thanksButton.gameObject.SetActive(false);
        }

        private void Choose1()
        {
            currentChoice = 0;
            choice1.GetComponent<Outline>().enabled = true;
            choice2.GetComponent<Outline>().enabled = false;
            choice3.GetComponent<Outline>().enabled = false;
            addButton.GetComponent<Button>().interactable = true;
        }

        private void Choose2()
        {
            currentChoice = 1;
            choice2.GetComponent<Outline>().enabled = true;
            addButton.GetComponent<Button>().interactable = true;
            choice1.GetComponent<Outline>().enabled = false;
            choice3.GetComponent<Outline>().enabled = false;
        }

        private void Choose3()
        {
            currentChoice = 2;
            choice3.GetComponent<Outline>().enabled = true;
            addButton.GetComponent<Button>().interactable = true;
            choice2.GetComponent<Outline>().enabled = false;
            choice1.GetComponent<Outline>().enabled = false;
        }


        private void PrayerChoice()
        {

        }




        void ClosePanel()
        {
            modalPanelObject.SetActive(false);
        }
    }
}