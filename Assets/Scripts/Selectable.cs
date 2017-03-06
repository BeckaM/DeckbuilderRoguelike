using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace Assets.Scripts
{

    public class Selectable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public Outline outline;

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var selectedCard = eventData.pointerPress;
            GameManager.instance.deckPanel.SelectCard(selectedCard);
        }
    }
}