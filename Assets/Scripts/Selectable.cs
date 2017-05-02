using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace Assets.Scripts
{

    public class Selectable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
    {
        public Outline outline;
        public bool muliganKeep;
        static CardManager selectedCard;


        public void OnPointerClick(PointerEventData eventData)
        {
           
            eventData.selectedObject = gameObject;
            selectedCard = gameObject.GetComponent<CardManager>();
            
            
            //var selection = eventData.pointerPress;
            //if (GameManager.instance.dungeonUI.modalPanel.isActive)
            //{
            //    GameManager.instance.dungeonUI.modalPanel.Select(selection);
            //}
            //else if (GameManager.instance.dungeonUI.deckPanel.isActive)
            //{
            //    GameManager.instance.dungeonUI.deckPanel.Select(selection);
            //}
            //else if (CardgameManager.instance.isActiveAndEnabled)
            //{

            //    if (CardgameManager.instance.cardgameUI.muliganPanelScript.isActiveAndEnabled)
            //    {
            //        CardgameManager.instance.cardgameUI.muliganPanelScript.Select(selection);
            //    }
            //    else
            //    {
            //        CardgameManager.instance.Select(selection);
            //    }
            //}
        }

        public void RedOutline()
        {
            outline.enabled = true;
            outline.effectColor = Color.red;
        }

        public void GreenOutline()
        {
            outline.enabled = true;
            outline.effectColor = Color.green;
        }

        public void ClearOutline()
        {
            outline.enabled = false;
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
           

        }

        public void OnPointerUp(PointerEventData eventData)
        {
           
        }

        public void OnSelect(BaseEventData eventData)
        {
            Debug.Log(eventData.selectedObject + " selected.");
            GreenOutline();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Debug.Log(eventData.selectedObject + " deselected.");
            
            ClearOutline();
        }
    }
}