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

            addButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            thanksButton.gameObject.SetActive(true);
        }

        internal void Shrine(string title, string subText, List<Prayer> prayers, UnityAction yesEvent, UnityAction noEvent)
        {
            modalPanelObject.SetActive(true);

            thanksButton.onClick.RemoveAllListeners();
            thanksButton.onClick.AddListener(yesEvent);
            thanksButton.onClick.AddListener(ClosePanel);

            this.title.text = title;
            this.subText.text = subText;
            choice1.GetComponent<Choice>().PopulateChoice();
            choice1.SetActive(true);

            addButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            thanksButton.gameObject.SetActive(true);
        }
                     

        void ClosePanel()
        {
            modalPanelObject.SetActive(false);
        }
    }
}