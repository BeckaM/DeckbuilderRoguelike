﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{

    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Transform parentToReturnTo = null;
        public Transform placeholderParent = null;
        public GameObject panel;
        private static bool beingDragged = false;
        public CardManager cardManager;

        public float distance;

        GameObject placeholder = null;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!cardManager.IsDragable) return;
            beingDragged = true;

            //Debug.Log("OnBeginDrag");

            //placeholder = new GameObject("placeholder", typeof(RectTransform));
            //placeholder.transform.SetParent(this.transform.parent, false);
            //LayoutElement le = placeholder.AddComponent<LayoutElement>();
            //le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
            //le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
            //le.flexibleWidth = 0;
            //le.flexibleHeight = 0;

            //placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

            //parentToReturnTo = this.transform.parent;
            //placeholderParent = parentToReturnTo;
            //this.transform.SetParent(this.transform.parent.parent, false);

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!cardManager.IsDragable) return;

            //Debug.Log ("OnDrag");

            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            distance = Vector3.Distance(transform.position, CardgameManager.instance.cam.ScreenToWorldPoint(eventData.position));

            var point = ray.GetPoint(distance);

            transform.position = new Vector3(point.x, point.y);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y);

            if (placeholder.transform.parent != placeholderParent)
                placeholder.transform.SetParent(placeholderParent);

            int newSiblingIndex = placeholderParent.childCount;

            for (int i = 0; i < placeholderParent.childCount; i++)
            {
                if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
                {
                    newSiblingIndex = i;
                    if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                        newSiblingIndex--;
                    break;
                }
            }
            placeholder.transform.SetSiblingIndex(newSiblingIndex);
        }

        public void OnEndDrag(PointerEventData eventData)
        {            
            if (!cardManager.IsDragable) return;

            this.transform.SetParent(parentToReturnTo, false);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

            Destroy(placeholder);
            Debug.Log("OnEndDrag. Card is playabe: " + cardManager.IsPlayable.ToString() + ". Was dropped on: " + placeholderParent.name);
            if (parentToReturnTo.name == "Tabletop" && cardManager.IsPlayable)
            {
                CardgameManager.instance.PlaceCard(cardManager);
            }
            else
            {
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            transform.transform.localScale = transform.localScale * 0.666f;
            cardManager.imagePanel.ShowFullDescription(false);
            beingDragged = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!beingDragged)
            {
                placeholder = new GameObject("placeholder", typeof(RectTransform));
                placeholder.transform.SetParent(this.transform.parent, false);
                LayoutElement le = placeholder.AddComponent<LayoutElement>();
                le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
                le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
                le.flexibleWidth = 0;
                le.flexibleHeight = 0;

                placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

                parentToReturnTo = this.transform.parent;
                placeholderParent = parentToReturnTo;

                this.transform.SetParent(this.transform.parent.parent, true);

                // positionToReturnTo = transform.localPosition;
                //  transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -200f);
                transform.transform.localScale = transform.localScale * 1.5f;
                cardManager.imagePanel.ShowFullDescription(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!beingDragged)
            {
                this.transform.SetParent(parentToReturnTo, false);
                this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

                //  transform.localPosition = positionToReturnTo;
                transform.transform.localScale = transform.localScale * 0.666f;
                cardManager.imagePanel.ShowFullDescription(false);
                Destroy(placeholder);
            }
        }
    }
}