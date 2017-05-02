using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CardImagePanel : MonoBehaviour
    {

        //public Sprite[] sprites;
        //public Sprite[] highlights;
        //public Sprite[] glows;
        public Image cardImage;
        //public Image highlight;
        //public Image glow;
        //public Image imageBackground;
        //public Image backgroundGlow;

        public GameObject fullDescriptionPanel;
        public GameObject imagePanel;

        public bool descriptionToggle = false;
        public bool animating = false;


        public void PopulateCardImage(Card card)
        {
            //Set Image 
            Sprite image = Resources.Load("CardImages/" + card.spriteIcon, typeof(Sprite)) as Sprite;

            if (image == null)
            {
                cardImage.sprite = Resources.Load("CardImages/" + "Placeholder", typeof(Sprite)) as Sprite;
            }
            else
            {
                cardImage.sprite = image;
            }

        }


        public void ShowFullDescription()
        {
            descriptionToggle = true;
            if (!animating)
            {
                animating = true;

                StartCoroutine(ShowDescriptionAnimation());
            }
        }

        public void HideFullDescription()
        {

            descriptionToggle = false;

        }

        private IEnumerator ShowDescriptionAnimation()
        {
            yield return StartCoroutine(Flip(90));

            fullDescriptionPanel.SetActive(true);
            imagePanel.gameObject.SetActive(false);

            yield return StartCoroutine(Flip(90));

            yield return new WaitForSeconds(2);
            while (descriptionToggle)
            {
                yield return new WaitForSeconds(1);
            }

            yield return StartCoroutine(HideDescriptionAnimation());
            animating = false;
        }

        private IEnumerator HideDescriptionAnimation()
        {

            yield return StartCoroutine(Flip(90));

            fullDescriptionPanel.SetActive(false);
            //     backgroundGlow.gameObject.SetActive(true);
            imagePanel.gameObject.SetActive(true);

            yield return StartCoroutine(Flip(90));
            animating = false;


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

        public void ResetPanel()
        {
            this.transform.localRotation = Quaternion.identity;
            fullDescriptionPanel.SetActive(false);
            //       backgroundGlow.gameObject.SetActive(true);
            cardImage.gameObject.SetActive(true);

        }

    }
}