using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CardMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        GameObject placeholder = null;
        Transform parentToReturnTo = null;
        Vector3 positionToReturnTo;

        public void OnPointerEnter(PointerEventData eventData)
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

            this.transform.SetParent(this.transform.parent.parent, true);

            positionToReturnTo = transform.localPosition;
          //  transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -200f);
            transform.transform.localScale = transform.localScale * 1.5f;
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.transform.SetParent(parentToReturnTo, false);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

          //  transform.localPosition = positionToReturnTo;
            transform.transform.localScale = transform.localScale * 0.666f;
            Destroy(placeholder);
        }
    }
}

