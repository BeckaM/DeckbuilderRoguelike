using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace Assets.Scripts
{
    public class ModalPanel : MonoBehaviour
    {

        public Text question;

        public Button yesButton;
        public Button noButton;
        public Button cancelButton;
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

        //  Announcement: A string and Cancel event;
        public void Chest(string title, UnityAction yesEvent, UnityAction noEvent)
        {
            modalPanelObject.SetActive(true);

            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(yesEvent);
            yesButton.onClick.AddListener(ClosePanel);

            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(noEvent);
            noButton.onClick.AddListener(ClosePanel);

            this.question.text = title;
            
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);
           
        }

        ////  Announcement with Image:  A string, a Sprite and Cancel event;
        //public void Choice(string question, Sprite iconImage, UnityAction cancelEvent)
        //{
        //    modalPanelObject.SetActive(true);

        //    cancelButton.onClick.RemoveAllListeners();
        //    cancelButton.onClick.AddListener(cancelEvent);
        //    cancelButton.onClick.AddListener(ClosePanel);

        //    this.question.text = question;


        //    this.iconImage.gameObject.SetActive(true);
        //    yesButton.gameObject.SetActive(false);
        //    noButton.gameObject.SetActive(false);
        //    cancelButton.gameObject.SetActive(true);
        //}

        ////  Yes/No: A string, a Yes event, a No event (No Cancel Button);
        //public void Choice(string question, UnityAction yesEvent, UnityAction noEvent)
        //{
        //    modalPanelObject.SetActive(true);



        //    this.question.text = question;

        //    this.iconImage.gameObject.SetActive(false);
        //    yesButton.gameObject.SetActive(true);
        //    noButton.gameObject.SetActive(true);
        //    cancelButton.gameObject.SetActive(false);
        //}

        ////  Yes/No/Cancel: A string, a Yes event, a No event and Cancel event;
        //public void Choice(string question, UnityAction yesEvent, UnityAction noEvent, UnityAction cancelEvent)
        //{
        //    modalPanelObject.SetActive(true);

        //    yesButton.onClick.RemoveAllListeners();
        //    yesButton.onClick.AddListener(yesEvent);
        //    yesButton.onClick.AddListener(ClosePanel);

        //    noButton.onClick.RemoveAllListeners();
        //    noButton.onClick.AddListener(noEvent);
        //    noButton.onClick.AddListener(ClosePanel);

        //    cancelButton.onClick.RemoveAllListeners();
        //    cancelButton.onClick.AddListener(cancelEvent);
        //    cancelButton.onClick.AddListener(ClosePanel);

        //    this.question.text = question;

        //    this.iconImage.gameObject.SetActive(false);
        //    yesButton.gameObject.SetActive(true);
        //    noButton.gameObject.SetActive(true);
        //    cancelButton.gameObject.SetActive(true);
        //}

        ////  Yes/No with Image: A string, a Sprite, a Yes event, a No event (No Cancel Button);
        //public void Choice(string question, Sprite iconImage, UnityAction yesEvent, UnityAction noEvent)
        //{
        //    modalPanelObject.SetActive(true);

        //    yesButton.onClick.RemoveAllListeners();
        //    yesButton.onClick.AddListener(yesEvent);
        //    yesButton.onClick.AddListener(ClosePanel);

        //    noButton.onClick.RemoveAllListeners();
        //    noButton.onClick.AddListener(noEvent);
        //    noButton.onClick.AddListener(ClosePanel);

        //    this.question.text = question;
        //    this.iconImage.sprite = iconImage;

        //    this.iconImage.gameObject.SetActive(true);
        //    yesButton.gameObject.SetActive(true);
        //    noButton.gameObject.SetActive(true);
        //    cancelButton.gameObject.SetActive(false);
        //}

        ////  Yes/No/Cancel with Image: A string, a Sprite, a Yes event, a No event and Cancel event;
        //public void Choice(string question, Sprite iconImage, UnityAction yesEvent, UnityAction noEvent, UnityAction cancelEvent)
        //{
        //    modalPanelObject.SetActive(true);

        //    yesButton.onClick.RemoveAllListeners();
        //    yesButton.onClick.AddListener(yesEvent);
        //    yesButton.onClick.AddListener(ClosePanel);

        //    noButton.onClick.RemoveAllListeners();
        //    noButton.onClick.AddListener(noEvent);
        //    noButton.onClick.AddListener(ClosePanel);

        //    cancelButton.onClick.RemoveAllListeners();
        //    cancelButton.onClick.AddListener(cancelEvent);
        //    cancelButton.onClick.AddListener(ClosePanel);

        //    this.question.text = question;
        //    this.iconImage.sprite = iconImage;

        //    this.iconImage.gameObject.SetActive(true);
        //    yesButton.gameObject.SetActive(true);
        //    noButton.gameObject.SetActive(true);
        //    cancelButton.gameObject.SetActive(true);
        //}

        void ClosePanel()
        {
            modalPanelObject.SetActive(false);
        }
    }
}