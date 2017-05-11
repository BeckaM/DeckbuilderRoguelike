using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace Assets.Scripts
{

    public class Selectable : MonoBehaviour, IPointerClickHandler
    {
        public Outline outline;
        public bool muliganKeep;
        public static GameObject selectedObject;
        public enum SelectContext { DeckPanel, ModalPanel, Muligan, NoSelect }
        public static SelectContext selectContext = SelectContext.NoSelect;


    
        public void OnEnable()
        {
            ClearOutline();
        }

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
            else if(selectContext == SelectContext.ModalPanel)
            {
               if(selectedObject) selectedObject.GetComponent<Selectable>().ClearOutline();
                selectedObject = gameObject;
                GameManager.instance.dungeonUI.modalPanel.SelectReward();
                GreenOutline();
            }
            else if (selectContext == SelectContext.DeckPanel)
            {
                if (selectedObject) selectedObject.GetComponent<Selectable>().ClearOutline();
                selectedObject = gameObject;
                GameManager.instance.dungeonUI.deckPanel.Select();
                GreenOutline();
            }
        }

        private void RedOutline()
        {
            outline.enabled = true;
            outline.effectColor = Color.red;
        }

        private void GreenOutline()
        {
            outline.enabled = true;
            outline.effectColor = Color.green;
        }

        public void ClearOutline()
        {
            outline.enabled = false;

        }
    }
}