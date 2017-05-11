using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{

    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public Transform parentToReturnTo = null;
        public Transform placeholderParent = null;
        public GameObject panel;

        public float distance;

        GameObject placeholder = null;

        public void OnBeginDrag(PointerEventData eventData)
        {
            var card = GetComponent<CardManager>();
            if (!card.IsDragable) return;

            //Debug.Log("OnBeginDrag");

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
            this.transform.SetParent(this.transform.parent.parent, false);

            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var card = GetComponent<CardManager>();
            if (!card.IsDragable) return;

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
            var card = GetComponent<CardManager>();
            if (!card.IsDragable) return;

            this.transform.SetParent(parentToReturnTo, false);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            Destroy(placeholder);
            Debug.Log("OnEndDrag. Card is playabe: " + card.IsPlayable.ToString() + ". Was dropped on: " + placeholderParent.name);
            if (parentToReturnTo.name == "Tabletop" && card.IsPlayable)
            {

                CardgameManager.instance.PlaceCard(card);
            }
        }



    }
}