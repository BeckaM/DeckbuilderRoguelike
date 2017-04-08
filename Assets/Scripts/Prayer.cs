//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.UI;

//namespace Assets.Scripts
//{
//    public class Prayer : MonoBehaviour
//    {
//        public GameObject prayerObject;

//        public Text prayerDescription;
//        public GameObject prayerImage;
//        public Text prayerName;
//        public string prayer;

//        public Sprite[] prayerIcons;
//        public UnityAction prayerEvent;

//        public void PopulatePrayer(UnityAction prayerEvent)
//        {
//            this.prayerEvent = prayerEvent;
//            prayer = prayerEvent.Method.Name;

//            if (prayerEvent.Method.Name == "Duplication")
//            {
//                prayerName.text = "Prayer of Duplication";
//                prayerDescription.text = "Choose a card in your deck and duplicate it.";
//                var imageComponent = prayerImage.GetComponent<Image>();
//                imageComponent.sprite = prayerIcons[1];
//                var background = GetComponent<Image>();
//                background.color = Color.black;
//            }

//            else if (prayerEvent.Method.Name == "RandomDestroyThree")
//            {
//                prayerName.text = "Prayer of Chaos";
//                prayerDescription.text = "Destroy three random cards in your deck.";
//                var imageComponent = prayerImage.GetComponent<Image>();
//                imageComponent.sprite = prayerIcons[3];
//                var background = GetComponent<Image>();
//                background.color = Color.black;
//            }

//            else if (prayerEvent.Method.Name == "Destruction")
//            {
//                prayerName.text = "Prayer of Destruction";
//                prayerDescription.text = "Choose a card in your deck and destroy it.";
//                var imageComponent = prayerImage.GetComponent<Image>();
//                imageComponent.sprite = prayerIcons[0];
//                var background = GetComponent<Image>();
//                background.color = Color.black;
//            }

//            else if (prayerEvent.Method.Name == "Upgrade")
//            {
//                prayerName.text = "Prayer of Evolution";
//                prayerDescription.text = "Choose a card in your deck and replace it with a random one of higher level.";
//                var imageComponent = prayerImage.GetComponent<Image>();
//                imageComponent.sprite = prayerIcons[2];
//                var background = GetComponent<Image>();
//                background.color = Color.black;
//            }
//        }
//    }
//}
