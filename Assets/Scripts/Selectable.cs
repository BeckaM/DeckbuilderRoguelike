using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace Assets.Scripts
{

    public class Selectable : MonoBehaviour, IPointerClickHandler, ISelectHandler, IDeselectHandler
    {
        public Outline outline;
        public bool muliganKeep;
        public static CardManager selectedCard;
        public enum SelectContext { DeckPanel, ModalPanel, Muligan, NoSelect }
        public static SelectContext selectContext= SelectContext.NoSelect;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (selectContext == SelectContext.Muligan)
            {
                if (muliganKeep)
                {
                    muliganKeep = false;
                    RedOutline();
                }
                else
                {
                    muliganKeep = true;
                    ClearOutline();
                }

            }
            else
            {
                eventData.selectedObject = gameObject;
                selectedCard = gameObject.GetComponent<CardManager>();
            }
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
        
        public void OnSelect(BaseEventData eventData)
        {
            Debug.Log(eventData.selectedObject + " selected.");
            
            if (selectContext == SelectContext.DeckPanel)
            {
                GameManager.instance.dungeonUI.deckPanel.Select();
                GreenOutline();
            }
            else if (selectContext == SelectContext.ModalPanel)
            {
                GameManager.instance.dungeonUI.modalPanel.SelectReward();
                GreenOutline();
            }
           
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Debug.Log(eventData.selectedObject + " deselected.");

            ClearOutline();
        }
    }
}