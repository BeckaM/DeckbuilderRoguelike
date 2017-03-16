using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CardImagePanel : MonoBehaviour
    {

        public Sprite[] sprites;
        public Sprite[] highlights;
        public Sprite[] glows;
        public Image cardImage;
        public Image highlight;
        public Image glow;
        public Image imageBackground;
        public Image backgroundGlow;

        public GameObject fullDescription;
        public bool descriptionToggle=false;


        public void PopulateCardImage(Card card)
        {
            //Set Image                       
            cardImage.sprite = sprites[card.spriteIcon];
            cardImage.color = card.spriteColor;
            highlight.sprite = highlights[card.spriteIcon];
            highlight.color = card.spriteHighlightColor;
            glow.sprite = glows[card.spriteIcon];
            glow.color = card.spriteGlowColor;

            imageBackground.color = card.spriteBackgroundColor;
            backgroundGlow.color = card.backgroundGlowColor;
        }


        public void ShowFullDescription(bool show)
        {
            if (show == descriptionToggle)
            {
                return;
            }
            else if (show == true)
            {
                descriptionToggle = true;
                StartCoroutine(ShowDescriptionAnimation());

            }
            else if (show == false)
            {
                descriptionToggle = false;
                StartCoroutine(HideDescriptionAnimation());
            }
        }

        private IEnumerator ShowDescriptionAnimation()
        {
            yield return StartCoroutine(Flip(90));

            fullDescription.SetActive(true);
            backgroundGlow.gameObject.SetActive(false);
            cardImage.gameObject.SetActive(false);

            yield return StartCoroutine(Flip(90));
        }

        private IEnumerator HideDescriptionAnimation()
        {
            yield return StartCoroutine(Flip(90));

            fullDescription.SetActive(false);
            backgroundGlow.gameObject.SetActive(true);
            cardImage.gameObject.SetActive(true);

            yield return StartCoroutine(Flip(90));
        }


        private IEnumerator Flip(float rotation)
        {
            float deltaRot = 10f;
            float rotationSpeed = 0.01f;
            var degreesToRotate = rotation;

            while (degreesToRotate > 0.001f)
            {
                this.transform.Rotate(0, deltaRot, 0);

                //Recalculate the remaining distance after moving.
                degreesToRotate = degreesToRotate - deltaRot;

                //Return and loop until sqrRemainingDistance is close enough to zero to end the function
                yield return new WaitForSeconds(rotationSpeed);
            }
        }


    }
}