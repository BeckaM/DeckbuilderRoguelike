using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Choice : MonoBehaviour
    {

        public GameObject cardObject;

        public GameObject cardDescription;
        public GameObject cardImage;
        public GameObject cardName;
        public Sprite goldIcon;
        public List<Sprite> prayerIcons;

        public void PopulateChoice(Card card)
        {
            //  var transformer = this.transform;

            //Set Image            
            var imageComponent = cardImage.GetComponent<Image>();
            imageComponent.sprite = cardObject.GetComponent<CardManager>().sprites[card.spriteIcon];

            //Set Card Title
            var titleComponent = cardName.GetComponent<Text>();
            titleComponent.text = card.cardName;

            //Set Card Description
            var cardtext = cardDescription.GetComponent<Text>();
            cardtext.text = card.cardText;

            //Set Card Background.
            var background = GetComponent<Image>();
            background.color = card.backgroundColor;
        }

        public void PopulateChoice(int gold)
        {
            //   var transformer = this.transform;

            //Set Image            
            var imageComponent = cardImage.GetComponent<Image>();
            imageComponent.sprite = goldIcon;

            //Set Card Title
            var titleComponent = cardName.GetComponent<Text>();
            titleComponent.text = "Gold!";

            //Set Card Description
            var cardtext = cardDescription.GetComponent<Text>();
            cardtext.text = "" + gold + " Gold Pieces.";

            //Set Card Background.
            var background = GetComponent<Image>();
            background.color = Color.gray;
        }

        public void PopulateChoice(Prayer prayer)
        {
            //   var transformer = this.transform;

            //Set Image            
            var imageComponent = cardImage.GetComponent<Image>();
            imageComponent.sprite = prayerIcons[prayer.sprite];

            //Set Card Title
            var titleComponent = cardName.GetComponent<Text>();
            titleComponent.text = prayer.name;

            //Set Card Description
            var cardtext = cardDescription.GetComponent<Text>();
            cardtext.text = prayer.description;

            //Set Card Background.
            var background = GetComponent<Image>();
            background.color = Color.blue;
        }



    }
}
